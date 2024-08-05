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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddNewCalcOptionWindow : Window
    {
        int CurrentId;
        public AddNewCalcOptionWindow(int currentID)
        {
            InitializeComponent();
            CalcTitleTextBox.Focus();
            this.CurrentId = currentID;
        }

        private void AddCalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (CalcTitleTextBox.Text != "")
            {
                SQLquery.AddCalcOption("database.db", CurrentId, CalcTitleTextBox.Text);
                this.Close();
            }
        }

        private void CancelCalcButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowObj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                if (CalcTitleTextBox.Text != "")
                {
                    SQLquery.AddCalcOption("database.db", CurrentId, CalcTitleTextBox.Text);
                    this.Close();
                }
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
