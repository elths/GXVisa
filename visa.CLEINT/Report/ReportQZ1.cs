using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace visa.CLEINT.Report
{
    public partial class ReportQZ1 : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportQZ1()
        {
            InitializeComponent();
        }

        private void ReportQZ1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Detail.Height = (PageHeight) / 2;
        }
    }
}
