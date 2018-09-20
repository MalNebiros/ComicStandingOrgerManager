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
    internal class CustomerDatabaseAdapter
    {
        private DatabaseConnectionManager _databaseConnectionManager;

        CustomerDatabaseAdapter(DatabaseConnectionManager databaseConnectionManager)
        {
            _databaseConnectionManager = databaseConnectionManager;
        }

        public IList<ICustomer> GetCustomers()
        {
            List<ICustomer> customers = new List<ICustomer>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {CustomersTableStructure.TableName}";

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
            string sql = $"SELECT * FROM {CustomersTableStructure.TableName} WHERE {CustomersTableStructure.FirstName}={firstName} AND {CustomersTableStructure.LastName}={lastName}";

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
            var firstName = (string)reader[$"{CustomersTableStructure.FirstName}"];
            var lastName = (string)reader[$"{CustomersTableStructure.LastName}"];
            var email = (string)reader[$"{CustomersTableStructure.Email}"];
            return new Customer(firstName, lastName, email);
        }
    }
}
