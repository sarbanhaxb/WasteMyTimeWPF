using Aspose.Cells;
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

            CalcOptionId = calcOptionId;
        }

        private void ShowBDO_Click(object sender, RoutedEventArgs e)
        {
            WindowBDOw windowBDO = new WindowBDOw(CalcOptionId);
            windowBDO.ShowDialog();

            WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);

        }

        private void DeleteWaste_Click(object sender, RoutedEventArgs e)
        {
            if (WasteDataGrid.SelectedItem is Waste waste)
            {
                SQLquery.DeleteWaste("database.db", waste.Id);
                WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
            }
        }

        public void WasteDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (WasteDataGrid.SelectedItem != null && WasteDataGrid.SelectedItem is Waste waste)
            {
                //обтирочный материал менее 15%
                if (waste.FKKOcode == "9 19 204 02 60 4")
                {
                    CalcPanel.Children.Clear();

                    //персонал
                    TextBox CountOfWorkers = new TextBox();
                    Label CountOfWorkersLabel = new Label();
                    CountOfWorkersLabel.Content = "Количество персонала, чел.";
                    CountOfWorkersLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    CountOfWorkers.HorizontalAlignment = HorizontalAlignment.Left;
                    CountOfWorkers.Width = 50;
                    CountOfWorkers.Text = "0";
                    //норматив
                    TextBox Normative = new TextBox();
                    Normative.Width = 50;
                    Normative.HorizontalAlignment = HorizontalAlignment.Left;
                    Normative.Text = "0,1";
                    Label NormativeLabel = new Label();
                    NormativeLabel.Content = "Норматив, кг/смена";
                    NormativeLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    //продолжительность
                    TextBox Duration = new TextBox();
                    Duration.Width = 50;
                    Duration.HorizontalAlignment = HorizontalAlignment.Left;
                    Duration.Text = "0";
                    Label DurationLabel = new Label();
                    DurationLabel.Content = "Продолжительность, дни";
                    DurationLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    //остаток нефтепродукта
                    TextBox Remainder = new TextBox();
                    Remainder.Width = 50;
                    Remainder.HorizontalAlignment = HorizontalAlignment.Left;
                    Remainder.Text = "14,999";
                    Label RemainderLabel = new Label();
                    RemainderLabel.Content = "Остаток нефтепродукта, %";
                    RemainderLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    Button Calc = new Button();
                    Calc.Width = 75;
                    Calc.Height = 20;
                    Calc.Content = "Рассчитать";
                    Calc.HorizontalAlignment = HorizontalAlignment.Left;

                    CalcPanel.Children.Add(CountOfWorkersLabel);
                    CalcPanel.Children.Add(CountOfWorkers);
                    CalcPanel.Children.Add(NormativeLabel);
                    CalcPanel.Children.Add(Normative);
                    CalcPanel.Children.Add(DurationLabel);
                    CalcPanel.Children.Add(Duration);
                    CalcPanel.Children.Add(RemainderLabel);
                    CalcPanel.Children.Add(Remainder);
                    CalcPanel.Children.Add(new Label());
                    CalcPanel.Children.Add(Calc);


                    Calc.Click += CalcWaste_Click;

                    void CalcWaste_Click(object sender1, RoutedEventArgs e1)
                    {
                        int CountOfWorkersD = Convert.ToInt32(CountOfWorkers.Text);
                        double NormativeD = Convert.ToDouble(Normative.Text);
                        int DurationD = Convert.ToInt32(Duration.Text);
                        double ReminderD = Convert.ToDouble(Remainder.Text);

                        double nN = Math.Round(CountOfWorkersD * NormativeD * DurationD / 1000 * (ReminderD/100+1), 3);
                        string nNs = Convert.ToString(nN);
                        SQLquery.UpdateNormative("database.db", waste.Id, nNs);
                        WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
                    }
                }

                else if (waste.FKKOcode == "9 19 204 01 60 3")
                {
                    CalcPanel.Children.Clear();

                    //персонал
                    TextBox CountOfWorkers = new TextBox();
                    Label CountOfWorkersLabel = new Label();
                    CountOfWorkersLabel.Content = "Количество персонала, чел.";
                    CountOfWorkersLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    CountOfWorkers.HorizontalAlignment = HorizontalAlignment.Left;
                    CountOfWorkers.Width = 50;
                    CountOfWorkers.Text = "0";
                    //норматив
                    TextBox Normative = new TextBox();
                    Normative.Width = 50;
                    Normative.HorizontalAlignment = HorizontalAlignment.Left;
                    Normative.Text = "0,1";
                    Label NormativeLabel = new Label();
                    NormativeLabel.Content = "Норматив, кг/смена";
                    NormativeLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    //продолжительность
                    TextBox Duration = new TextBox();
                    Duration.Width = 50;
                    Duration.HorizontalAlignment = HorizontalAlignment.Left;
                    Duration.Text = "0";
                    Label DurationLabel = new Label();
                    DurationLabel.Content = "Продолжительность, дни";
                    DurationLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    //остаток нефтепродукта
                    TextBox Remainder = new TextBox();
                    Remainder.Width = 50;
                    Remainder.HorizontalAlignment = HorizontalAlignment.Left;
                    Remainder.Text = "15";
                    Label RemainderLabel = new Label();
                    RemainderLabel.Content = "Остаток нефтепродукта, %";
                    RemainderLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    Button Calc = new Button();
                    Calc.Width = 75;
                    Calc.Height = 20;
                    Calc.Content = "Рассчитать";
                    Calc.HorizontalAlignment = HorizontalAlignment.Left;

                    CalcPanel.Children.Add(CountOfWorkersLabel);
                    CalcPanel.Children.Add(CountOfWorkers);
                    CalcPanel.Children.Add(NormativeLabel);
                    CalcPanel.Children.Add(Normative);
                    CalcPanel.Children.Add(DurationLabel);
                    CalcPanel.Children.Add(Duration);
                    CalcPanel.Children.Add(RemainderLabel);
                    CalcPanel.Children.Add(Remainder);
                    CalcPanel.Children.Add(new Label());
                    CalcPanel.Children.Add(Calc);


                    Calc.Click += CalcWaste_Click;

                    void CalcWaste_Click(object sender1, RoutedEventArgs e1)
                    {
                        int CountOfWorkersD = Convert.ToInt32(CountOfWorkers.Text);
                        double NormativeD = Convert.ToDouble(Normative.Text);
                        int DurationD = Convert.ToInt32(Duration.Text);
                        double ReminderD = Convert.ToDouble(Remainder.Text);

                        double nN = Math.Round(CountOfWorkersD * NormativeD * DurationD / 1000 * (ReminderD / 100 + 1), 3);
                        string nNs = Convert.ToString(nN);
                        SQLquery.UpdateNormative("database.db", waste.Id, nNs);
                        WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
                    }
                }

                else if (waste.FKKOcode == "9 19 100 02 20 4" || waste.FKKOcode == "9 19 111 21 20 4" || waste.FKKOcode == "9 19 111 24 20 4")
                {
                    //шлак сварочный

                    CalcPanel.Children.Clear();

                    //Количество сварочных электродов
                    TextBox Count = new TextBox();
                    Label CountLabel = new Label();
                    CountLabel.Content = "Количество сварочных электродов, тонн";
                    CountLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    Count.HorizontalAlignment = HorizontalAlignment.Left;
                    Count.Width = 50;
                    Count.Text = "0";
                    //норматив
                    TextBox Normative = new TextBox();
                    Normative.Width = 50;
                    Normative.HorizontalAlignment = HorizontalAlignment.Left;
                    Normative.Text = "10";
                    Label NormativeLabel = new Label();
                    NormativeLabel.Content = "Норматив образования шлака, %";
                    NormativeLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    Button Calc = new Button();
                    Calc.Width = 75;
                    Calc.Height = 20;
                    Calc.Content = "Рассчитать";
                    Calc.HorizontalAlignment = HorizontalAlignment.Left;

                    CalcPanel.Children.Add(CountLabel);
                    CalcPanel.Children.Add(Count);
                    CalcPanel.Children.Add(NormativeLabel);
                    CalcPanel.Children.Add(Normative);
                    CalcPanel.Children.Add(new Label());
                    CalcPanel.Children.Add(Calc);

                    Calc.Click += CalcWaste_Click;

                    void CalcWaste_Click(object sender1, RoutedEventArgs e1)
                    {
                        double CountD = Convert.ToDouble(Count.Text);
                        double NormativeD = Convert.ToDouble(Normative.Text);

                        double nN = Math.Round(CountD * NormativeD / 100, 3);
                        string nNs = Convert.ToString(nN);

                        SQLquery.UpdateNormative("database.db", waste.Id, nNs);
                        WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
                    }
                }

                else if (waste.FKKOcode == "7 33 100 01 72 4")
                {
                    CalcPanel.Children.Clear();

                    //Мусор бытовой
                    TextBox People = new TextBox();
                    People.Width = 50;
                    People.HorizontalAlignment = HorizontalAlignment.Left;
                    People.Text = "0";
                    Label PeopleLabel = new Label();
                    PeopleLabel.Content = "Количество людей, чел.";
                    PeopleLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    TextBox Normative = new TextBox();
                    Normative.Width = 50;
                    Normative.HorizontalAlignment = HorizontalAlignment.Left;
                    Normative.Text = "0,04";
                    Label NormativeLabel = new Label();
                    NormativeLabel.Content = "Норматив образования на 1 человека в год";
                    NormativeLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    TextBox Duration = new TextBox();
                    Duration.Width = 50;
                    Duration.HorizontalAlignment = HorizontalAlignment.Left;
                    Duration.Text = "0";
                    Label DurationLabel = new Label();
                    DurationLabel.Content = "Продолжительность образования, дней";
                    DurationLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    Button Calc = new Button();
                    Calc.Width = 75;
                    Calc.Height = 20;
                    Calc.Content = "Рассчитать";
                    Calc.HorizontalAlignment = HorizontalAlignment.Left;

                    CalcPanel.Children.Add(PeopleLabel);
                    CalcPanel.Children.Add(People);
                    CalcPanel.Children.Add(NormativeLabel);
                    CalcPanel.Children.Add(Normative);
                    CalcPanel.Children.Add(DurationLabel);
                    CalcPanel.Children.Add(Duration);
                    CalcPanel.Children.Add(new Label());
                    CalcPanel.Children.Add(Calc);

                    Calc.Click += CalcWaste_Click;

                    void CalcWaste_Click(object sender1, RoutedEventArgs e1)
                    {
                        int PeopleD = Convert.ToInt32(People.Text);
                        double NormativeD = Convert.ToDouble(Normative.Text);
                        int DurationD = Convert.ToInt32(Duration.Text);

                        double nN = Math.Round(PeopleD * NormativeD * DurationD / 365, 3);
                        string nNs = Convert.ToString(nN);

                        SQLquery.UpdateNormative("database.db", waste.Id, nNs);
                        WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
                    }
                }

                else
                {
                    CalcPanel.Children.Clear();
                }
            }
        }

        private void WasteDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Waste p = e.Row.Item as Waste;
            try
            {
                int selectedColumn = WasteDataGrid.CurrentCell.Column.DisplayIndex;
                var selectedCell = WasteDataGrid.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                var res = cellContent as System.Windows.Controls.TextBox;

                SQLquery.UpdateNormative("database.db", p.Id, res.Text);
            }
            catch { }
        }
    }
}
