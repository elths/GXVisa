namespace visa.CLEINT.Report
{
    partial class ReportQZ
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
            this.xrCountry = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCustomerName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelBirthday = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelUserName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelMDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelMemo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelTimes = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelNamNimh = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelDuty = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelPID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelVID = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelType = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelPDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelDDate = new DevExpress.XtraReports.UI.XRLabel();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.printSendInfo_SP1 = new visa.CLEINT.Data.PrintSendInfo_SP();
            this.sp_SelectPrintSendInfoTableAdapter = new visa.CLEINT.Data.PrintSendInfo_SPTableAdapters.sp_SelectPrintSendInfoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.printSendInfo_SP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCountry,
            this.xrCustomerName,
            this.xrLabelBirthday,
            this.xrLabelUserName,
            this.xrLabelMDate,
            this.xrLabelMemo,
            this.xrLabelTimes,
            this.xrLabelNamNimh,
            this.xrLabelDuty,
            this.xrLabelPID,
            this.xrLabelVID,
            this.xrLabelType,
            this.xrLabelPDate,
            this.xrLabelDDate});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 800F;
            this.Detail.KeepTogether = true;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrCountry
            // 
            this.xrCountry.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.FBirthNationlityPresentEn")});
            this.xrCountry.Dpi = 254F;
            this.xrCountry.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCountry.LocationFloat = new DevExpress.Utils.PointFloat(770F, 649.4375F);
            this.xrCountry.Name = "xrCountry";
            this.xrCountry.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrCountry.SizeF = new System.Drawing.SizeF(254F, 42F);
            this.xrCountry.StylePriority.UseFont = false;
            this.xrCountry.Text = "xrCountry";
            // 
            // xrCustomerName
            // 
            this.xrCustomerName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.Name")});
            this.xrCustomerName.Dpi = 254F;
            this.xrCustomerName.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCustomerName.LocationFloat = new DevExpress.Utils.PointFloat(148F, 380F);
            this.xrCustomerName.Name = "xrCustomerName";
            this.xrCustomerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrCustomerName.SizeF = new System.Drawing.SizeF(412.7499F, 42F);
            this.xrCustomerName.StylePriority.UseFont = false;
            this.xrCustomerName.Text = "xrCustomerName";
            // 
            // xrLabelBirthday
            // 
            this.xrLabelBirthday.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.BirthDay")});
            this.xrLabelBirthday.Dpi = 254F;
            this.xrLabelBirthday.LocationFloat = new DevExpress.Utils.PointFloat(635F, 562.5625F);
            this.xrLabelBirthday.Name = "xrLabelBirthday";
            this.xrLabelBirthday.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBirthday.SizeF = new System.Drawing.SizeF(254F, 42F);
            this.xrLabelBirthday.Text = "xrLabelBirthday";
            // 
            // xrLabelUserName
            // 
            this.xrLabelUserName.Dpi = 254F;
            this.xrLabelUserName.LocationFloat = new DevExpress.Utils.PointFloat(149F, 587.3958F);
            this.xrLabelUserName.Multiline = true;
            this.xrLabelUserName.Name = "xrLabelUserName";
            this.xrLabelUserName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelUserName.SizeF = new System.Drawing.SizeF(381F, 85F);
            this.xrLabelUserName.StylePriority.UseTextAlignment = false;
            this.xrLabelUserName.Text = "xrLabelUserName";
            this.xrLabelUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelMDate
            // 
            this.xrLabelMDate.Dpi = 254F;
            this.xrLabelMDate.LocationFloat = new DevExpress.Utils.PointFloat(148F, 279.4583F);
            this.xrLabelMDate.Name = "xrLabelMDate";
            this.xrLabelMDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelMDate.SizeF = new System.Drawing.SizeF(320.1458F, 42F);
            this.xrLabelMDate.Text = "xrLabelMDate";
            // 
            // xrLabelMemo
            // 
            this.xrLabelMemo.Dpi = 254F;
            this.xrLabelMemo.LocationFloat = new DevExpress.Utils.PointFloat(22F, 22F);
            this.xrLabelMemo.Name = "xrLabelMemo";
            this.xrLabelMemo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelMemo.SizeF = new System.Drawing.SizeF(599F, 106F);
            this.xrLabelMemo.Text = "xrLabelMemo";
            // 
            // xrLabelTimes
            // 
            this.xrLabelTimes.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.TimesName")});
            this.xrLabelTimes.Dpi = 254F;
            this.xrLabelTimes.LocationFloat = new DevExpress.Utils.PointFloat(650.875F, 318F);
            this.xrLabelTimes.Name = "xrLabelTimes";
            this.xrLabelTimes.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelTimes.SizeF = new System.Drawing.SizeF(344.4583F, 104F);
            this.xrLabelTimes.StylePriority.UseTextAlignment = false;
            this.xrLabelTimes.Text = "xrLabelTimes";
            this.xrLabelTimes.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabelNamNimh
            // 
            this.xrLabelNamNimh.Dpi = 254F;
            this.xrLabelNamNimh.LocationFloat = new DevExpress.Utils.PointFloat(149F, 216.7917F);
            this.xrLabelNamNimh.Name = "xrLabelNamNimh";
            this.xrLabelNamNimh.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelNamNimh.SizeF = new System.Drawing.SizeF(253F, 41.99998F);
            this.xrLabelNamNimh.StylePriority.UseTextAlignment = false;
            this.xrLabelNamNimh.Text = "Nam Ninh";
            this.xrLabelNamNimh.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelDuty
            // 
            this.xrLabelDuty.Dpi = 254F;
            this.xrLabelDuty.LocationFloat = new DevExpress.Utils.PointFloat(148F, 487F);
            this.xrLabelDuty.Multiline = true;
            this.xrLabelDuty.Name = "xrLabelDuty";
            this.xrLabelDuty.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelDuty.SizeF = new System.Drawing.SizeF(381F, 85F);
            this.xrLabelDuty.StylePriority.UseTextAlignment = false;
            this.xrLabelDuty.Text = "xrLabelDuty";
            this.xrLabelDuty.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelPID
            // 
            this.xrLabelPID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.PID")});
            this.xrLabelPID.Dpi = 254F;
            this.xrLabelPID.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelPID.LocationFloat = new DevExpress.Utils.PointFloat(550F, 439.0417F);
            this.xrLabelPID.Name = "xrLabelPID";
            this.xrLabelPID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelPID.SizeF = new System.Drawing.SizeF(339F, 47F);
            this.xrLabelPID.StylePriority.UseFont = false;
            this.xrLabelPID.StylePriority.UseTextAlignment = false;
            this.xrLabelPID.Text = "xrLabelPID";
            this.xrLabelPID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelVID
            // 
            this.xrLabelVID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.FVID")});
            this.xrLabelVID.Dpi = 254F;
            this.xrLabelVID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabelVID.LocationFloat = new DevExpress.Utils.PointFloat(148F, 725.25F);
            this.xrLabelVID.Name = "xrLabelVID";
            this.xrLabelVID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelVID.SizeF = new System.Drawing.SizeF(614F, 42F);
            this.xrLabelVID.StylePriority.UseFont = false;
            this.xrLabelVID.StylePriority.UseTextAlignment = false;
            this.xrLabelVID.Text = "xrLabelVID";
            this.xrLabelVID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelType
            // 
            this.xrLabelType.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.CType")});
            this.xrLabelType.Dpi = 254F;
            this.xrLabelType.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabelType.LocationFloat = new DevExpress.Utils.PointFloat(931F, 85F);
            this.xrLabelType.Name = "xrLabelType";
            this.xrLabelType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelType.SizeF = new System.Drawing.SizeF(93F, 42F);
            this.xrLabelType.StylePriority.UseFont = false;
            this.xrLabelType.StylePriority.UseTextAlignment = false;
            this.xrLabelType.Text = "xrLabelType";
            this.xrLabelType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelPDate
            // 
            this.xrLabelPDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.PDate")});
            this.xrLabelPDate.Dpi = 254F;
            this.xrLabelPDate.LocationFloat = new DevExpress.Utils.PointFloat(770F, 233F);
            this.xrLabelPDate.Name = "xrLabelPDate";
            this.xrLabelPDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelPDate.SizeF = new System.Drawing.SizeF(301.9583F, 41.99998F);
            this.xrLabelPDate.StylePriority.UseTextAlignment = false;
            this.xrLabelPDate.Text = "xrLabelPDate";
            this.xrLabelPDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabelDDate
            // 
            this.xrLabelDDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_SelectPrintSendInfo.DDate")});
            this.xrLabelDDate.Dpi = 254F;
            this.xrLabelDDate.LocationFloat = new DevExpress.Utils.PointFloat(466F, 233F);
            this.xrLabelDDate.Name = "xrLabelDDate";
            this.xrLabelDDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelDDate.SizeF = new System.Drawing.SizeF(296F, 41.99998F);
            this.xrLabelDDate.StylePriority.UseTextAlignment = false;
            this.xrLabelDDate.Text = "xrLabelDDate";
            this.xrLabelDDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            // printSendInfo_SP1
            // 
            this.printSendInfo_SP1.DataSetName = "PrintSendInfo_SP";
            this.printSendInfo_SP1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sp_SelectPrintSendInfoTableAdapter
            // 
            this.sp_SelectPrintSendInfoTableAdapter.ClearBeforeFill = true;
            // 
            // ReportQZ
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.topMarginBand1,
            this.bottomMarginBand1});
            this.DataAdapter = this.sp_SelectPrintSendInfoTableAdapter;
            this.DataMember = "sp_SelectPrintSendInfo";
            this.DataSource = this.printSendInfo_SP1;
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 1300;
            this.PageWidth = 2101;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.Version = "13.1";
            ((System.ComponentModel.ISupportInitialize)(this.printSendInfo_SP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRLabel xrLabelType;
        public DevExpress.XtraReports.UI.XRLabel xrLabelDDate;
        public DevExpress.XtraReports.UI.XRLabel xrLabelPDate;
        public DevExpress.XtraReports.UI.XRLabel xrLabelVID;
        public DevExpress.XtraReports.UI.XRLabel xrLabelPID;
        public DevExpress.XtraReports.UI.XRLabel xrLabelDuty;
        public DevExpress.XtraReports.UI.XRLabel xrLabelNamNimh;
        public DevExpress.XtraReports.UI.XRLabel xrLabelTimes;
        public DevExpress.XtraReports.UI.XRLabel xrLabelMemo;
        public DevExpress.XtraReports.UI.XRLabel xrLabelMDate;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        public DevExpress.XtraReports.UI.XRLabel xrLabelUserName;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBirthday;
        public Data.PrintSendInfo_SP printSendInfo_SP1;
        public Data.PrintSendInfo_SPTableAdapters.sp_SelectPrintSendInfoTableAdapter sp_SelectPrintSendInfoTableAdapter;
        public DevExpress.XtraReports.UI.XRLabel xrCustomerName;
        public DevExpress.XtraReports.UI.XRLabel xrCountry;
    }
}
