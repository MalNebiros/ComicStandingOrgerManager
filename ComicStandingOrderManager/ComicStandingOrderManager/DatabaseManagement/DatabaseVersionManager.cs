using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;


namespace ComicStandingOrderManager.DatabaseManagement
{
    class DatabaseVersionManager
    {
        const int LatestDatabaseVersion = 0;

        internal void EnsureDatabaseVersionIsCorrect(string databaseFileName)
        {
            if (!System.IO.File.Exists(databaseFileName))
            {
                CreateDatabase(databaseFileName);
            }
            else
            {
                var version = CheckVersion(databaseFileName);
                if (version < LatestDatabaseVersion)
                {
                    UpdateDatabase(databaseFileName);
                }
            }
        }

        private void UpdateDatabase(string databaseFileName)
        {
            throw new NotImplementedException();
        }

        private int CheckVersion(string databaseFileName)
        {
            SQLiteConnection databaseConnection = new SQLiteConnection($"Data Source={databaseFileName};Version=3;");
            databaseConnection.Open();
            try
            {
                string sql = $"select * from {SystemTableStructure.TableName} WHERE {SystemTableStructure.Key}=DatabaseVersion";
                SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                string keyCheck = (string)reader[$"{SystemTableStructure.Key}"];
                if (keyCheck == "DatabaseVersion")
                {
                    var foundDatabaseVersion = (int)reader[$"{SystemTableStructure.Value}"];
                    return foundDatabaseVersion;
                }
                return -1;
            }
            finally
            {
                databaseConnection.Close();
            }
        }

        private void CreateDatabase(string databaseFileName)
        {
            SQLiteConnection.CreateFile(databaseFileName);

            SQLiteConnection databaseConnection = new SQLiteConnection($"Data Source={databaseFileName};Version=3;");

            databaseConnection.Open();
            try
            {
                CreateTables(databaseConnection);
            }
            finally
            {
                databaseConnection.Close();
            }
        }

        private void CreateTables(SQLiteConnection databaseConnection)
        {
            CreateCustomersTable(databaseConnection);
            CreateComicSeriesTable(databaseConnection);
            CreateStandingOrdersTable(databaseConnection);
        }

        private void CreateSystemTable(SQLiteConnection databaseConnection)
        {
            string sql = $"CREATE TABLE {SystemTableStructure.TableName} ({SystemTableStructure.Key} varchar(50), {SystemTableStructure.Value} varchar(50)";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();

            sql = $"INSERT INTO {SystemTableStructure.TableName} ({SystemTableStructure.Key}, {SystemTableStructure.Value}) values ('DatabaseVersion', {LatestDatabaseVersion})";

            command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateCustomersTable(SQLiteConnection databaseConnection)
        {
            string sql = $"CREATE TABLE {CustomersTableStructure.TableName} ({CustomersTableStructure.Id} int, {CustomersTableStructure.FirstName} varchar(50), {CustomersTableStructure.LastName} varchar(50), {CustomersTableStructure.Email} varchar(100), KEY({CustomersTableStructure.FirstName}, {CustomersTableStructure.LastName}))";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateComicSeriesTable(SQLiteConnection databaseConnection)
        {
            string sql = $"CREATE TABLE {ComicSeriesTableStructure.TableName} ({ComicSeriesTableStructure.Id} int, {ComicSeriesTableStructure.Name} varchar(255), KEY({ComicSeriesTableStructure.Name})";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateStandingOrdersTable(SQLiteConnection databaseConnection)
        {
            string sql = $"CREATE TABLE {StandingOrdersTableStructure.TableName} ({StandingOrdersTableStructure.CustomerId} int, {StandingOrdersTableStructure.SeriesId} int, FOREIGN KEY({StandingOrdersTableStructure.CustomerId}) REFERENCES {CustomersTableStructure.TableName}({CustomersTableStructure.Id}), FOREIGN KEY({StandingOrdersTableStructure.SeriesId}) REFERENCES {ComicSeriesTableStructure.TableName}({ComicSeriesTableStructure.Id}))";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }
    }
}
