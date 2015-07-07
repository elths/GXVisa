using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace visa.CLEINT.Report
{
    public partial class ReportTable4BatByNumber : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportTable4BatByNumber()
        {
            InitializeComponent();
        }

        private void xrCompany_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            cell.Text = "";
            if (this.GetCurrentColumnValue("QID") != null)
            {
               // cell.Text = DataAccess.DaDDD.GetValue<string>("Customer", "CreateCompany", " ID = '" + this.GetCurrentColumnValue("QID").ToString() + "' ");
            }
        }

        private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((this.RowCount == 0) || (this.CurrentRowIndex == this.RowCount - 1))
            {
                xrTableCell22.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            }
        }

        private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            cell.Text = "";
            if (this.GetCurrentColumnValue("QID") != null)
            {
               // cell.Text = DataAccess.DaDDD.GetValue<string>("SendDtl", "ID", " CustomerID = '" + this.GetCurrentColumnValue("QID").ToString() + "' ");
            }
        }

    }
}
