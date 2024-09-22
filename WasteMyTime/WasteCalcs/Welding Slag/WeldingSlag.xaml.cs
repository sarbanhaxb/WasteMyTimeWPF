using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static MaterialDesignThemes.Wpf.Theme;
using ComboBox = System.Windows.Controls.ComboBox;

namespace WasteMyTime.WasteCalcs
{
    public partial class WeldingSlag : Window
    {
        ObservableCollection<RowWeldingSlag> Weldings = new ObservableCollection<RowWeldingSlag>();
        public WeldingSlag()
        {
            InitializeComponent();
            dataGrid1.ItemsSource = Weldings;
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            double comboBoxItem1 = 0.08d;
            double comboBoxItem2 = 0.09d;
            double comboBoxItem3 = 0.10d;
            double comboBoxItem4 = 0.11d;
            double comboBoxItem5 = 0.12d;

            RowWeldingSlag rowWeldingSlag = new RowWeldingSlag();
            rowWeldingSlag.ElectrodeBrand = "Электрод";
            rowWeldingSlag.Quantity = "";
            rowWeldingSlag.Normative = new ObservableCollection<double>() { comboBoxItem1, comboBoxItem2, comboBoxItem3, comboBoxItem4, comboBoxItem5};
            rowWeldingSlag.Mass = 0;
            Weldings.Add(rowWeldingSlag);
        }

        private void DelRow_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem is RowWeldingSlag rowWeldingSlag)
                Weldings.Remove(rowWeldingSlag);
        }
        private void AboutMethodClick(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox("Методика расчета шлака сварочного", "Расчет выполнен на основании п.37 таблицы 3.6.1 Методических рекомендаций по оценке объемов образования отходов производства и потребления, Москва 2003г.", this);
            aboutBox.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Weldings)
            {
                Console.WriteLine(item.Mass);
            }
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dataGrid1.SelectedItem != null && dataGrid1.SelectedItem is RowWeldingSlag item)
            {
                Console.WriteLine(item.Mass);
                string Quantity = item.Quantity.ToString();
                string Normative = item.SelectedNormative.ToString();
                double QuantityD = double.Parse(Quantity, System.Globalization.CultureInfo.InvariantCulture); ;
                double NormativeD = double.Parse(Normative, System.Globalization.CultureInfo.InvariantCulture);
                item.Mass = QuantityD * NormativeD;
            }


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is double selectedItem)
                {
                    var currentItem = dataGrid1.SelectedItem as RowWeldingSlag;

                    if (currentItem != null)
                    {
                        currentItem.SelectedNormative = selectedItem;
                    }

                    //ТУТ ЗАКОНЧИЛ
                    string Quantity = currentItem.Quantity.ToString();
                    double Normative = currentItem.SelectedNormative;
                    double QuantityD = double.Parse(Quantity, System.Globalization.CultureInfo.InvariantCulture); ;
                    currentItem.Mass = QuantityD * Normative;

                }
            }
            
        }
    }



    public class RowWeldingSlag
    {
        private string electrodeBrand;
        private string quantity;
        private double selectedNormative; // Добавьте это свойство
        private double mass;

        public string ElectrodeBrand
        {
            get => electrodeBrand;
            set
            {
                electrodeBrand = value;
                OnPropertyChanged(nameof(ElectrodeBrand));
            }
        }

        public string Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double SelectedNormative // Новое свойство для хранения выбранного значения
        {
            get => selectedNormative;
            set
            {
                selectedNormative = value;
                OnPropertyChanged(nameof(SelectedNormative));
            }
        }

        public double Mass
        {
            get => mass;
            set
            {
                mass = value;
                OnPropertyChanged(nameof(Mass));
            }
        }

        public ObservableCollection<double> Normative { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
