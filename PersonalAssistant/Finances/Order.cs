using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace PersonalAssistant.Finances
{
    public class Order
    {
        private int id;
        private string ticker;
        private double numShares;
        private double price;
        private DateTime date;

        public Order(int id, DateTime date, string ticker, double numShares, double price)
        {
            this.id = id;
            constructorSaveAttributes(date, ticker, numShares, price);
        }

        public Order(DateTime date, string ticker, double numShares, double price)
        {
            id = IdHelper.nextId("order");
            constructorSaveAttributes(date, ticker, numShares, price);
        }

        private void constructorSaveAttributes(DateTime date, string ticker, double numShares, double price)
        {
            this.date = date;
            this.ticker = ticker;
            this.numShares = numShares;
            this.price = price;
        }

        public void save()
        {
            string saveOrderSql = "INSERT INTO ORDERS (ID, DATE, TICKER, SHARES, PRICE) VALUES (@id, @date, @ticker, @shares, @price)";
            using (SQLiteConnection dbConnection = dbHelper.getDbConnection())
            {
                dbConnection.Open();
                using (SQLiteCommand saveOrderCommand = new SQLiteCommand(saveOrderSql, dbConnection))
                {
                    saveOrderCommand.Parameters.AddWithValue("@id", id);
                    saveOrderCommand.Parameters.AddWithValue("@date", date);
                    saveOrderCommand.Parameters.AddWithValue("@ticker", ticker);
                    saveOrderCommand.Parameters.AddWithValue("@shares", numShares);
                    saveOrderCommand.Parameters.AddWithValue("@price", price);
                    saveOrderCommand.ExecuteNonQuery();
                }
            }
        }
    }
}