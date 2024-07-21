using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpVectors;

namespace WasteMyTime
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SQLquery.CreateDatabase("database.db");
            this.PritnTree();
        }

        private void PritnTree()
        {
            var cities = SQLquery.GetCitiesWithObjects("database.db");
            TreeWidget.ItemsSource = cities;
        }

        private void MainWindowExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindowsAddCityButton_Click(object sender, RoutedEventArgs e)
        {
            AddCityWindow addCityWindow = new AddCityWindow();
            addCityWindow.Owner = this;
            addCityWindow.ShowDialog();
            this.PritnTree();
        }

        private void MainWindowDeleteCityButton_Click(object sender, RoutedEventArgs e)
        {
            //НЕ РАБОТАЕТ
            var item = TreeWidget.SelectedItem;
            Console.WriteLine(item.ToString());
            Console.Write("asd");

            if (item != null)
            {
                //это не работает, если в названии города есть пробел
                //int id = SQLquery.GetCityId("database.db", item.ToString().Split()[1].Split(':')[1]);
                //SQLquery.DeleteCity("database.db", id);
                this.PritnTree();
            }
        }

    }
}
