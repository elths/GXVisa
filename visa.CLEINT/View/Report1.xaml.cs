using DevExpress.Xpf.Printing;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Report1.xaml 的交互逻辑
    /// </summary>
    public partial class Report1 : UserControl
    {
        visaEntities visaORM = new visaEntities();

        Customer cc;

        public Report1()
        {
            InitializeComponent();

            cc = visaORM.Customer.FirstOrDefault(c => c.FID == 1);

            InitData();
            //DataContext = this;
            //VisaEdit ReportModel = new VisaEdit();
        }

        public Report1(string printID)
        {
            InitializeComponent();
            string keyString = printID;
            var model = visaORM.Customer.FirstOrDefault(c => c.FAutoID == keyString || c.FPassportNo == keyString);
            if (model == null)
            {
                MessageBox.Show("找不到对应的签证");
                return;
            }

            Report.ReportTable1 report = new Report.ReportTable1();
            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);
            MainPreview.Model = new XtraReportPreviewModel(report);


            report.CreateDocument();

        }

        public Report1(List<string> printID)
        {
            InitializeComponent();

            string keyString = "";

            foreach (string autoID in printID)
            {
                keyString +=  autoID + ",";
            }

            Report.ReportTable1 report = new Report.ReportTable1();
            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);
            MainPreview.Model = new XtraReportPreviewModel(report);

            DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
            pt.Print();

            report.CreateDocument();

        }


        private void InitData()
        {
            //SimpleLink MainLink = new SimpleLink();


            //MainLink.ReportHeaderTemplate = (DataTemplate)Resources["HeaderTemplate"];
            //MainPreview.Model = new LinkPreviewModel(MainLink);
            //MainLink.PageHeaderData = cc;
            //MainLink.CreateDocument(true);


            //report.DataSource = visaORM.Customer.FirstOrDefault(c => c.FID == 1);
        }

        private void txtQZID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string keyString = txtQZID.Text;
                var model = visaORM.Customer.FirstOrDefault(c => c.FAutoID == keyString || c.FPassportNo == keyString);
                if (model == null)
                {
                    MessageBox.Show("找不到对应的签证");
                    return;
                }


                Report.ReportTable1 report = new Report.ReportTable1();
                report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);
                MainPreview.Model = new XtraReportPreviewModel(report);



                //Report.ReportTable1 report = new Report.ReportTable1();
                //report.FilterString = "FAutoID = " + model.FID + " or FPassportNo =  '" + model.FPassportNo + "'";

                MainPreview.Model = new XtraReportPreviewModel(report);


                report.CreateDocument();
            } 


        }


    }
}
