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

            MainDataGrid.ItemsSource = SQLquery.LoadCalcsOption("database.db", CurrentObj.IdCity);

        }

        private void AddCalcButton_Click(object sender, RoutedEventArgs e)
        {
            var addnewcalc = new AddNewCalcOptionWindow(CurrentObj.IdCity);
            addnewcalc.ShowDialog();
            this.RefreshDG();
        }

        public void RefreshDG()
        {
            MainDataGrid.ItemsSource = SQLquery.LoadCalcsOption("database.db", CurrentObj.IdCity);
        }

        private void DelCalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null && MainDataGrid.SelectedItem is CalcOption calcOption)
            {
                SQLquery.DelCalcOption("database.db", calcOption.Id);
                this.RefreshDG();
            }
        }
    }
}
