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

        public bool AddNewCustomer(Customer customer)
        {
            if(GetCustomerByName(customer.FirstName, customer.LastName) != null)
            {
                return false;
            }

            var connection = _databaseConnectionManager.GetConnection();
            var sql = $"INSERT INTO {CustomersTableStructure.TableName} ({CustomersTableStructure.Id}, {CustomersTableStructure.FirstName}, {CustomersTableStructure.LastName}, {CustomersTableStructure.Email}) values ({customer.Id}, '{customer.FirstName}', '{customer.LastName}', '{customer.Email}')";
            var command = new SQLiteCommand(sql, connection);

            connection.Open();
            var returnCode = command.ExecuteNonQuery();
            connection.Close();
            return returnCode == 1;
        }

        public bool DeleteCustomer(Customer customer)
        {
            if (GetCustomerByName(customer.FirstName, customer.LastName) != null)
            {

                var connection = _databaseConnectionManager.GetConnection();
                var sql = $"DELETE FROM {CustomersTableStructure.TableName} WHERE {CustomersTableStructure.FirstName}='{customer.FirstName}' AND {CustomersTableStructure.LastName}='{customer.LastName}'";
                var command = new SQLiteCommand(sql, connection);

                connection.Open();
                var returnCode = command.ExecuteNonQuery();
                connection.Close();
                return returnCode == 1;
            }
            else
            {
                return false;
            }
        }


        public IList<ICustomer> GetCustomers()
        {
            List<ICustomer> customers = new List<ICustomer>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {CustomersTableStructure.TableName}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                connection.Open();
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
