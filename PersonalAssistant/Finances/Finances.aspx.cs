using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HtmlAgilityPack;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Data.SQLite;
using static PersonalAssistant.IOHelper;

namespace PersonalAssistant.Finances
{
    /// <summary>
    /// Finance page support
    /// </summary>
    public partial class Finances : System.Web.UI.Page
    {
        private HashSet<string> watchList;  // local copy of the watch list
        private bool watchListLoaded;   // if we successfully loaded the watch list from disk. If we did, we can overwrite the existing file later.
        private const string watchListFileName = "~/data/watchlist.ser";    // relative location of the watchlist on disk
        private string watchListFilePath;   // mapped file path on the server
        private const string yahooFinanceQuoteUrlBase = "http://finance.yahoo.com/quote/";  // url for quotes from yahoo
        private Dictionary<string, string> settings;
        /// <summary>
        /// Load data when the page loads. Maps the relative watchlist path on the server and loads the watchlist onto the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            watchListFilePath = Server.MapPath(watchListFileName);  // map the relative file location on the server.
            loadWatchList();    // load watchList locally and render to the page
            //loadSettings();
        }
        /// <summary>
        /// Scrape the html from yahoo for a stock quote
        /// </summary>
        /// <param name="ticker">stock ticker to lookeup</param>
        /// <returns></returns>
        public bool getLastCloseFromYahoo(string ticker, out double price)
        {
            string yahooFinanceQuoteUrl = yahooFinanceQuoteUrlBase + ticker;    // build url for ticker request
            HtmlWeb web = new HtmlWeb();
            HtmlNode quoteNode = tryGetPreviousClose(web, yahooFinanceQuoteUrl);
            if (quoteNode == null)  // couldn't find the quote
            {
                price = default(double);
                return false;
            }
            return Double.TryParse(quoteNode.InnerText, out price); // return true if we find a valid price
        }

        public HtmlNode tryGetPreviousClose(HtmlWeb web, string yahooFinanceQuoteUrl, int timesToRetry = 3, int delayBetweenRetries = 1000)
        {
            for (int retryNum = 1; retryNum <= timesToRetry; retryNum++)
            {
                try
                {
                    HtmlDocument document = web.Load(yahooFinanceQuoteUrl); // load the yahoo finance page for the given ticker
                                                                            // get the next td cell after the 'Previous Close' label td cell
                    return document.DocumentNode.SelectSingleNode("//tr/td[contains(., 'Previous Close')]/following-sibling::td");
                }
                catch (Exception)
                {
                    Thread.Sleep(delayBetweenRetries);  // wait a bit to retry
                    continue; //try again
                }
            }
            return null;    // retries failed
        }

        /// <summary>
        /// Add an ticker to the watchlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddToWatchList_Click(object sender, EventArgs e)
        {
            string ticker = txtAddToWatchList.Value.ToUpper();
            watchList.Add(ticker);
            IOHelper.saveSerializable<HashSet<string>>(watchListFilePath, watchList, watchListLoaded);
            addToWatchListTable(ticker);
        }

        private void loadWatchList()
        {
            if (IOHelper.loadSerializable<HashSet<string>>(watchListFilePath, out watchList) == FileLoadResult.FileLoaded)
            {
                renderWatchList();
                watchListLoaded = true;
            }
            else
            {
                watchList = new HashSet<string>();
                watchListLoaded = false;
            }
        }

        private void renderWatchList()
        {
            foreach (string ticker in watchList)
            {
                tblWatchList.Attributes["class"] = "show";
                addToWatchListTable(ticker);
            }
        }

        private void addToWatchListTable(string ticker)
        {
            const string failedToLoadQuote = "<ERR>";
            HtmlTableRow watchListRow = new HtmlTableRow();
            // Ticker cell
            HtmlTableCell tickerCell = new HtmlTableCell();
            tickerCell.InnerText = ticker;
            watchListRow.Cells.Add(tickerCell);
            // Last Close cell
            HtmlTableCell quoteCell = new HtmlTableCell();
            double quote;
            if (getLastCloseFromYahoo(ticker, out quote))
            {
                quoteCell.InnerText = quote.ToString();
            }
            else
            {
                quoteCell.InnerText = failedToLoadQuote;
            }
            watchListRow.Cells.Add(quoteCell);
            tblWatchList.Rows.Add(watchListRow);
        }

        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            DateTime date;
            string ticker;
            double shares;
            double price;

            validateDate(dateOrderDate.Value, out date);
            validateTicker(txtOrderTicker.Value, out ticker);
            Double.TryParse(numOrderShares.Value, out shares);
            Double.TryParse(numOrderPrice.Value, out price);
            Order newOrder = new Order(date, ticker, shares, price);
            newOrder.save();
        }

        private bool validateDate(string dateInput, out DateTime validatedDate)
        {
            if (!DateTime.TryParse(dateInput, out validatedDate))    // couldn't parse date
            {
                return false;
            }
            if (validatedDate > DateTime.Today)         // don't allow future dates
            {
                return false;
            }
            return true;    // validation passed, validated date in out parameter
        }

        private bool validateTicker(string tickerInput, out string validatedTicker)
        {
            if (tickerInput == null)    // can't be null
            {
                validatedTicker = null;
                return false;
            }
            validatedTicker = tickerInput.ToUpper();    // must be all uppercase. Won't fail for this.
            if (!validatedTicker.All(Char.IsLetter))    // must only contain letters
            {
                return false;
            }
            if (validatedTicker.Length > 4)             // max length of a valid stock ticker is 4
            {
                return false;
            }
            return true;        // passed validation. validated ticker is in out parameter.
        }

    }
}