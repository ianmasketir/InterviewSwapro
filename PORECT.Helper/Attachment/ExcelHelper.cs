using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace PORECT.Helper
{
    public class ExcelHelper
    {
        /// <summary>
        /// Extract data from excel
        /// </summary>
        /// <param name="attachment">Converted IFormfile type as Class Model</param>
        /// <param name="sheetName">Name of the sheet (case sensitive)</param>
        /// <param name="ignoreEmptyRow">Set false if want to include empty row</param>
        /// <returns>List of data as List Object</returns>
        public List<object[]> ExtractData(Attachment attachment, string? sheetName = null, bool ignoreEmptyRow = true)
        {
            try
            {
                List<object[]> data = new List<object[]>();
                using (var stream = attachment.FormFileContent.OpenReadStream())
                {
                    using (IWorkbook workbook = attachment.MimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ?
                                                new XSSFWorkbook(stream) : //.xlsx
                                                new HSSFWorkbook(stream)) //.xls
                    {
                        ISheet sheet = !string.IsNullOrEmpty(sheetName) ? workbook.GetSheet(sheetName) :
                                        workbook.GetSheetAt(0);
                        IFormulaEvaluator formulaEvaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();

                        if (sheet.LastRowNum > 0)
                        {
                            for (int row = 0; row <= sheet.LastRowNum; row++)
                            {
                                IRow currentRow = sheet.GetRow(row);// ?? sheet.CreateRow(row);
                                if (currentRow != null)
                                {
                                    var rowData = new List<object>();
                                    for (int col = 0; col < currentRow.LastCellNum; col++)
                                    {
                                        ICell cell = currentRow.GetCell(col, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                        string cellValue = GetCellValue(cell, formulaEvaluator);
                                        rowData.Add(cellValue);
                                    }

                                    if (ignoreEmptyRow)
                                    {
                                        var empty = rowData.Where(x => string.IsNullOrEmpty(x.ToString())).ToList();
                                        if (empty.Count < rowData.Count)
                                            data.Add(rowData.ToArray());
                                    }
                                    else
                                        data.Add(rowData.ToArray());
                                }
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get cell value. Any formula will be executed.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="formulaEvaluator"></param>
        /// <returns>Data as string</returns>
        private string GetCellValue(ICell cell, IFormulaEvaluator formulaEvaluator)
        {
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString();
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    var evaluatedCell = formulaEvaluator.Evaluate(cell);
                    return evaluatedCell?.FormatAsString() ?? string.Empty;
                case CellType.Blank:
                    return string.Empty;
                default:
                    return cell.ToString();
            }
        }

        #region Specific Module
        //public byte[] DownloadReport(decimal total, string filePath, DateTime period, string createdBy)
        public byte[] DownloadReport(decimal total, string filePath, int month, int year, string createdBy)
        {
            try
            {
                using (FileStream fs = System.IO.File.OpenRead(filePath))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);

                    ISheet worksheet = workbook.GetSheet("Room Bookng");

                    if (worksheet != null)
                    {
                        ICell cell = worksheet.GetRow(1).GetCell(8); //D:10
                        var cellValue = cell.StringCellValue;
                        cell.SetCellValue(cellValue.Replace("[FormDate]", DateTime.Now.ToString("dd MMM yyyy")));//replace placeholder
                        ICell cell1 = worksheet.GetRow(9).GetCell(3); //D:10
                        cell1.SetCellValue(createdBy);
                        ICell cell2 = worksheet.GetRow(10).GetCell(3); //D:11
                        cell2.SetCellValue(string.Format("Rp. {0}", total.ToString("N2")));
                        ICell cell3 = worksheet.GetRow(8).GetCell(9); //J:9
                        cell3.SetCellValue(string.Format("{0} {1}", 
                            month.ToMonthEnum().ToString().ToIDNMonth(),
                            year.ToString()
                        ));
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.Write(stream, false);
                        return stream.ToArray();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion Specific Module
    }
}
