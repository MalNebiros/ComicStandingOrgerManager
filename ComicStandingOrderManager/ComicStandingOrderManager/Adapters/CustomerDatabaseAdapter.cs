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
    class CustomerDatabaseAdapter
    {
        private DatabaseConnectionManager _databaseConnectionManager;
        private const string _customerTableName = "Customers";
        private const string _customerFirstName = "FirstName";
        private const string _customerLastName = "LastName";
        private const string _customerEmail = "Email";
        private

        public CustomerDatabaseAdapter(DatabaseConnectionManager databaseConnectionManager)
        {
            _databaseConnectionManager = databaseConnectionManager;
        }

        public IList<ICustomer> GetCustomers()
        {
            List<ICustomer> customers = new List<ICustomer>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {_customerTableName}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            connection.Open();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var customer = QueryEntryToCustomer(reader);
                customers.Add(customer);
            }
            connection.Close();
            return customers;
        }

        public ICustomer GetCustomerByName(string firstName, string lastName)
        {
            List<ICustomer> customers = new List<ICustomer>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {_customerTableName} WHERE {_customerFirstName}={firstName} AND {_customerLastName}={lastName}";

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
                if (customers.Count == 1)
                {
                    return customers[0];
                }
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        private Customer QueryEntryToCustomer(SQLiteDataReader reader)
        {
            var firstName = (string)reader[$"{_customerFirstName}"];
            var lastName = (string)reader[$"{_customerLastName}"];
            var email = (string)reader[$"{_customerEmail}"];
            return new Customer(firstName, lastName, email);
        }
    }
}
