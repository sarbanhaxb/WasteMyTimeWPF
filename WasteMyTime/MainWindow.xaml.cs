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
                SQLquery.AddObject("database.db", id_city, "Новый объект");
            }
            this.PritnTree();
        }

        private void MenuItemDeleteItemClick(object sender, RoutedEventArgs e)
        {
            // Логика для действия 2
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
    }
}
