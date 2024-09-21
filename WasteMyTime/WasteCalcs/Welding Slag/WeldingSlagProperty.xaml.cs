using DocumentFormat.OpenXml.Office.MetaAttributes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace WasteMyTime.WasteCalcs.Welding_Slag
{
    /// <summary>
    /// Логика взаимодействия для WeldingSlagProperty.xaml
    /// </summary>
    public partial class WeldingSlagProperty : Window
    {
        public WeldingSlagProperty(Window owner)
        {
            this.Owner = owner;

            InitializeComponent();
            NormativeCB.Text = Convert.ToString(SQLquery.GetNormative("WeldingSlag"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)NormativeCB.SelectedItem;
            SQLquery.SetNormative(typeItem.Content.ToString(), "WeldingSlag");
            this.Close();
        }
    }
}
