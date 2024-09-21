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
    /// Логика взаимодействия для AboutBox.xaml
    /// </summary>
    public partial class AboutBox : Window
    {
        public AboutBox(string Title, string Message, Window owner)
        {
            InitializeComponent();
            this.Title = Title;
            MainTextBox.Text = Title;
            this.Owner = owner;
            this.NotMainText.Text = Message;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
