using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ComicStandingOrderManager.DatabaseManagement
{
    class DatabaseConnectionManager
    {
        private DatabaseVersionManager databaseVersionManager;
        private const string _databaseFileName = "standingOrders.sqlite";

        public DatabaseConnectionManager()
        {
            databaseVersionManager.EnsureDatabaseVersionIsCorrect(_databaseFileName);
        }
        
    }
}
