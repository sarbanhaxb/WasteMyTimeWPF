using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Data.Entity;
using System.Windows.Documents;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

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

                command.CommandText = "CREATE TABLE IF NOT EXISTS CalcsOption" +
                                    "(id INTEGER PRIMARY KEY, " +
                                    "object_id INTEGER, " +
                                    "title VARCHAR(100) NOT NULL, " +
                                    "FOREIGN KEY (object_id) REFERENCES objects(id) ON DELETE CASCADE)";
                command.ExecuteNonQuery();

                command.CommandText = "CREATE TABLE IF NOT EXISTS WasteItems" +
                    "(id INTEGER PRIMARY KEY, " +
                    "calcOption_id INTEGER, " +
                    "FKKOcode VARCHAR(100) NOT NULL, " +
                    "Title VARCHAR(100) NOT NULL, " +
                    "Normative VARCHAR(10) DEFAULT '0', " +
                    "FOREIGN KEY (calcOption_id) REFERENCES CalcsOption(id) ON DELETE CASCADE)";
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateNormative(string Dataname, int id, string n)
        {
            string sqlExpression = $"UPDATE WasteItems SET Normative='{n}' WHERE id={id}";
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

        public static ObservableCollection<CalcOption> LoadCalcsOption(string Dataname, int id_object)
        {
            var itemSource = new ObservableCollection<CalcOption>();
            string sqlExpression = $"SELECT * FROM CalcsOption WHERE object_id={id_object}";

            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CalcOption item = new CalcOption();
                        item.Id = Convert.ToInt32(reader["id"]);
                        item.ObjectID = Convert.ToInt32(reader["object_id"]);
                        item.Title = Convert.ToString(reader["title"]);
                        itemSource.Add(item);
                    }
                }

            }
            return itemSource;
        }

        public static BDOItem getBDOItem(string Dataname, string FKKOcode)
        {
            var item = new BDOItem();
            string sqlExpression = $"SELECT * FROM bdo WHERE num='{FKKOcode}'";

            using (var conn = new SQLiteConnection($"Data Source={Dataname}"))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, conn);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item.Number = Convert.ToString(reader["num"]);
                        item.Title = Convert.ToString(reader["title"]);
                        item.OriginManufacturing = Convert.ToString(reader["originManufacturing"]);
                        item.OriginProducts = Convert.ToString(reader["originProducts"]);
                        item.OriginProcess = Convert.ToString(reader["originProcess"]);
                        item.Compound = Convert.ToString(reader["compound"]);
                        item.CompoundPercentMin = Convert.ToDouble(reader["compoundPercentMin"]);
                        item.CompoundPercentMax = Convert.ToDouble(reader["compoundPercentMax"]);
                        item.CompoundNotice = Convert.ToString(reader["compoundNotice"]);
                        item.WasteNotice = Convert.ToString(reader["wasteNotice"]);
                        item.PhysicalState = Convert.ToString(reader["physicalState"]);
                        item.HazardClass = Convert.ToString(reader["hazardClass"]);
                        item.AttributionCriteria = Convert.ToString(reader["attributionCriteria"]);
                        item.Doc = Convert.ToString(reader["docs"]);
                    }
                }
            }
            return item;
        }

        public static ObservableCollection<Waste> LoadWaste(string Dataname, int CalcsOption)
        {
            var itemSource = new ObservableCollection<Waste>();
            string sqlExpression = $"SELECT * FROM WasteItems WHERE calcOption_id={CalcsOption}";

            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand( sqlExpression, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Waste item = new Waste();
                        item.Id = Convert.ToInt32(reader["id"]);
                        item.calcOption_id = Convert.ToInt32(reader["calcOption_id"]);
                        item.FKKOcode = (string)reader["FKKOcode"];
                        item.Title = (string)reader["Title"];
                        string x = (string)reader["Normative"];
                        item.Normative = Convert.ToDouble(x.Replace('.', ','));
                        itemSource.Add(item);
                    }
                }
            }
            return itemSource;
        }

        public static void AddWasteToCalcOption(string Dataname, int CalcOption, string FKKOcode, string Title, double Normative)
        {
            string sqlExpression = $"INSERT INTO WasteItems (calcOption_id, FKKOcode, Title, Normative) VALUES ({CalcOption}, '{FKKOcode}', '{Title}', {Normative})";
            using (var connections = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connections.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connections);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        public static void AddCalcOption(string Dataname, int object_id, string title)
        {
            string sqlExpression = $"INSERT INTO CalcsOption (object_id, title) VALUES ({object_id}, '{title}')";
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

        public static void DeleteWaste(string Dataname, int wasteId)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={Dataname}; foreign_keys = ON;"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("DELETE FROM WasteItems WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", wasteId);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
        }

        public static void DelCalcOption(string Dataname, int id)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={Dataname}; foreign_keys = ON;"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("DELETE FROM CalcsOption WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка при выполнении операции.", $"{ex.Message}", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
        }


        public static ObservableCollection<BDOItem> LoadBDO()
        {
            var ItemsSource = new ObservableCollection<BDOItem>();
            string sqlExpression = $"SELECT * FROM bdo";

            using (var connection = new SQLiteConnection($"Data Source=BDO.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BDOItem item = new BDOItem();
                        item.Number = Convert.ToString(reader["num"]);
                        item.Title = Convert.ToString(reader["title"]);
                        item.OriginManufacturing = Convert.ToString(reader["originManufacturing"]);
                        item.OriginProducts = Convert.ToString(reader["originProducts"]);
                        item.OriginProcess = Convert.ToString(reader["originProcess"]);
                        item.Compound = Convert.ToString(reader["compound"]);
                        item.CompoundPercentMin = Convert.ToDouble(reader["compoundPercentMin"]);
                        item.CompoundPercentMax = Convert.ToDouble(reader["compoundPercentMax"]);
                        item.CompoundNotice = Convert.ToString(reader["compoundNotice"]);
                        item.WasteNotice = Convert.ToString(reader["wasteNotice"]);
                        item.PhysicalState = Convert.ToString(reader["physicalState"]);
                        item.HazardClass = Convert.ToString(reader["hazardClass"]);
                        item.AttributionCriteria = Convert.ToString(reader["attributionCriteria"]);
                        item.Doc = Convert.ToString(reader["docs"]);

                        ItemsSource.Add(item);
                    }
                }
            }
            return ItemsSource;
        }

        public static bool CheckExists(string Dataname, string tableName)
        {
            string sqlExpression = $"SELECT EXISTS(SELECT 1 FROM sqlite_master WHERE type='table' AND name='{tableName}')";
            using (var connection = new SQLiteConnection($"Data Source={Dataname}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                return Convert.ToBoolean(command.ExecuteScalar());
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
                catch (SQLiteException ex)
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

        public static void UpdateCalcOptionTitle(string Dataname, int id, string title)
        {
            string sqlExpression = $"UPDATE CalcsOption SET title='{title}' WHERE id={id}";
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

        public static string GetNormative(string title)
        {
            string sqlExpression = $"SELECT Normative FROM CalcProperty WHERE Title='{title}'";
            using (var connection = new SQLiteConnection("Data Source=CalcProperty.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand( sqlExpression, connection);
                return Convert.ToString(command.ExecuteScalar());
            }
        }

        public static void SetNormative(string Normative, string Title)
        {
            string sqlExpression = $"UPDATE CalcProperty SET Normative='{Normative}' WHERE Title='{Title}'";
            using (var connection = new SQLiteConnection($"Data Source=CalcProperty.db"))
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
