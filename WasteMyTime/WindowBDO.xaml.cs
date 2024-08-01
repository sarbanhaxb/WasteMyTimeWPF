﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace WasteMyTime
{
    /// <summary>
    /// Логика взаимодействия для WindowBDO.xaml
    /// </summary>
    public partial class WindowBDO : Window
    {
        public WindowBDO()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            //BDOItems itemSourceList = SQLquery.LoadBDO();
            //ICollectionView cvTasks = CollectionViewSource.GetDefaultView(TableInfo.ItemsSource);
            //TableInfo.ItemsSource = itemSourceList;

        }
    }
}
