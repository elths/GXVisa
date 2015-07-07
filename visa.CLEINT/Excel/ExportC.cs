using System;
using System.Collections.Generic;
using System.Text;
using EX = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.Common;
using visa.MODEL;
using System.Data.Objects;

namespace visa.CLEINT.Excel
{
    public class ExportC
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);


        public static void ExportTable3(string saveFileName, List<sp_SelectTable3ForExcel_Result> dataRows, string datetime, string dsn, string company, List<MessageInfo> messages)
        {
            try
            {
                string fileName;
                fileName = AppDomain.CurrentDomain.BaseDirectory + "Excel\\Table33.xlsx";

                EX.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = true;
                EX.Workbook book = app.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                EX.Worksheet sheet = (EX.Worksheet)book.Worksheets[1];

                sheet.Cells[1, "A"] = datetime;

                //string[] dsnArray = dsn.Split('-');
                //if (dsnArray.Length<3)
                //{
                //    Log.WriteLog.WriteOperateLog("导出失败，DSN为：" + dsn);
                //    MessageBox.Show("导出失败，DSN为：" + dsn);
                //    return;
                //}
                //sheet.Cells[1, "E"] = company + ("-") + dsnArray[1] + ("-") + dsnArray[2];
                sheet.Cells[1, "E"] = dsn;
                sheet.Cells[1, "H"] = company;
                //sheet.Cells[1, "I"] = MainContext.UserCompanyName;

                int row = 3;
                foreach (var dataRow in dataRows)
                {
                    row++;
                    sheet.Cells[row, "A"] = row - 3;
                    sheet.Cells[row, "B"] = dataRow.Name;
                    sheet.Cells[row, "C"] = dataRow.BirthDay;
                    sheet.Cells[row, "D"] = dataRow.Sex;
                    sheet.Cells[row, "E"] = dataRow.FBirthNationlityPresentEn;
                    sheet.Cells[row, "F"] = dataRow.FBirthNationlityEn;
                    sheet.Cells[row, "G"] = "'" + dataRow.PID;
                    //sheet.Cells[row, "G"] = dataRow.Work;
                    sheet.Cells[row, "H"] = ((DateTime)(dataRow.FPassportValidDate)).ToString("dd/MM/yyyy");
                    sheet.Cells[row, "I"] = dataRow.CQJDate;
                    sheet.Cells[row, "J"] = dataRow.TimesName;
                    sheet.Cells[row, "K"] = dataRow.FCreateCompany;
                    sheet.Cells[row, "L"] = dataRow.FSysMemo;

                    EX.Range range = sheet.Range[sheet.Cells[row, "A"], sheet.Cells[row, "L"]];

                    switch (dataRow.iType)
                    {
                        case 1:
                            break;
                        case 2:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 3:
                            range.Font.ColorIndex = 5;// 蓝色
                            break;
                        case 4:
                            range.Font.ColorIndex = 3;// 红色
                            break;
                        case 5:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 6:
                        case 7:
                            range.Font.ColorIndex = 5;
                            break;
                        case 8:
                            range.Font.ColorIndex = 3;
                            break;
                        case 9:
                            range.Font.ColorIndex = 45;//橙色
                            break;
                        case 10:
                            range.Font.ColorIndex = 5;
                            break;
                        case 11:
                            range.Font.ColorIndex = 26;//粉红色
                            break;
                        case 12:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 13:
                            range.Font.ColorIndex = 26;
                            break;
                        case 14:
                            range.Font.ColorIndex = 5;
                            break;
                        case 15:
                            range.Font.ColorIndex = 3;
                            break;
                        case 16:
                            range.Font.ColorIndex = 16;
                           break;
                        case 17:
                            range.Font.ColorIndex = 5;
                            break;
                        case 18:
                            range.Font.ColorIndex = 3;
                            break;
                        case 19:
                            range.Font.ColorIndex = 5;
                            break;
                        case 20:
                            range.Font.ColorIndex = 3;
                            break;
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                            range.Font.ColorIndex = 16;
                           break;
                        case 25:
                        case 26:
                           break;
                        default:
                            break;
                    }
                }

                EX.Range range22 = sheet.Range[sheet.Cells[2, "A"], sheet.Cells[row, "L"]];

                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft);
                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop);
                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom);
                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight);
                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical);
                NewMethod(range22, Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal);

                foreach (MessageInfo info in messages)
                {
                    row++;
                    EX.Range range = sheet.Range[sheet.Cells[row, "B"], sheet.Cells[row, "I"]];
                    range.Merge(Missing.Value);
                    sheet.Cells[row, "B"] = info.message;

                    switch (info.iType)
                    {
                        case 1:
                            break;
                        case 2:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 3:
                            range.Font.ColorIndex = 5;// 蓝色
                            break;
                        case 4:
                            range.Font.ColorIndex = 3;// 红色
                            break;
                        case 5:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 6:
                        case 7:
                            range.Font.ColorIndex = 5;
                            break;
                        case 8:
                            range.Font.ColorIndex = 3;
                            break;
                        case 9:
                            range.Font.ColorIndex = 45;//橙色
                            break;
                        case 10:
                            range.Font.ColorIndex = 5;
                            break;
                        case 11:
                            range.Font.ColorIndex = 26;//粉红色
                            break;
                        case 12:
                            range.Font.ColorIndex = 10;//绿色
                            break;
                        case 13:
                            range.Font.ColorIndex = 26;
                            break;
                        case 14:
                            range.Font.ColorIndex = 5;
                            break;
                        case 15:
                            range.Font.ColorIndex = 3;
                            break;
                        case 16:
                            range.Font.ColorIndex = 16;
                            break;
                        case 17:
                            range.Font.ColorIndex = 5;
                            break;
                        case 18:
                            range.Font.ColorIndex = 3;
                            break;
                        case 19:
                            range.Font.ColorIndex = 5;
                            break;
                        case 20:
                            range.Font.ColorIndex = 3;
                            break;
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                            range.Font.ColorIndex = 16;
                            break;
                        case 25:
                        case 26:
                            break;
                        default:
                            break;
                    }
                }

                book.SaveAs(saveFileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                //book.PrintPreview();
                book.PrintOutEx();
                book.Close(Missing.Value, Missing.Value, Missing.Value);

                IntPtr t = new IntPtr(app.Hwnd);
                int id = 0;
                GetWindowThreadProcessId(t, out id);
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(id);
                p.Kill();
                MessageBox.Show("导出成功，生成的文件位置为：" + saveFileName);
            }
            catch (System.Exception ex)
            {
                Log.WriteLog.WriteErorrLog(ex);
                MessageBox.Show(ex.ToString());
            }

        }


        private static void NewMethod(EX.Range range, Microsoft.Office.Interop.Excel.XlBordersIndex index)
        {
            range.Borders[index].LineStyle = EX.XlLineStyle.xlContinuous;
            range.Borders[index].Weight = EX.XlBorderWeight.xlThin;
            range.Borders[index].ColorIndex = EX.Constants.xlAutomatic;
        }

    }


    public struct MessageInfo
    {
        public string message;
        public int days;
        public int times;
        public string pur;
        public int? iType;
        public MessageInfo(string message, int days, int times, string pur)
        {
            this.message = message;
            this.days = days;
            this.times = times;
            this.pur = pur;
            this.iType = 0;
        }
        public MessageInfo(string message, int? iType)
        {
            this.message = message;
            this.iType = iType;
            this.days = 0;
            this.times = 0;
            this.pur = "";
        }
    }

}
