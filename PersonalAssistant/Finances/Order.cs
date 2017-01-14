using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalAssistant.Finances
{
    public class Order
    {
        private string ticker;
        private double numShares;
        private double price;
        private DateTime date;
        public Order(string ticker, double numShares, double price, DateTime date)
        {
            this.ticker = ticker;
            this.numShares = numShares;
            this.price = price;
            this.date = date;
        }
    }
}