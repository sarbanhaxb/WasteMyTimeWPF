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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

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
            SQLquery.ForeignKeysOn("database.db");
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
            if (TreeWidget.SelectedItem != null)
            {
                if (TreeWidget.SelectedItem is City)
                {
                    var item = (City)TreeWidget.SelectedItem;
                    SQLquery.DeleteCity("database.db", item.Id);
                }
                else if (TreeWidget.SelectedItem is ObjectItem)
                {
                    var item = (ObjectItem)TreeWidget.SelectedItem;
                    SQLquery.DeleteObject("database.db", item.Id);
                }
            }
            this.PritnTree();
        }

        private void TreeWidget_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем исходный элемент, на который нажали правой кнопкой мыши
            var clickedItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

            if (clickedItem != null)
            {
                // Устанавливаем выделение для выбранного элемента
                clickedItem.IsSelected = true;

                // Создаем контекстное меню
                ContextMenu contextMenu = new ContextMenu();

                // Добавляем элементы в контекстное меню
                MenuItem action1 = new MenuItem { Header = "Добавить объект" };
                action1.Click += MenuItemAddObjectClick;

                MenuItem action2 = new MenuItem { Header = "Удалить объект" };
                action2.Click += MenuItemDeleteItemClick;

                contextMenu.Items.Add(action1);
                contextMenu.Items.Add(action2);

                // Устанавливаем контекстное меню и открываем его
                contextMenu.PlacementTarget = clickedItem;
                contextMenu.IsOpen = true;

                e.Handled = true; // Отменяем стандартное поведение
            }
        }

        private void MenuItemAddObjectClick(object sender, RoutedEventArgs e)
        {
            if (TreeWidget.SelectedItem is City)
            {
                int id_city = ((City)TreeWidget.SelectedItem).Id;
                var addObjectWindow = new AddObjectWindow(id_city);
                addObjectWindow.ShowDialog();
                this.PritnTree();
            }
        }

        private void MenuItemDeleteItemClick(object sender, RoutedEventArgs e)
        {
            if (TreeWidget.SelectedItem is ObjectItem item)
            {
                SQLquery.DeleteObject("database.db", item.Id);
                this.PritnTree();
            }
            else if (TreeWidget.SelectedItem is City city)
            {
                SQLquery.DeleteCity("database.db", city.Id);
                this.PritnTree();
            }
        }

        // Вспомогательный метод для поиска предков
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void TESTBUTTON_Click(object sender, RoutedEventArgs e)
        {
            var cities = SQLquery.GetCities("database.db");
            foreach (var c in cities)
            {
                Console.WriteLine($"ГОРОД: {c.Title}, {c.Id}");

                foreach(ObjectItem c2 in SQLquery.GetObjects("database.db", c.Id))
                {
                    Console.WriteLine($"ОБЪЕКТ ГОРОДА: {c2.IdCity} {c2.Title}");
                }
            }
        }

        private void TreeWidget_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeWidget.SelectedItem is City city)
            {
                TextBoxCityName.Text = city.Title;
                TextBoxObjectTitle.Text = "";
            }
            else if (TreeWidget.SelectedItem is ObjectItem item)
            {
                TextBoxCityName.Text= SQLquery.GetCityTitle("database.db", item.IdCity);
                TextBoxObjectTitle.Text = item.Title;
            }
        }
    }
}
