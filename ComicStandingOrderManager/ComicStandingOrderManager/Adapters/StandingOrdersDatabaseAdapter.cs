using ComicStandingOrderManager.DatabaseManagement;
using ComicStandingOrderManager.DataModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.Adapters
{
    class StandingOrdersDatabaseAdapter
    {
        private DatabaseConnectionManager _databaseConnectionManager;

        StandingOrdersDatabaseAdapter(DatabaseConnectionManager databaseConnectionManager)
        {
            _databaseConnectionManager = databaseConnectionManager;
        }

        public bool AddNewStandingOrder(int customerId, int seriesId)
        {
            var connection = _databaseConnectionManager.GetConnection();
            var sql = $"INSERT INTO {StandingOrdersTableStructure.TableName} ({StandingOrdersTableStructure.CustomerId}, {StandingOrdersTableStructure.SeriesId}) values ({customerId}, {seriesId})";
            var command = new SQLiteCommand(sql, connection);

            connection.Open();
            var returnCode = command.ExecuteNonQuery();
            connection.Close();
            return returnCode == 1;
        }

        public bool DeleteStandingOrder(int customerId, int seriesId)
        {
            var connection = _databaseConnectionManager.GetConnection();
            var sql = $"DELETE FROM {StandingOrdersTableStructure.TableName} WHERE {StandingOrdersTableStructure.CustomerId}={customerId} AND {StandingOrdersTableStructure.SeriesId}={seriesId}";
            var command = new SQLiteCommand(sql, connection);
            try
            {
                connection.Open();
                var returnCode = command.ExecuteNonQuery();
                return returnCode == 1;
            }
            finally
            {
                connection.Close();
            }
        }

        public IDictionary<string, int> GetAggregatedStandingOrders()
        {
            Dictionary<string, int> seriesOrderCounts = new Dictionary<string, int>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT {ComicSeriesTableStructure.TableName}.{ComicSeriesTableStructure.Name}, COUNT(*) AS 'Subscribers' FROM {StandingOrdersTableStructure.TableName} INNER JOIN {ComicSeriesTableStructure.TableName} ON {StandingOrdersTableStructure.TableName}.{StandingOrdersTableStructure.SeriesId}={ComicSeriesTableStructure.TableName}.{ComicSeriesTableStructure.Id} GROUP BY {ComicSeriesTableStructure.TableName}.{ComicSeriesTableStructure.Id}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var series = (string)reader[$"{ComicSeriesTableStructure.Name}"];
                    var subscribers = (int)reader[$"Subscribers"];


                    seriesOrderCounts.Add(series, subscribers);
                }
                return seriesOrderCounts;
            }
            finally
            {
                connection.Close();
            }
        }

        public IList<IComicSeries> GetSubscriptionsForCustomer(int customerId)
        {
            List<IComicSeries> series = new List<IComicSeries>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {StandingOrdersTableStructure.TableName} INNER JOIN {ComicSeriesTableStructure.TableName} ON {StandingOrdersTableStructure.TableName}.{StandingOrdersTableStructure.SeriesId}={ComicSeriesTableStructure.TableName}.{ComicSeriesTableStructure.Id} WHERE {StandingOrdersTableStructure.CustomerId}={customerId}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            connection.Open();
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var currentSeries = QueryEntryToSeries(reader);
                    series.Add(currentSeries);
                }
                return series;
            }
            finally
            {
                connection.Close();
            }
        }

        private ComicSeries QueryEntryToSeries(SQLiteDataReader reader)
        {
            var id = (int)reader[$"{ComicSeriesTableStructure.Id}"];
            var name = (string)reader[$"{ComicSeriesTableStructure.Name}"];
            var publisher = (string)reader[$"{ComicSeriesTableStructure.Publisher}"];
            return new ComicSeries(id, name, publisher);
        }

        public IList<ICustomer> GetCustomersSubscribedToSeries(int seriesId)
        {
            List<ICustomer> customers = new List<ICustomer>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {StandingOrdersTableStructure.TableName} INNER JOIN {CustomersTableStructure.TableName} ON {StandingOrdersTableStructure.TableName}.{StandingOrdersTableStructure.SeriesId}={ComicSeriesTableStructure.TableName}.{ComicSeriesTableStructure.Id} WHERE {StandingOrdersTableStructure.SeriesId}={seriesId}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            connection.Open();
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var customer = QueryEntryToCustomer(reader);
                    customers.Add(customer);
                }
                return customers;
            }
            finally
            {
                connection.Close();
            }
        }

        private Customer QueryEntryToCustomer(SQLiteDataReader reader)
        {
            var firstName = (string)reader[$"{CustomersTableStructure.FirstName}"];
            var lastName = (string)reader[$"{CustomersTableStructure.LastName}"];
            var email = (string)reader[$"{CustomersTableStructure.Email}"];
            return new Customer(firstName, lastName, email);
        }
    }
}
