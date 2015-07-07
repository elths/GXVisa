using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace visa.CLEINT.Report
{
    public partial class ReportTable2 : DevExpress.XtraReports.UI.XtraReport
    {
        private bool isTag = false;

        public ReportTable2()
        {
            InitializeComponent();
        }

        private void xrTableCell_TextChanged(object sender, EventArgs e)
        {
            if (!isTag)
            {
                isTag = true;
                XRTableCell cell = (XRTableCell)sender;
                if ((cell.Text == "True") || (cell.Text == "1"))
                {
                    cell.Text = "¡Ì";
                }
                else
                {
                    cell.Text = "";
                }
                isTag = false;
            }
        }
    }
}
