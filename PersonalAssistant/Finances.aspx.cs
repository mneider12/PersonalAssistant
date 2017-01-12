using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HtmlAgilityPack;


namespace PersonalAssistant
{
    public partial class Finances : System.Web.UI.Page
    {
        private const string yahooFinanceQuoteUrlBase = "http://finance.yahoo.com/quote/";
        protected void Page_Load(object sender, EventArgs e)
        {
            double price;
            if (getLastCloseFromYahoo("V", out price))
            {
                spnVisaPrice.InnerText = price.ToString();
            }
        }
        public bool getLastCloseFromYahoo(string ticker, out double price)
        {
            string yahooFinanceQuoteUrl = yahooFinanceQuoteUrlBase + ticker;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(yahooFinanceQuoteUrl);
            HtmlNode quoteNode = document.DocumentNode.SelectSingleNode("//tr/td[contains(., 'Previous Close')]/following-sibling::td");
            return Double.TryParse(quoteNode.InnerText, out price);
        }
    }
}