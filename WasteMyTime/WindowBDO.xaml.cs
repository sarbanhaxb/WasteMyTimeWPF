using System;
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

            //var _itemSourceList = new CollectionViewSource() { Source = SQLquery.LoadBDO() };
            //_itemSourceList.Filter += new FilterEventHandler(YourFilter);
            //ICollectionView ItemList = _itemSourceList.View;

            BDOItems itemSourceList = SQLquery.LoadBDO();
            ICollectionView cvTasks = CollectionViewSource.GetDefaultView(TableInfo.ItemsSource);
            TableInfo.ItemsSource = itemSourceList;
        }

        //private void YourFilter(object sender, FilterEventArgs e)
        //{
        //    var obj = e.Item as BDOItem;
        //    if (obj != null)
        //    {
        //        if (obj.Number.Contains(TEXTFILTER.Text))
        //        {
        //            e.Accepted = true;
        //        }
        //        else
        //        {
        //            e.Accepted = false;
        //        }
        //    }
        //}

        //private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        //{
        //    BDOItem t = e.Item as BDOItem;
        //    if (t != null) 
        //    {
        //        if (this.cbComleteFilter.IsChecked == true && t.Complete == true) 
        //        {
        //            e.Accepted = false;
        //        }
        //        else
        //        {
        //            e.Accepted = true;
        //        }
        //    }
        //}

        //private void TEXTFILTER_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
    }
}
