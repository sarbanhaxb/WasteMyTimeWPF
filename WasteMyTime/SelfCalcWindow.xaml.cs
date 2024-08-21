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
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.IO;
using DocumentFormat.OpenXml.Office2013.Word;

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
                    DurationLabel.Content = "Количество смен";
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

                        double nN = Math.Round(CountOfWorkersD * NormativeD * DurationD / 1000 * (ReminderD / 100 + 1), 3);
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
                    DurationLabel.Content = "Количество смен";
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
                else if (waste.Title.StartsWith("смет с территории") || waste.FKKOcode.StartsWith("7 33 39"))
                {
                    CalcPanel.Children.Clear();

                    //смет с территории
                    Label SquareLabel = new Label();
                    SquareLabel.Content = "Площадь убираемой территории, кв.м.";
                    SquareLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    TextBox Square = new TextBox();
                    Square.HorizontalAlignment = HorizontalAlignment.Left;
                    Square.Text = "0";
                    Square.Width = 50;

                    TextBox Normative = new TextBox();
                    Normative.Width = 50;
                    Normative.HorizontalAlignment = HorizontalAlignment.Left;
                    Normative.Text = "5";
                    Label NormativeLabel = new Label();
                    NormativeLabel.Content = "Удельный норматив, образования отхода, кг/кв.м.";
                    NormativeLabel.HorizontalAlignment = HorizontalAlignment.Left;

                    Button Calc = new Button();
                    Calc.Width = 75;
                    Calc.Height = 20;
                    Calc.Content = "Рассчитать";
                    Calc.HorizontalAlignment = HorizontalAlignment.Left;

                    CalcPanel.Children.Add(SquareLabel);
                    CalcPanel.Children.Add(Square);
                    CalcPanel.Children.Add(NormativeLabel);
                    CalcPanel.Children.Add(Normative);
                    CalcPanel.Children.Add(new Label());
                    CalcPanel.Children.Add(Calc);

                    Calc.Click += CalcWaste_Click;

                    void CalcWaste_Click(object sender1, RoutedEventArgs e1)
                    {
                        double SquareD = Convert.ToDouble(Square.Text);
                        double NormativeD = Convert.ToDouble(Normative.Text);

                        double nN = Math.Round(SquareD * NormativeD / 1000 , 3);
                        string nNs = Convert.ToString(nN);

                        SQLquery.UpdateNormative("database.db", waste.Id, nNs);
                        WasteDataGrid.ItemsSource = SQLquery.LoadWaste("database.db", CalcOptionId);
                    }

                }

                else
                {
                    CalcPanel.Children.Clear();
                    Label label = new Label();
                    label.Content = "Методика отсутствует";
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;

                    CalcPanel.Children.Add(label);
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

        private void CalcWaste_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<BDOItem, double> list = new Dictionary<BDOItem, double>();
            foreach (var dr in WasteDataGrid.ItemsSource)
            {
                if (dr is Waste waste)
                {
                    list.Add(SQLquery.getBDOItem("BDO.db", waste.FKKOcode), waste.Normative);
                }
            }

            string fileName = "ex.docx";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "*.docx";
            saveFileDialog.Filter = "Microsoft Word document (.docx)|*.docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;
            }
            

            using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = new Body();
                //var doc = document.MainDocumentPart.Document;
                Table table = new Table();

                TableProperties tableProperties = new TableProperties
                    (
                    new TableBorders
                        (
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 11 }
                        )
                     );
                table.AppendChild<TableProperties>(tableProperties);

                for (int i = 0; i < 1; i++) 
                {
                    var tr = new TableRow();
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text("Наименование\r\nотхода\r\n"))));
                    tc.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc);

                    var tc1 = new TableCell();
                    tc1.Append(new Paragraph(new Run(new Text("Код по ФККО"))));
                    tc1.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc1);

                    var tc2 = new TableCell();
                    tc2.Append(new Paragraph(new Run(new Text("Класс опасности по\r\nПриказу МПР №242\r\n от 22.05.2017\r\n"))));
                    tc2.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc2);


                    var tc3 = new TableCell();
                    tc3.Append(new Paragraph(new Run(new Text("Агрегатное состояние"))));
                    tc3.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc3);

                    var tc4 = new TableCell();
                    tc4.Append(new Paragraph(new Run(new Text("Состав отхода"))));
                    tc4.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc4);

                    var tc5 = new TableCell();
                    tc5.Append(new Paragraph(new Run(new Text("Норматив \r\nобразования отходов, \r\nт/период\r\n"))));
                    tc5.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc5);

                    var tc6 = new TableCell();
                    tc6.Append(new Paragraph(new Run(new Text("Срок накопления отходов"))));
                    tc6.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc6);

                    var tc7 = new TableCell();
                    tc7.Append(new Paragraph(new Run(new Text("Порядок обращения с\r\nотходом"))));
                    tc7.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc7);

                    var tc8 = new TableCell();
                    tc8.Append(new Paragraph(new Run(new Text("Место накопления "))));
                    tc8.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc8);

                    var tc9 = new TableCell();
                    tc9.Append(new Paragraph(new Run(new Text("Возможный контрагент (вид обращения)"))));
                    tc9.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc9);

                    table.Append(tr);
                }

                foreach (var item in list)
                {
                    var tr = new TableRow();
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(item.Key.Title))));
                    tc.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc);

                    var tc1 = new TableCell();
                    tc1.Append(new Paragraph(new Run(new Text(item.Key.Number))));
                    tc1.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc1);

                    var tc2 = new TableCell();
                    tc2.Append(new Paragraph(new Run(new Text(item.Key.HazardClass))));
                    tc2.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc2);


                    var tc3 = new TableCell();
                    tc3.Append(new Paragraph(new Run(new Text(item.Key.PhysicalState))));
                    tc3.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc3);

                    var tc4 = new TableCell();
                    tc4.Append(new Paragraph(new Run(new Text(item.Key.Compound))));
                    tc4.Append(new Paragraph(new Run(new Text(item.Key.CompoundNotice))));
                    tc4.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc4);

                    var tc5 = new TableCell();
                    tc5.Append(new Paragraph(new Run(new Text(Convert.ToString(item.Value)))));
                    tc5.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc5);

                    var tc6 = new TableCell();
                    tc6.Append(new Paragraph(new Run(new Text("Не более 11 месяцев"))));
                    tc6.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc6);

                    var tc7 = new TableCell();
                    tc7.Append(new Paragraph(new Run(new Text("Обработка/\nобезвреживание/\nутилизация/\nразмещение"))));
                    tc7.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc7);

                    var tc8 = new TableCell();
                    tc8.Append(new Paragraph(new Run(new Text("Контейнер\nс закрывающейся крышкой"))));
                    tc8.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc8);

                    var tc9 = new TableCell();
                    tc9.Append(new Paragraph(new Run(new Text("ООО РОГА И КОПЫТА"))));
                    tc9.Append(new TableCellProperties(new TableCellWidth { Type = TableWidthUnitValues.Auto }));
                    tr.Append(tc9);

                    table.Append(tr);

                }

                docBody.Append(table);
                mainPart.Document.Append(docBody);
                mainPart.Document.Save();
            }





        }
    }
}
