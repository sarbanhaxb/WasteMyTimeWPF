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
    public partial class AddObjectWindow : Window
    {
        private int _idCity;
        public AddObjectWindow(int idCity)
        {
            InitializeComponent();
            _idCity = idCity;
            ObjTitleTextBox.Focus();
        }

        private void AddObjButton_Click(object sender, RoutedEventArgs e)
        {
            SQLquery.AddObject("database.db", _idCity, ObjTitleTextBox.Text);
            this.Close();
        }

        private void CancelObjButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowObj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SQLquery.AddObject("database.db", _idCity, ObjTitleTextBox.Text);
                this.Close();
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
