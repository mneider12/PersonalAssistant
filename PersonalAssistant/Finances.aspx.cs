using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PersonalAssistant
{
    public partial class Finances : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebRequest visaQuoteRequest = WebRequest.Create("http://finance.yahoo.com/quote/v");
            using (WebResponse visaQuoteResponse = visaQuoteRequest.GetResponse())
            {
                using (Stream visaQuoteStream = visaQuoteResponse.GetResponseStream())
                {
                    using (StreamReader visaQuoteStreamReader = new StreamReader(visaQuoteStream))
                    {
                        string quoteLine;
                        while ((quoteLine = visaQuoteStreamReader.ReadLine()) != null)
                        {
                            Console.WriteLine(quoteLine);
                        }
                    }
                }
            }

        }
    }
}