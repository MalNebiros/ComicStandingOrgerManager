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
    class SeriesDatabaseAdapter
    {
        private DatabaseConnectionManager _databaseConnectionManager;

        SeriesDatabaseAdapter(DatabaseConnectionManager databaseConnectionManager)
        {
            _databaseConnectionManager = databaseConnectionManager;
        }

        public bool AddNewSeries(ComicSeries series)
        {
            if (GetSeriesByName(series.Name) != null)
            {
                return false;
            }

            var connection = _databaseConnectionManager.GetConnection();
            var sql = $"INSERT INTO {ComicSeriesTableStructure.TableName} ({ComicSeriesTableStructure.Id}, {ComicSeriesTableStructure.Name}, {ComicSeriesTableStructure.Publisher}) values ({series.Id}, '{series.Name}', '{series.Publisher}')";
            var command = new SQLiteCommand(sql, connection);

            connection.Open();
            var returnCode = command.ExecuteNonQuery();
            connection.Close();
            return returnCode == 1;
        }

        public bool DeleteSeries(ComicSeries series)
        {
            if (GetSeriesByName(series.Name) != null)
            {

                var connection = _databaseConnectionManager.GetConnection();
                var sql = $"DELETE FROM {ComicSeriesTableStructure.TableName} WHERE {ComicSeriesTableStructure.Id}={series.Id} AND {ComicSeriesTableStructure.Name}='{series.Name}' AND {ComicSeriesTableStructure.Publisher}='{series.Publisher}'";
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

        public IList<IComicSeries> GetSeries()
        {
            List<IComicSeries> series = new List<IComicSeries>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {ComicSeriesTableStructure.TableName}";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                connection.Open();
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

        public IComicSeries GetSeriesByName(string name)
        {
            List<IComicSeries> series = new List<IComicSeries>();

            var connection = _databaseConnectionManager.GetConnection();
            string sql = $"SELECT * FROM {ComicSeriesTableStructure.TableName} WHERE {ComicSeriesTableStructure.Name}={name}";

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
                if (series.Count == 1)
                {
                    return series[0];
                }
                return null;
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
    }
}
