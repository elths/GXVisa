using DevExpress.Xpf.Printing;
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
    /// Report4Batch.xaml 的交互逻辑
    /// </summary>
    public partial class Report4Batch : UserControl
    {
        visaEntities visaORM = new visaEntities();

        DateTime DateFrom;
        DateTime DateTo;
        string StartNumber;
        string EndNumber;
        string FirstLetter;
        public Report4Batch()
        {
            InitializeComponent();
            InitPreview(DateTime.Now, DateTime.Now);

        }



        private void btnGenReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (cbDateFrom.EditValue == null || cbDateTo.EditValue == null)
                {
                    MessageBox.Show("请设置时间段");
                    return;
                }
                DateFrom = cbDateFrom.DateTime;
                DateTo = cbDateTo.DateTime.AddDays(1);

                InitPreview(DateFrom, DateTo);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }

        void InitPreview(DateTime dateFrom, DateTime dateTo)
        {
            try
            {

                if (dateFrom == null || cbDateTo == null)
                {
                    MessageBox.Show("请设置时间段");
                    return;
                }

                Report.ReportTable4Bat report = new Report.ReportTable4Bat();

                report.xrTiaojian.Text = cbDateFrom.DateTime.ToString("dd/MM/yyyy") + " - " + cbDateTo.DateTime.ToString("dd/MM/yyyy");

                report.sp_SelectPrintSendInfoBatchTableAdapter.Fill(report.printSendInfoBatch_SP1.sp_SelectPrintSendInfoBatch, dateFrom, dateTo);
                MainPreview.Model = new XtraReportPreviewModel(report);



                report.xrLabelCaption.Text = txtReportTitle.Text;
                MainPreview.Model = new XtraReportPreviewModel(report);

                report.CreateDocument();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }


        /// <summary>
        /// 根据签证号查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenReportByNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtLetter.Text) || string.IsNullOrEmpty(txtStartNum.Text) || string.IsNullOrEmpty(txtEndNum.Text))
                {
                    MessageBox.Show("请设置号码前缀与开始结束数字");
                    return;
                }
                FirstLetter = txtLetter.Text;
                StartNumber = txtStartNum.Text;
                EndNumber = txtEndNum.Text;

                InitPreviewByNumber();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        void InitPreviewByNumber()
        {

            try
            {

                if (string.IsNullOrEmpty(FirstLetter) || string.IsNullOrEmpty(StartNumber) || string.IsNullOrEmpty(EndNumber))
                {
                    MessageBox.Show("请设置号码前缀与开始结束数字");
                    return;
                }

                Report.ReportTable4BatByNumber report = new Report.ReportTable4BatByNumber();

                report.xrTiaojian.Text = FirstLetter + StartNumber + " - " + FirstLetter + EndNumber;

                report.sp_SelectPrintSendInfoBatchByNumberTableAdapter.Fill(report.sp_SelectPrintSendInfoBatchByNumber1._sp_SelectPrintSendInfoBatchByNumber, FirstLetter, Convert.ToInt32(StartNumber), Convert.ToInt32(EndNumber));
                MainPreview.Model = new XtraReportPreviewModel(report);



                report.xrLabelCaption.Text = txtReportTitle.Text;
                MainPreview.Model = new XtraReportPreviewModel(report);

                report.CreateDocument();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }




    }
}
