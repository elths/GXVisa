using DevExpress.Data.Filtering;
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

namespace visa.CLEINT.View
{
    /// <summary>
    /// ListName.xaml 的交互逻辑
    /// </summary>
    public partial class ListName : UserControl
    {
        visaEntities visaORM = new visaEntities();


        /// <summary>
        /// 数据源
        /// </summary>
        List<sp_SelectTable1ForListName_Result> list = new List<sp_SelectTable1ForListName_Result>();

        DateTime? DateFrom;
        DateTime? DateTo;

        public ListName()
        {
            InitializeComponent();

            InitFilter();
            filter.ApplyFilter();

        }

        void InitFilter()
        {
            List<FilterColumn> columns = new List<FilterColumn>();

            GridColumn column = gridMain.Columns["FPassportNo"];
            FilterColumn newColumn = new FilterColumn()
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


            filter.FilterColumns = columns;

            this.gridMain.FilterString = "[FPassportNo] = ? and [FAutoID] = ? ";

            dateFrom.EditValue = DateTime.Today.AddDays(-7);
            dateTo.EditValue = DateTime.Today;


        }


        private void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            filter.ApplyFilter();
        }

        private void btnClear_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void dateFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (dateFrom.DateTime != null && dateTo.DateTime != null)
            {
                DateFrom = dateFrom.DateTime;
                DateTo = dateTo.DateTime.AddDays(1);
                list = visaORM.sp_SelectTable1ForListName(DateFrom, DateTo).ToList();
                gridMain.ItemsSource = list;
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchGrid();
        }


        /// <summary>
        /// 原搜索事件
        /// </summary>
        void searchGrid()
        {

            List<Customer> c = visaORM.Customer.ToList();

            if (dateFrom.Text != "") c = c.Where(u => u.FCreateDate >= Convert.ToDateTime(dateFrom.Text)).ToList();
            if (dateTo.Text != "") c = c.Where(u => u.FCreateDate <= Convert.ToDateTime(dateTo.Text)).ToList();
            //if (dateFromSend.Text != "") c = c.Where(u => u.FSysSendDate >= Convert.ToDateTime(dateFrom.Text)).ToList();
            //if (dateToSend.Text != "") c = c.Where(u => u.FSysSendDate <= Convert.ToDateTime(dateTo.Text)).ToList();
            //if (dateFromChk.Text != "") c = c.Where(u => u.FSysChkDate >= Convert.ToDateTime(dateFrom.Text)).ToList();
            //if (dateToChk.Text != "") c = c.Where(u => u.FSysChkDate <= Convert.ToDateTime(dateTo.Text)).ToList();
            //if (txtQZID.Text != "") c = c.Where(u => u.FQZID == txtQZID.Text).ToList();
            //if (txtName.Text != "") c = c.Where(u => u.FName == txtName.Text).ToList();
            //if (txtPassportNo.Text != "") c = c.Where(u => u.FPassportNo == txtPassportNo.Text).ToList();



        }

        /// <summary>
        /// 清空方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dateFrom.Text = "";
            dateTo.Text = "";
            //dateFromSend.Text = "";
            //dateToSend.Text = "";
            //dateFromChk.Text = "";
            //dateToChk.Text = "";
            //txtQZID.Text = "";
            //txtName.Text = "";
            //txtPassportNo.Text = "";
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

        private void btnZFVisa_Click(object sender, RoutedEventArgs e)
        {
            if (gridMain.SelectedItem == null)
            {
                MessageBox.Show("请选择一条数据");
                return;
            }

            if (MessageBox.Show("是否确认作废该签证，作废后将返回到表一修改?", "作废确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int CID = (gridMain.SelectedItem as sp_Customer_Select_ByDate_Result).FID;
                Customer cObj = visaORM.Customer.FirstOrDefault(c => c.FID == CID);
                if (cObj == null)
                {
                    MessageBox.Show("找不到对应签证数据，作废失败");
                    return;
                }

                visaORM.Customer.Attach(cObj);
                cObj.FsysZF = true;
                cObj.FSysSend = false;
                cObj.FSysPut = false;
                cObj.FSysChk = false;
                cObj.FSysPrint = false;
                visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);

                TB_TableSubmit submitObj = visaORM.TB_TableSubmit.FirstOrDefault(s1 => s1.FAutoID == cObj.FAutoID);
                if (submitObj != null)
                {
                    visaORM.DeleteObject(submitObj);
                }
                SendInfo sendObj = visaORM.SendInfo.FirstOrDefault(s2 => s2.FCustomerID == cObj.FID);
                if (sendObj != null)
                {
                    visaORM.DeleteObject(sendObj);
                }

                visaORM.SaveChanges();

                MessageBox.Show("已经作废签证号为 " + cObj.FPassportNo + " 的签证");
                dateFrom_EditValueChanged(null, null);

            }


        }

        private void btnDelVisa_Click(object sender, RoutedEventArgs e)
        {
            if (gridMain.SelectedItem == null)
            {
                MessageBox.Show("请选择一条数据");
                return;
            }

            if (MessageBox.Show("是否确认删除该签证，取消后将不能使用?", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int CID = (gridMain.SelectedItem as sp_Customer_Select_ByDate_Result).FID;
                Customer cObj = visaORM.Customer.FirstOrDefault(c => c.FID == CID);
                if (cObj == null)
                {
                    MessageBox.Show("找不到对应签证数据，作废失败");
                    return;
                }


                TB_TableSubmit submitObj = visaORM.TB_TableSubmit.FirstOrDefault(s1 => s1.FAutoID == cObj.FAutoID);
                if (submitObj != null)
                {
                    visaORM.DeleteObject(submitObj);
                }
                SendInfo sendObj = visaORM.SendInfo.FirstOrDefault(s2 => s2.FCustomerID == cObj.FID);
                if (sendObj != null)
                {
                    visaORM.DeleteObject(sendObj);
                }

                visaORM.DeleteObject(cObj);

                visaORM.SaveChanges();

                MessageBox.Show("已经删除签证号为 " + cObj.FPassportNo + " 的签证");
                dateFrom_EditValueChanged(null, null);

            }
        }


    }
}
