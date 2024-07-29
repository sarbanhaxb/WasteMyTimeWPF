﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;

namespace WasteMyTime
{
    public static class SQLquery
    {
        public static void ForeignKeysOn(string Dataname)
        {
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "PRAGMA foreign_keys = ON;";
                command.ExecuteNonQuery();
            }
        }
        public static void CreateDatabase(string Dataname)
        {
            using (var connection = new SQLiteConnection($"Data Source={Dataname}")) 
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;

                command.CommandText = "CREATE TABLE IF NOT EXISTS cities " +
                    "(id INTEGER PRIMARY KEY, " +
                    "title VARCHAR(100) NOT NULL UNIQUE)";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS objects" +
                    "(id INTEGER PRIMARY KEY, " +
                    "id_city INTEGER, " +
                    "title VARCHAR(100) NOT NULL UNIQUE, " +
                    "FOREIGN KEY (id_city) REFERENCES cities(id) ON DELETE CASCADE)";
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
                        city.Id = Convert.ToInt32(reader["id"]);
                        city.Title = Convert.ToString(reader["title"]);
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
                        ObjectItem obj = new ObjectItem();
                        obj.Id = Convert.ToInt32(rdr["id"]);
                        obj.IdCity = Convert.ToInt32(rdr["id_city"]);
                        obj.Title = Convert.ToString(rdr["title"]);

                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public static List<Object> GetObjects(string Dataname, int id_city)
        {
            List<Object> list = new List<Object>();
            string sqlExpression = $"SELECT * FROM objects WHERE id_city={id_city}";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader rdr = command.ExecuteReader())
                    while (rdr.Read())
                    {
                        ObjectItem obj = new ObjectItem();
                        obj.Id = Convert.ToInt32(rdr["id"]);
                        obj.IdCity = Convert.ToInt32(rdr["id_city"]);
                        obj.Title = Convert.ToString(rdr["title"]);
                        list.Add(obj);
                    }
                return list;
            }
        }


        public static void AddCity(string Dataname, string title)
        {
            string sqlExpression = $"INSERT INTO cities (title) VALUES ('{title}')";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}")) 
            {
                connection.Open();
                try
                {
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex) 
                {
                    MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public static void AddObject(string Dataname, int id_city, string title)
        {
            string sqlExpression = $"INSERT INTO objects (id_city, title) VALUES ({id_city}, '{title}')";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                catch ( SQLiteException ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public static void DeleteCity(string Dataname, int cityId)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={Dataname}; foreign_keys = ON;"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("DELETE FROM cities WHERE id = @cityId", connection);
                    command.Parameters.AddWithValue("@cityId", cityId);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при удалении города: {ex.Message}");
                MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
        }

        public static void DeleteObject(string Dataname, int objectId)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("DELETE FROM objects WHERE id = @objectId", connection);
                    command.Parameters.AddWithValue("@objectId", objectId);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при удалении объекта: {ex.Message}");
                MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
        }

        public static int GetCityId(string Dataname, string title)
        {
            int cityId;

            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT id FROM cities WHERE title=@title");
                command.Parameters.AddWithValue("@title", title);
                cityId = command.ExecuteReader().GetInt32(0);
            }

            return cityId;
        }

        public static string GetCityTitle(string Dataname, int id_city)
        {
            string title = "";
            string sqlExpression = $"SELECT title FROM cities WHERE id={id_city}";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                return Convert.ToString(command.ExecuteScalar());
            }
        }


        public static ObservableCollection<City> GetCitiesWithObjects(string Dataname)
        {
            var cities = new ObservableCollection<City>();

            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();

                var cityCommand = new SQLiteCommand("SELECT id, title FROM cities", connection);

                using (var reader = cityCommand.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        var city = new City
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1)
                        };

                        var objectCommand = new SQLiteCommand("SELECT id, id_city, title FROM objects WHERE id_city = @id_city", connection);
                        objectCommand.Parameters.AddWithValue("@id_city", city.Id);

                        using (var objectReader = objectCommand.ExecuteReader())
                        {
                            while (objectReader.Read())
                            {
                                ObjectItem obj = new ObjectItem
                                {
                                    Id = objectReader.GetInt32(0),
                                    IdCity = objectReader.GetInt32(1),
                                    Title = objectReader.GetString(2)
                                };
                                city.Objects.Add(obj);
                            }
                        }

                        cities.Add(city);
                    }
                }
            }
            return cities;
        }

        public static void UpdateCityData(string Dataname, int id, string title) 
        {
            string sqlExpression = $"UPDATE cities SET title='{title}' WHERE id={id}";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    MessageBox.Show($"Ошибка изменения наименования: {ex.Message}", "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public static void UpdateObjectData(string Dataname, int id, string title)
        {
            string sqlExpression = $"UPDATE objects SET title='{title}' WHERE id={id}";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    MessageBox.Show($"Ошибка изменения наименования: {ex.Message}", "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
