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
            List<City> cities = SQLquery.GetCities("database.db");
            List<Object> objects = SQLquery.GetObjects("database.db");
            foreach (var city in cities)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = city.Title;
                foreach (var obj in objects) 
                {
                    if (obj.Id_city == city.Id)
                    {
                        TreeViewItem sub_item = new TreeViewItem();
                        sub_item.Header = obj.Title;
                        item.Items.Add(sub_item);
                    }
                }
                TreeWidget.Items.Add(item);
            }

        }

        private void MainWindowExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindowsAddCityButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
