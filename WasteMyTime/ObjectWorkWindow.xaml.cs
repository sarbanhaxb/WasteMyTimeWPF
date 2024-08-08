using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WasteMyTime
{
    /// <summary>
    /// Логика взаимодействия для ObjectWorkWindow.xaml
    /// </summary>
    public partial class ObjectWorkWindow : Window
    {
        ObjectItem CurrentObj;
        public ObjectWorkWindow(ObjectItem currentObj)
        {
            InitializeComponent();

            this.CurrentObj = currentObj;

            this.Title = $"Вариант данных: {CurrentObj.Title}";
            TextBlockInfo.Text = $"Город: {SQLquery.GetCityTitle("database.db", CurrentObj.IdCity)}";

            MainDataGrid.ItemsSource = SQLquery.LoadCalcsOption("database.db", CurrentObj.Id);
        }

        private void AddCalcButton_Click(object sender, RoutedEventArgs e)
        {
            var addnewcalc = new AddNewCalcOptionWindow(CurrentObj.Id);
            addnewcalc.ShowDialog();
            this.RefreshDG();
        }

        public void RefreshDG()
        {
            MainDataGrid.ItemsSource = SQLquery.LoadCalcsOption("database.db", CurrentObj.Id);
        }

        private void DelCalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null && MainDataGrid.SelectedItem is CalcOption calcOption)
            {
                var result = MessageBox.Show("Точно хотите удалить расчет?\nУдалятся все данные", "Удаление расчета", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) 
                {
                    SQLquery.DelCalcOption("database.db", calcOption.Id);
                    this.RefreshDG();
                }
            }
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null && MainDataGrid.SelectedItem is CalcOption calcOption)
            {
                SelfCalcWindow selfCalcWindow = new SelfCalcWindow(calcOption.Id);
                selfCalcWindow.ShowDialog();
            }
        }

        private void EditCalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null && MainDataGrid.SelectedItem is CalcOption calcOption)
            {
                SelfCalcWindow selfCalcWindow = new SelfCalcWindow(calcOption.Id);
                selfCalcWindow.ShowDialog();
            }
        }
    }
}
