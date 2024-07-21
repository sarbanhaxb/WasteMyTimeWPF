using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace WasteMyTime
{
    public static class SQLquery
    {
        public static void CreateDatabase(string Dataname)
        {
            using (var connection = new SQLiteConnection($"Data Source={Dataname}")) 
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS cities (id INTEGER PRIMARY KEY, title VARCHAR(100) NOT NULL UNIQUE)";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS objects(id INTEGER PRIMARY KEY, id_city INTEGER, title VARCHAR(100) NOT NULL UNIQUE, FOREIGN KEY (id_city) REFERENCES cities(id) ON DELETE CASCADE)";
                command.ExecuteNonQuery();
            }
        }

        public static List<City> GetCities(string Dataname)
        {
            List<City> list = new List<City>();
            string sqlExpression = "SELECT * FROM cities";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        City city = new City();
                        city.Id = Convert.ToInt32(reader["ID"]);
                        city.Title = Convert.ToString(reader["Title"]);
                        list.Add(city);
                    }
                }
            }
            return list;
        }

        public static List<Object> GetObjects(string Dataname) 
        { 
            List<Object> list = new List<Object>();
            string sqlExpression = "SELECT * FROM objects";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Object obj = new Object();
                        obj.Id = Convert.ToInt32(rdr["ID"]);
                        obj.Id_city = Convert.ToInt32(rdr["ID_CITY"]);
                        obj.Title = Convert.ToString(rdr["Title"]);

                        list.Add(obj);
                    }
                }
            }
            return list;
        }
    }
}
