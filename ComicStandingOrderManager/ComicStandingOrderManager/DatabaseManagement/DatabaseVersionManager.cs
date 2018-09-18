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
        internal void EnsureDatabaseVersionIsCorrect(string databaseFileName)
        {
            if (!System.IO.File.Exists(databaseFileName))
            {
                CreateDatabase(databaseFileName);
            }
        }

        private void CreateDatabase(string databaseFileName)
        {
            SQLiteConnection.CreateFile(databaseFileName);

            SQLiteConnection databaseConnection = new SQLiteConnection($"Data Source={databaseFileName};Version=3;");

            databaseConnection.Open();

            CreateTables(databaseConnection);

            databaseConnection.Close();
        }

        private void CreateTables(SQLiteConnection databaseConnection)
        {
            CreateCustomersTable(databaseConnection);
            CreateComicSeriesTable(databaseConnection);
            CreateStandingOrdersTable(databaseConnection);
        }

        private void CreateCustomersTable(SQLiteConnection databaseConnection)
        {
            string sql = "CREATE TABLE Customers (Id int, FirstName varchar(50), LastName varchar(50), Email varchar(100))";
            SQLiteCommand command = new SQLiteCommand(sql, databaseConnection);
            command.ExecuteNonQuery();
        }

        private void CreateComicSeriesTable(SQLiteConnection databaseConnection)
        {
            string sql = "CREATE TABLE ComicSeries (Id int, Name varchar(255))";
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
