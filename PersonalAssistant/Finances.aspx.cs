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
    public partial class Finances : System.Web.UI.Page
    {
        private List<string> watchList;
        private bool watchListLoaded;
        private const string watchListFileName = "~/data/watchlist.ser";
        private string watchListFilePath;
        private const string yahooFinanceQuoteUrlBase = "http://finance.yahoo.com/quote/";
        protected void Page_Load(object sender, EventArgs e)
        {
            watchListFilePath = Server.MapPath(watchListFileName);
            loadWatchList();
        }
        public bool getLastCloseFromYahoo(string ticker, out double price)
        {
            string yahooFinanceQuoteUrl = yahooFinanceQuoteUrlBase + ticker;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(yahooFinanceQuoteUrl);
            HtmlNode quoteNode = document.DocumentNode.SelectSingleNode("//tr/td[contains(., 'Previous Close')]/following-sibling::td");
            return Double.TryParse(quoteNode.InnerText, out price);
        }

        protected void btnAddToWatchList_Click(object sender, EventArgs e)
        {
            string ticker = txtAddToWatchList.Value;
            watchList.Add(ticker);
            IOHelper.saveSerializable<List<string>>(watchListFilePath, watchList, watchListLoaded);
        }

        private void loadWatchList()
        {
            if (IOHelper.loadSerializable<List<string>>(watchListFilePath, out watchList))
            {
                renderWatchList();
                watchListLoaded = true;
            }
            else
            {
                watchList = new List<string>();
                watchListLoaded = false;
            }
        }

        private void renderWatchList()
        {
            foreach (string ticker in watchList)
            {
                tblWatchList.Attributes["class"] = "show";
                HtmlTableRow watchListRow = new HtmlTableRow();
                HtmlTableCell tickerCell = new HtmlTableCell();
                tickerCell.InnerText = ticker;
                watchListRow.Cells.Add(tickerCell);
                tblWatchList.Rows.Add(watchListRow);
            }
        }
    }
}