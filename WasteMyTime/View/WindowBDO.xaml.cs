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
using WasteMyTime.Model;
using WasteMyTime.ViewModel;
using static MaterialDesignThemes.Wpf.Theme;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

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
        }
    }

    public class WindowBDOw : WindowBDO
    {
        public int parId;
        public WindowBDOw(int Id) : base()
        {
            parId = Id;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "Добавить";
            menuItem.Click += MenuAddWasteClick;

            contextMenu.Items.Add(menuItem);
            this.BDODataGrid.ContextMenu = contextMenu;
        }

        private void MenuAddWasteClick(object sender, RoutedEventArgs e)
        {
            if (BDODataGrid.SelectedItem is BDOItem bdoItem)
            {
                SQLquery.AddWasteToCalcOption("database.db", parId, bdoItem.Number, bdoItem.Title, 0);
            }
        }
    }
}
