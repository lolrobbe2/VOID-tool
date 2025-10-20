using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FoxholeBot.repositories;
using NetCord;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace FoxholeBot.types
{
    public class OutputFormatter
    {
        IDictionary<StockPile, StockPileItem[]> stockpiles { get; set; } = new Dictionary<StockPile, StockPileItem[]>();

        public OutputFormatter AddStockpile(StockPile stockpile, StockPileItem[] items)
        {
            stockpiles.Add(stockpile, items);
            return this;
        }

        public MemoryStream Build()
        {
            var workbook = new XLWorkbook();
            foreach (var stockpile in stockpiles) 
            {
                workbook.AddWorksheet(FormatStockpile(stockpile.Key, stockpile.Value));
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
        private static IXLWorksheet FormatStockpile(StockPile stockpile, StockPileItem[] items)
        {
            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add($"{stockpile.Region.ToLower()}-{stockpile.Name}");
            Header(stockpile, sheet);
            CodeHeader(stockpile, sheet);
            RegionHeader(stockpile, sheet);
            ItemHeader(sheet);
            int offset = 6;
            for (int i = offset; i < items.Length + offset; i++)
            {
                Item(items[i - offset], i, sheet);
            }

            sheet.Cells().Style.Alignment.WrapText = false;

            // Adjust all columns to content
            sheet.Columns().AdjustToContents();
            // Adjust all rows to content
            sheet.Rows().AdjustToContents();

            return sheet;
        }

        private static void RegionHeader(StockPile stockpile, IXLWorksheet sheet)
        {
            sheet.Range("A3:C3").Merge();
            sheet.Cell("A3").Value = $"Region: {stockpile.Region}";
            sheet.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("A3").Style.Font.Bold = true;
            sheet.Cell("A3").Style.Fill.BackgroundColor = XLColor.Black;
            sheet.Cell("A3").Style.Font.FontColor = XLColor.White;

            sheet.Range("A4:C4").Merge();
            sheet.Cell("A4").Value = $"SubRegion: {stockpile.Subregion}";
            sheet.Cell("A4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A4").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("A4").Style.Font.Bold = true;
            sheet.Cell("A4").Style.Fill.BackgroundColor = XLColor.Black;
            sheet.Cell("A4").Style.Font.FontColor = XLColor.White;
        }
        private static void ItemHeader(IXLWorksheet sheet)
        {
            sheet.Cell("A5").Value = $"Item";

            sheet.Cell("A5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("A5").Style.Font.Bold = true;
            sheet.Cell("A5").Style.Font.FontSize = 14;
            sheet.Cell("A5").Style.Fill.BackgroundColor = XLColor.AppleGreen;

            sheet.Cell("B5").Value = $"Amount";
            sheet.Cell("B5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("B5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("B5").Style.Font.Bold = true;
            sheet.Cell("B5").Style.Font.FontSize = 14;
            sheet.Cell("B5").Style.Fill.BackgroundColor = XLColor.AppleGreen;

            sheet.Cell("C5").Value = $"Crated";
            sheet.Cell("C5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("C5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("C5").Style.Font.Bold = true;
            sheet.Cell("C5").Style.Font.FontSize = 14;
            sheet.Cell("C5").Style.Fill.BackgroundColor = XLColor.AppleGreen;

        }
        private static void CodeHeader(StockPile stockpile, IXLWorksheet sheet)
        {
            sheet.Range("A2:C2").Merge();
            sheet.Cell("A2").Value = $"Code: {stockpile.Code}";

            sheet.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("A2").Style.Font.Bold = true;
            sheet.Cell("A2").Style.Font.FontSize = 14;
            sheet.Cell("A2").Style.Fill.BackgroundColor = XLColor.Black;
            sheet.Cell("A2").Style.Font.FontColor = XLColor.White;
        }

        private static void Header(StockPile stockpile, IXLWorksheet sheet)
        {
            sheet.Range("A1:C1").Merge();
            sheet.Cell("A1").Value = $"Name: {stockpile.Name}";

            sheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Cell("A1").Style.Font.Bold = true;
            sheet.Cell("A1").Style.Font.FontSize = 14;
            sheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.RedMunsell;

        }

        private static void Item(StockPileItem item, int index, IXLWorksheet sheet)
        {
            sheet.Cell($"A{index}").Value = item.Name;
            sheet.Cell($"B{index}").Value = item.Count;
            sheet.Cell($"C{index}").Value = item.Crated;

            var range = sheet.Range($"A{index}:C{index}");
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Alternating row background
            if (index % 2 == 0)
            {
                range.Style.Fill.BackgroundColor = XLColor.LightGray;
            }
            else
            {
                range.Style.Fill.BackgroundColor = XLColor.White;
            }

            // Optional: text alignment and wrapping
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            range.Style.Alignment.WrapText = true;
        }

    }
}
