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
                string sql = "select * from System WHERE Key=DatabaseVersion";
                SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                string keyCheck = (string)reader["Key"];
                if (keyCheck == "DatabaseVersion")
                {
                    var foundDatabaseVersion = (int)reader["Value"];
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
            string sql = "CREATE TABLE System (Key varchar(50), Value varchar(50)";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();

            sql = $"INSERT INTO System (Key, Value) values ('DatabaseVersion', {LatestDatabaseVersion})";

            command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateCustomersTable(SQLiteConnection databaseConnection)
        {
            string sql = "CREATE TABLE Customers (Id int, FirstName varchar(50), LastName varchar(50), Email varchar(100), KEY(FirstName, LastName))";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateComicSeriesTable(SQLiteConnection databaseConnection)
        {
            string sql = "CREATE TABLE ComicSeries (Id int, Name varchar(255), KEY(Name)";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateStandingOrdersTable(SQLiteConnection databaseConnection)
        {
            string sql = "CREATE TABLE Standingorders (CustomerId int, SeriesId int, FOREIGN KEY(CustomerId) REFERENCES Customers(Id), FOREIGN KEY(SeriesId) REFERENCES ComicSeries(Id))";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }
    }
}
