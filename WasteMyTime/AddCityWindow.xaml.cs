﻿using System;
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
    /// Логика взаимодействия для AddCityWindow.xaml
    /// </summary>
    public partial class AddCityWindow : Window
    {
        public AddCityWindow()
        {
            InitializeComponent();
            CityTitleTextBox.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddCityButton_Click(object sender, RoutedEventArgs e)
        {
            if (CityTitleTextBox.Text != "")
            {
                SQLquery.AddCity("database.db", CityTitleTextBox.Text);
                this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Console.WriteLine(CityTitleTextBox.Text);
                SQLquery.AddCity("database.db", CityTitleTextBox.Text);
                this.Close();
            }
            if (e.Key == Key.Escape) 
            {
                this.Close();
            }
        }
    }
}
