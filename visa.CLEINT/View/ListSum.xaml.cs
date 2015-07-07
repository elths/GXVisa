using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Grid;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using visa.MODEL;
using DevExpress.Xpf.Editors.Settings;

namespace visa.CLEINT.View
{
    /// <summary>
    /// ListSum.xaml 的交互逻辑
    /// </summary>
    public partial class ListSum : UserControl
    {
        visaEntities visaORM = new visaEntities();
        List<sp_Customer_Select_ByDate_Result> list = new List<sp_Customer_Select_ByDate_Result>();

        public ListSum()
        {
            InitializeComponent();
            //cbCompany.ItemsSource = visaORM.User.Select(u => u.FCompanyName).Distinct();

            InitFilter();
            filter.ApplyFilter();

        }

        DateTime? DateFrom;
        DateTime? DateTo;

        void InitFilter()
        {
            List<FilterColumn> columns = new List<FilterColumn>();

            GridColumn column = gridMain.Columns["FPurpose"];
            FilterColumn newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);
            column = gridMain.Columns["FCreateCompany"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);

            column = gridMain.Columns["FPassportNo"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);

            columns.Add(newColumn);
            column = gridMain.Columns["FName"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);


            column = gridMain.Columns["FQZID"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);


            column = gridMain.Columns["FAutoID"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);

            column = gridMain.Columns["FYNCom"];
            newColumn = new FilterColumn()
            {
                FieldName = column.FieldName,
                ColumnCaption = column.Header,
                ColumnType = typeof(string)
            };
            columns.Add(newColumn);


            filter.FilterColumns = columns;

            this.gridMain.FilterString = "[FPurpose] = ? and [FCreateCompany] = ? and [FPassportNo] = ? and [FQZID] = ? and  [FName] = ?  and [FAutoID] = ? and  [FYNCom] = ?";

            dateFrom.EditValue = DateTime.Today.AddDays(-7);
            dateTo.EditValue = DateTime.Today;


            //gridMain.FilterCriteria = new BinaryOperator("FPassportNo","E",BinaryOperatorType.Like);
            //gridMain.FilterCriteria += new BinaryOperator("FName", "", BinaryOperatorType.Like);

        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            //List<Customer> c = visaORM.Customer.ToList();

            //if (dateFrom.Text != "") c = c.Where(u => u.FCreateDate >= Convert.ToDateTime(dateFrom.Text) ).ToList();
            //if (dateTo.Text != "") c = c.Where(u => u.FCreateDate <= Convert.ToDateTime(dateTo.Text)).ToList();
            //if (cbCompany.SelectedItem!=null) 
            //{
            //    string FCreateCompanyString = cbCompany.SelectedItem.ToString();
            //    c = c.Where(u => u.FCreateCompany == FCreateCompanyString).ToList();
            //}

            //gridMain.ItemsSource = c;
            //tbSum.Text = string.Format("共有数据 {0} 条", c.Count.ToString());
            filter.ApplyFilter();

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dateFrom.Text = "";
            dateTo.Text = "";
            //cbCompany.SelectedIndex = -1;
            tbSum.Text = "";
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions;
                ((TableView)gridMain.View).ExportToXlsx(fileDialog.FileName);

                MessageBox.Show("导出成功！", "提示");
            }
        }

        private void dateFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (dateFrom.DateTime != null && dateTo.DateTime != null)
            {
                DateFrom = dateFrom.DateTime;
                DateTo = dateTo.DateTime.AddDays(1);
                list = visaORM.sp_Customer_Select_ByDate(DateFrom, DateTo).ToList();
                gridMain.ItemsSource = list;
                //tbSum.Text = string.Format("共有数据 {0} 条", list.Count.ToString());
            }

        }

        private void filter_BeforeShowValueEditor(object sender, ShowValueEditorEventArgs e)
        {
            if (e.CurrentNode.FirstOperand.PropertyName == "FPurpose")
            {
                e.CustomEditSettings = new ComboBoxEditSettings() { ItemsSource = new string[] { "旅游", "企业", "劳务", "领事馆", "投资", "留学", "外交NG1", "外交NG2", "外交NG3", "外交NG4", "其他VR", "其他TT", "其他NN1", "其他NN2", "其他NN3", "其他LV1", "其他LV2", "其他HN" } ,IsTextEditable=true };
            }
            else if (e.CurrentNode.FirstOperand.PropertyName == "FYNCom")
            {
                e.CustomEditSettings = new ComboBoxEditSettings() { ItemsSource = visaORM.TB_VietnamCompany.Where(v=>v.FStatus==true).Select(v=>v.FShortName), IsTextEditable = true };
            }
            else if (e.CurrentNode.FirstOperand.PropertyName == "FCreateCompany")
            {
                e.CustomEditSettings = new ComboBoxEditSettings() { ItemsSource = visaORM.User.Where(u=>u.FIsDelete==false).Select(u => u.FCompanyName).Distinct(), IsTextEditable = true };
            }

        }

    }
}
