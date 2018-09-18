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
        }

        private void CreateCustomersTable()
        {

        }

        private void CreateComicSeriesTable()
        {

        }

        private void CreateStandingOrdersTable()
        {

        }
    }
}
