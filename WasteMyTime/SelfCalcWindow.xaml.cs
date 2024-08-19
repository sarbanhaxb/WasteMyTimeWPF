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
    /// Логика взаимодействия для SelfCalcWindow.xaml
    /// </summary>
    public partial class SelfCalcWindow : Window
    {
        public int CalcOptionId;
        public SelfCalcWindow(int calcOptionId, string title)
        {
            InitializeComponent();
            this.Title = $"Вариант расчета: {title}";
            var WasteItems = SQLquery.LoadWaste("database.db", calcOptionId);

            WasteDataGrid.ItemsSource = WasteItems;

            foreach (var WasteItem in WasteItems)
            {
                Console.WriteLine(WasteItem.Title);
            }

            CalcOptionId = calcOptionId;
        }

        private void ShowBDO_Click(object sender, RoutedEventArgs e)
        {
            WindowBDOw windowBDO = new WindowBDOw(CalcOptionId);
            windowBDO.ShowDialog();

            WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);

        }
    }
}
