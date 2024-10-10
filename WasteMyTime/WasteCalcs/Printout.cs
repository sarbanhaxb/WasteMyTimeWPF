using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WasteMyTime;
using System.IO;
using DocumentFormat.OpenXml.Drawing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace WasteMyTime.WasteCalcs
{
    public class Printout
    {
        private string heading;
        List<dynamic> list = new List<dynamic>();

        public Printout(string heading, List<dynamic> list)
        {
            this.heading = heading;
            this.list = list;
        }

        public void SaveReport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx";
            saveFileDialog.Title = "Сохранить отчет";
            saveFileDialog.FileName = "Отчет";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                CreateWordDocument(filePath);
            }
        }

        private void CreateWordDocument(string filepath)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filepath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Создаем основной документ
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();

                // Добавление заголовка 3 уровня
                Paragraph headingParagraph = new Paragraph(new Run(new Text(heading)))
                {
                    ParagraphProperties = new ParagraphProperties(
                        new OutlineLevel { Val = 2 },  // Уровень 3 оглавления, так как уровень начинается с 0
                        new ParagraphStyleId { Val = "Heading3" }  // Установить стиль заголовка 3 уровня
                    )
                };
                body.Append(headingParagraph);

                // Добавление других параграфов

                foreach (var item in list)
                {
                    body.Append(item);
                }
                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }
        }
    }
}