using DevExpress.Xpf.Printing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Report4.xaml 的交互逻辑
    /// </summary>
    public partial class Report4 : UserControl
    {
        visaEntities visaORM = new visaEntities();

        ObservableCollection<string> SendCollection = new ObservableCollection<string>();
        public Report4()
        {
            InitializeComponent();
            InitPreview("");
            InitSendCollection(false);
            cbSendNo.ItemsSource = SendCollection;
        }


        public Report4(string SendNo)
        {
            InitializeComponent();
            InitPreview(SendNo);
            InitSendCollection(false);
            cbSendNo.ItemsSource = SendCollection; 
        }


        void InitSendCollection(bool isAll)
        {
            try
            {

                SendCollection.Clear();
                List<string> list = new List<string>();


                if (isAll == false) //只显示一周内的
                {

                    DateTime dt = DateTime.Now.AddDays(-7);
                    list = visaORM.SendInfo.Where(s => s.FCreateDate > dt).OrderByDescending(s => s.FID).Select(s => s.FSendNo).ToList();
                }
                else //显示所有的
                {
                    list = visaORM.SendInfo.OrderByDescending(s => s.FID).Select(s => s.FSendNo).ToList();
                }

                foreach (string sString in list.Distinct())
                {
                    SendCollection.Add(sString);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }


        void InitPreview(string SendNo)
        {
            try
            {

                Report.ReportTable4 report = new Report.ReportTable4();

                report.sp_SelectPrintSendInfoTableAdapter.Fill(report.printSendInfo_SP1.sp_SelectPrintSendInfo, SendNo);
                MainPreview.Model = new XtraReportPreviewModel(report);

                var model = visaORM.SendInfo.FirstOrDefault(s => s.FSendNo == SendNo);

                if (model != null)
                {
                    report.xrLabelDSN.Text = model.FDSN;
                    report.xrLabelComapny.Text = model.FVietnamCompany;

                }
                else
                {
                    report.xrLabelDSN.Text = "";
                    report.xrLabelComapny.Text = "";

                }


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

        private void cbSendNo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbSendNo.SelectedItem != null)
            {
                InitPreview(cbSendNo.SelectedItem.ToString());

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitSendCollection(true);
            DevExpress.Xpf.Core.DXMessageBox.Show("已经可以选择所有历史单号");
        }

    }
}
