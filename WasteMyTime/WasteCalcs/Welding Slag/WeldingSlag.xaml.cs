using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;


namespace WasteMyTime.WasteCalcs
{
    public class Welding : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int WasteItemId { get; set; }
        private string _electrodeBrand;
        private string _electrodeMass;
        private double _normative;
        private double _slagMass;

        public string ElectrodeBrand
        {
            get => _electrodeBrand;
            set
            {
                if (_electrodeBrand != value)
                {
                    _electrodeBrand = value;
                    OnPropertyChanged(nameof(ElectrodeBrand));
                }
            }
        }

        public string ElectrodeMass
        {
            get => _electrodeMass;
            set
            {
                if (_electrodeMass != value)
                {
                    _electrodeMass = value;
                    OnPropertyChanged(nameof(ElectrodeMass));
                }
            }
        }

        public double Normative
        {
            get => _normative;
            set
            {
                if (_normative != value)
                {
                    _normative = value;
                    OnPropertyChanged(nameof(Normative));
                }
            }
        }

        public double SlagMass
        {
            get => _slagMass;
            set
            {
                if (_slagMass != value)
                {
                    _slagMass = value;
                    OnPropertyChanged(nameof(SlagMass));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class WeldingSlag : Window, INotifyPropertyChanged
    {
        public int WasteItemID;
        public ObservableCollection<Welding> Weldings { get; set; }
        private string connectionString = "Data Source=database.db;";

        public WeldingSlag(int WasteItemId)
        {
            this.WasteItemID = WasteItemId;
            InitializeComponent();
            Weldings = new ObservableCollection<Welding>();
            LoadData();
            DataContext = this;

            WeldingDataGrid.RowEditEnding += WeldingDataGrid_RowEditEnding;
        }

        private void WeldingDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var updatedWelding = e.Row.DataContext as Welding;
                UpdateDatabase(updatedWelding);
            }
        }

        private void UpdateDatabase(Welding updatedWelding)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            if (updatedWelding.ElectrodeMass.Contains(","))
            {
                updatedWelding.ElectrodeMass = updatedWelding.ElectrodeMass.Replace(",", ".");
            }
            updatedWelding.SlagMass = Math.Round(Convert.ToDouble(updatedWelding.ElectrodeMass) * updatedWelding.Normative, 3);

            string sqlExpression = $"UPDATE WeldingData SET ElectrodeBrand='{updatedWelding.ElectrodeBrand}', ElectrodeMass={updatedWelding.ElectrodeMass}, Normative={updatedWelding.Normative}, SlagMass={updatedWelding.SlagMass} WHERE id={updatedWelding.ID}";
            using (var connection = new SQLiteConnection($"Data Source=database.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }

        private void LoadData()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM WeldingData WHERE WasteItemId={WasteItemID}";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Weldings.Add(new Welding
                    {
                        ID = Convert.ToInt32((reader["id"])),
                        WasteItemId = Convert.ToInt32((reader["WasteItemId"])),
                        ElectrodeBrand = reader["ElectrodeBrand"].ToString(),
                        ElectrodeMass = reader["ElectrodeMass"].ToString(),
                        Normative = double.Parse(reader["Normative"].ToString()),
                        SlagMass = double.Parse(reader["SlagMass"].ToString())
                    });
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newWelding = new Welding
            {
                WasteItemId = WasteItemID,
                ElectrodeBrand = "Новая марка",
                ElectrodeMass = "0",
                Normative = 0.10d,
                SlagMass = 0
            };
            SaveToDatabase(newWelding);

            newWelding.ID = GetLastID();
            Weldings.Add(newWelding);
        }

        private int GetLastID()
        {
            int id;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id FROM WeldingData ORDER BY id DESC LIMIT 1;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            return id;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WeldingDataGrid.SelectedItem is Welding selectedWelding)
            {
                Weldings.Remove(selectedWelding);
                RemoveFromDatabase(selectedWelding);
            }
        }

        private void SaveToDatabase(Welding welding)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO WeldingData (WasteItemId, ElectrodeBrand, ElectrodeMass, Normative, SlagMass) VALUES (@WasteItemId, @brand, @mass, @normative, @slag)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@WasteItemId", welding.WasteItemId);
                command.Parameters.AddWithValue("@brand", welding.ElectrodeBrand);
                command.Parameters.AddWithValue("@mass", welding.ElectrodeMass);
                command.Parameters.AddWithValue("@normative", welding.Normative);
                command.Parameters.AddWithValue("@slag", welding.SlagMass);
                command.ExecuteNonQuery();
            }
        }

        private void RemoveFromDatabase(Welding welding)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM WeldingData WHERE id = @ID";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@ID", welding.ID);
                command.ExecuteNonQuery();
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox("Методика расчета шлака сварочного", "Расчет выполнен на основании п.37 таблицы 3.6.1 Методических рекомендаций по оценке объемов образования отходов производства и потребления, Москва 2003г.", this);
            aboutBox.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PrintCalc_Click(object sender, RoutedEventArgs e)
        {
            List<string> Header = SQLquery.GetWasteForPrint(WasteItemID);
            double sum = 0;
            foreach (var item in Weldings) 
            {
                sum += item.SlagMass;
            }

            Table table = new Table();

            TableProperties tableProperties = new TableProperties(new TableBorders(new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                                                                 new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                                                                 new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                                                                 new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                                                                 new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                                                                                 new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }));
            table.AppendChild(tableProperties);

            TableCell Cell1 = new TableCell(new Paragraph(new Run(new Text("Марка электрода"))));
            TableCell Cell2 = new TableCell(new Paragraph(new Run(new Text("Масса электродов, т"))));
            TableCell Cell3 = new TableCell(new Paragraph(new Run(new Text("Норматив образования"))));
            TableCell Cell4 = new TableCell(new Paragraph(new Run(new Text("Масса отхода, т"))));
            table.Append(new TableRow(Cell1, Cell2, Cell3, Cell4));



            foreach (var welding in Weldings)
            {
                TableRow row = new TableRow();
                TableCell ElectrodeBrandCell = new TableCell(new Paragraph(new Run(new Text(welding.ElectrodeBrand))));
                TableCell ElectrodeMassCell = new TableCell(new Paragraph(new Run(new Text(welding.ElectrodeMass))));
                TableCell NormativeCell = new TableCell(new Paragraph(new Run(new Text(welding.Normative.ToString()))));
                TableCell SlagMassCell = new TableCell(new Paragraph(new Run(new Text(welding.SlagMass.ToString()))));
                row.Append(ElectrodeBrandCell);
                row.Append(ElectrodeMassCell);
                row.Append(NormativeCell);
                row.Append(SlagMassCell);
                table.Append(row);
            }


            List<dynamic> listD = new List<dynamic>();
            listD.Add(new Paragraph(new Run(new Text("Расчет выполнен на основании п.37 таблицы 3.6.1 Методических рекомендаций по оценке объемов образования отходов производства и потребления, Москва 2003г."))));
            listD.Add(table);
            listD.Add(new Paragraph(new Run(new Text($"Норматив образования отхода: {sum.ToString()} тонн"))));

            Printout report = new Printout(Header[0] + " " + Header[1], listD);
            report.SaveReport();
        }
    }
}