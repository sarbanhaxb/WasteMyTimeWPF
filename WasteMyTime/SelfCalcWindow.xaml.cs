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
using WasteMyTime.WasteCalcs;

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

        private void WasteDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WasteDataGrid.SelectedItem != null && WasteDataGrid.SelectedItem is Waste waste)
            {
                if (waste.FKKOcode == "9 19 100 02 20 4" || waste.FKKOcode == "9 19 111 21 20 4" || waste.FKKOcode == "9 19 111 24 20 4")
                {
                    WeldingSlag weldingSlag = new WeldingSlag();
                    weldingSlag.Owner = this;
                    weldingSlag.ShowDialog();
                }
            }
        }
    }
}