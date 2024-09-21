using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WasteMyTime.WasteCalcs.Welding_Slag;
using static MaterialDesignThemes.Wpf.Theme;

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
            ComboBoxItem comboBoxItem1 = new ComboBoxItem();
            comboBoxItem1.Content = SQLquery.GetNormative("WeldingSlag");
            ComboBoxItem comboBoxItem2 = new ComboBoxItem();
            ComboBoxItem comboBoxItem3 = new ComboBoxItem();
            ComboBoxItem comboBoxItem4 = new ComboBoxItem();
            ComboBoxItem comboBoxItem5 = new ComboBoxItem();

            switch (comboBoxItem1.Content)
            {
                case "0.08":
                    comboBoxItem2.Content = "0.09";
                    comboBoxItem3.Content = "0.10";
                    comboBoxItem4.Content = "0.11";
                    comboBoxItem5.Content = "0.12";
                    break;
                case "0.09":
                    comboBoxItem2.Content = "0.08";
                    comboBoxItem3.Content = "0.10";
                    comboBoxItem4.Content = "0.11";
                    comboBoxItem5.Content = "0.12";
                    break;
                case "0.10":
                    comboBoxItem2.Content = "0.08";
                    comboBoxItem3.Content = "0.09";
                    comboBoxItem4.Content = "0.11";
                    comboBoxItem5.Content = "0.12";
                    break;
                case "0.11":
                    comboBoxItem2.Content = "0.08";
                    comboBoxItem3.Content = "0.09";
                    comboBoxItem4.Content = "0.10";
                    comboBoxItem5.Content = "0.12";
                    break;
                case "0.12":
                    comboBoxItem2.Content = "0.08";
                    comboBoxItem3.Content = "0.09";
                    comboBoxItem4.Content = "0.10";
                    comboBoxItem5.Content = "0.11";
                    break;
            }

            Weldings.Add(new RowWeldingSlag() {ElectrodeBrand="Электрод", Quantity=0, Normative=new List<ComboBoxItem>() {comboBoxItem1, comboBoxItem2, comboBoxItem3, comboBoxItem4, comboBoxItem5}, Mass=0 });
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WeldingSlagProperty weldingSlagProperty = new WeldingSlagProperty(this);
            weldingSlagProperty.ShowDialog();
        }
    }



    public class RowWeldingSlag
    {
        public string ElectrodeBrand { get; set; }
        public double Quantity { get; set; }
        public List<ComboBoxItem> Normative { get; set; }
        public double Mass { get; set; }
    }
}
