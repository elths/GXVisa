namespace visa.CLEINT.Report
{
    partial class ReportQZ1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabelNation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelRemarks = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelTimes = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelBirthDay = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelSex = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelExpirationDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelIssueDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelVisaType = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelVID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelPID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelMName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelGName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelSName = new DevExpress.XtraReports.UI.XRLabel();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelNation,
            this.xrLabelRemarks,
            this.xrLabelTimes,
            this.xrLabelBirthDay,
            this.xrLabelSex,
            this.xrLabelExpirationDate,
            this.xrLabelIssueDate,
            this.xrLabelVisaType,
            this.xrLabelVID,
            this.xrLabelPID,
            this.xrLabelMName,
            this.xrLabelGName,
            this.xrLabelSName});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 1008F;
            this.Detail.KeepTogether = true;
            this.Detail.MultiColumn.ColumnCount = 2;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabelNation
            // 
            this.xrLabelNation.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.CountryShort")});
            this.xrLabelNation.Dpi = 254F;
            this.xrLabelNation.LocationFloat = new DevExpress.Utils.PointFloat(381F, 529F);
            this.xrLabelNation.Name = "xrLabelNation";
            this.xrLabelNation.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelNation.SizeF = new System.Drawing.SizeF(254F, 63F);
            this.xrLabelNation.Text = "xrLabelNation";
            // 
            // xrLabelRemarks
            // 
            this.xrLabelRemarks.Dpi = 254F;
            this.xrLabelRemarks.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelRemarks.LocationFloat = new DevExpress.Utils.PointFloat(42F, 826F);
            this.xrLabelRemarks.Name = "xrLabelRemarks";
            this.xrLabelRemarks.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelRemarks.SizeF = new System.Drawing.SizeF(1101F, 148F);
            this.xrLabelRemarks.StylePriority.UseFont = false;
            this.xrLabelRemarks.Text = "xrLabelRemarks";
            // 
            // xrLabelTimes
            // 
            this.xrLabelTimes.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.TimesName")});
            this.xrLabelTimes.Dpi = 254F;
            this.xrLabelTimes.LocationFloat = new DevExpress.Utils.PointFloat(635F, 720F);
            this.xrLabelTimes.Name = "xrLabelTimes";
            this.xrLabelTimes.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelTimes.SizeF = new System.Drawing.SizeF(254F, 63F);
            this.xrLabelTimes.Text = "xrLabelTimes";
            // 
            // xrLabelBirthDay
            // 
            this.xrLabelBirthDay.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.BirthDay")});
            this.xrLabelBirthDay.Dpi = 254F;
            this.xrLabelBirthDay.LocationFloat = new DevExpress.Utils.PointFloat(85F, 635F);
            this.xrLabelBirthDay.Name = "xrLabelBirthDay";
            this.xrLabelBirthDay.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBirthDay.SizeF = new System.Drawing.SizeF(318F, 64F);
            this.xrLabelBirthDay.Text = "xrLabelBirthDay";
            // 
            // xrLabelSex
            // 
            this.xrLabelSex.Dpi = 254F;
            this.xrLabelSex.LocationFloat = new DevExpress.Utils.PointFloat(85F, 529F);
            this.xrLabelSex.Name = "xrLabelSex";
            this.xrLabelSex.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelSex.SizeF = new System.Drawing.SizeF(254F, 64F);
            this.xrLabelSex.Text = "xrLabelSex";
            // 
            // xrLabelExpirationDate
            // 
            this.xrLabelExpirationDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.EDate")});
            this.xrLabelExpirationDate.Dpi = 254F;
            this.xrLabelExpirationDate.LocationFloat = new DevExpress.Utils.PointFloat(677F, 614F);
            this.xrLabelExpirationDate.Name = "xrLabelExpirationDate";
            this.xrLabelExpirationDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelExpirationDate.SizeF = new System.Drawing.SizeF(360F, 64F);
            this.xrLabelExpirationDate.Text = "xrLabelExpirationDate";
            // 
            // xrLabelIssueDate
            // 
            this.xrLabelIssueDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.IDate")});
            this.xrLabelIssueDate.Dpi = 254F;
            this.xrLabelIssueDate.LocationFloat = new DevExpress.Utils.PointFloat(677F, 529F);
            this.xrLabelIssueDate.Name = "xrLabelIssueDate";
            this.xrLabelIssueDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelIssueDate.SizeF = new System.Drawing.SizeF(360F, 64F);
            this.xrLabelIssueDate.Text = "xrLabelIssueDate";
            // 
            // xrLabelVisaType
            // 
            this.xrLabelVisaType.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.VType")});
            this.xrLabelVisaType.Dpi = 254F;
            this.xrLabelVisaType.LocationFloat = new DevExpress.Utils.PointFloat(677F, 423F);
            this.xrLabelVisaType.Name = "xrLabelVisaType";
            this.xrLabelVisaType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelVisaType.SizeF = new System.Drawing.SizeF(254F, 64F);
            this.xrLabelVisaType.Text = "xrLabelVisaType";
            // 
            // xrLabelVID
            // 
            this.xrLabelVID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.VID")});
            this.xrLabelVID.Dpi = 254F;
            this.xrLabelVID.LocationFloat = new DevExpress.Utils.PointFloat(635F, 275F);
            this.xrLabelVID.Multiline = true;
            this.xrLabelVID.Name = "xrLabelVID";
            this.xrLabelVID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelVID.SizeF = new System.Drawing.SizeF(572F, 106F);
            this.xrLabelVID.Text = "xrLabelVID";
            // 
            // xrLabelPID
            // 
            this.xrLabelPID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SendDtl.PID")});
            this.xrLabelPID.Dpi = 254F;
            this.xrLabelPID.LocationFloat = new DevExpress.Utils.PointFloat(698F, 190F);
            this.xrLabelPID.Name = "xrLabelPID";
            this.xrLabelPID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelPID.SizeF = new System.Drawing.SizeF(381F, 64F);
            this.xrLabelPID.Text = "xrLabelPID";
            // 
            // xrLabelMName
            // 
            this.xrLabelMName.Dpi = 254F;
            this.xrLabelMName.LocationFloat = new DevExpress.Utils.PointFloat(85F, 423F);
            this.xrLabelMName.Name = "xrLabelMName";
            this.xrLabelMName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelMName.SizeF = new System.Drawing.SizeF(487F, 64F);
            this.xrLabelMName.Text = "xrLabelMName";
            // 
            // xrLabelGName
            // 
            this.xrLabelGName.Dpi = 254F;
            this.xrLabelGName.LocationFloat = new DevExpress.Utils.PointFloat(85F, 318F);
            this.xrLabelGName.Name = "xrLabelGName";
            this.xrLabelGName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelGName.SizeF = new System.Drawing.SizeF(487F, 64F);
            this.xrLabelGName.Text = "xrLabelGName";
            // 
            // xrLabelSName
            // 
            this.xrLabelSName.Dpi = 254F;
            this.xrLabelSName.LocationFloat = new DevExpress.Utils.PointFloat(85F, 212F);
            this.xrLabelSName.Name = "xrLabelSName";
            this.xrLabelSName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelSName.SizeF = new System.Drawing.SizeF(487F, 64F);
            this.xrLabelSName.Text = "xrLabelSName";
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Dpi = 254F;
            this.topMarginBand1.HeightF = 0F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Dpi = 254F;
            this.bottomMarginBand1.HeightF = 0F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ReportQZ1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.topMarginBand1,
            this.bottomMarginBand1});
            this.Dpi = 254F;
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 2101;
            this.PageWidth = 2969;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.Version = "13.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReportQZ1_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRLabel xrLabelPID;
        public DevExpress.XtraReports.UI.XRLabel xrLabelMName;
        public DevExpress.XtraReports.UI.XRLabel xrLabelGName;
        public DevExpress.XtraReports.UI.XRLabel xrLabelSName;
        public DevExpress.XtraReports.UI.XRLabel xrLabelVID;
        public DevExpress.XtraReports.UI.XRLabel xrLabelIssueDate;
        public DevExpress.XtraReports.UI.XRLabel xrLabelVisaType;
        public DevExpress.XtraReports.UI.XRLabel xrLabelExpirationDate;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBirthDay;
        public DevExpress.XtraReports.UI.XRLabel xrLabelSex;
        public DevExpress.XtraReports.UI.XRLabel xrLabelTimes;
        public DevExpress.XtraReports.UI.XRLabel xrLabelRemarks;
        public DevExpress.XtraReports.UI.XRLabel xrLabelNation;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
    }
}
