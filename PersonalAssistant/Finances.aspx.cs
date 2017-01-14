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

namespace PersonalAssistant
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
        /// <summary>
        /// Load data when the page loads. Maps the relative watchlist path on the server and loads the watchlist onto the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            watchListFilePath = Server.MapPath(watchListFileName);  // map the relative file location on the server.
            loadWatchList();    // load watchList locally and render to the page
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
            HtmlDocument document = web.Load(yahooFinanceQuoteUrl); // load the yahoo finance page for the given ticker
            // get the next td cell after the 'Previous Close' label td cell
            HtmlNode quoteNode = document.DocumentNode.SelectSingleNode("//tr/td[contains(., 'Previous Close')]/following-sibling::td");
            if (quoteNode == null)  // couldn't find the quote
            {
                price = default(double);
                return false;
            }
            return Double.TryParse(quoteNode.InnerText, out price); // return true if we find a valid price
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
            if (IOHelper.loadSerializable<HashSet<string>>(watchListFilePath, out watchList))
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
    }
}