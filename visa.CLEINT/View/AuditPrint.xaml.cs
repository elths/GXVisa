using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using getIcao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
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
using visa.CLEINT.Model;
using visa.MODEL;

namespace visa.CLEINT.View
{
    /// <summary>
    /// AuditPrint.xaml 的交互逻辑
    /// </summary>
    public partial class AuditPrint : UserControl, INotifyPropertyChanged
    {

        visaEntities visaORM = new visaEntities();
        Report.ReportQZ report = new Report.ReportQZ();

        public event PropertyChangedEventHandler PropertyChanged;

        string QZIDnum = "";
        string QZIDstring = "";

        public ObservableCollection<sp_SelectPrintSendInfo_Result> sendCollection { get; set; }

        public sp_SelectPrintSendInfo_Result CurrentItem { get; set; }

        private ViewModel.ViewModel _viewModel;

        public ViewModel.ViewModel viewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnPropertyChanged("viewModel");
            }
        }

        /// <summary>
        /// 设置打印预览页是根据审批号显示，还是根据护照号显示
        /// </summary>
        private int selectPrintSendInfopara = 1;

        public AuditPrint()
        {
            InitializeComponent();

            sendCollection = new ObservableCollection<sp_SelectPrintSendInfo_Result>();
            InitData();
            InitializeViewModel();
            DataContext = this;
        }

        void InitData()
        {
            InitNotPrintSource(false);
            cbAuditPerson.ItemsSource = visaORM.TB_AuditUser.Where(a => a.FStatus == true).OrderBy(a => a.FOrder).Select(a => a.FName).Distinct();
            cbAuditPosition.ItemsSource = visaORM.TB_AuditPosition.Where(a => a.FStatus == true).OrderBy(a => a.FOrder).Select(a => a.FName).Distinct();
        }

        /// <summary>
        /// 初始化未打印列表
        /// </summary>
        /// <param name="isPrint"></param>
        void InitNotPrintSource(bool isPrint)
        {

            List<string> notPrintObjs;
            if (!isPrint)
                notPrintObjs = visaORM.SendInfo.Where(s => s.FSysPrint == false && s.FSysSend == true).OrderByDescending(s => s.FCreateDate).Select(s => s.FSendNo).ToList();
            else
                notPrintObjs = visaORM.SendInfo.Where(s => s.FSysSend == true).OrderByDescending(s => s.FCreateDate).Select(s => s.FSendNo).ToList();

            List<string> notPrintList = new List<string>();
            foreach (string notPrintObj in notPrintObjs.ToList().Distinct())
            {
                notPrintList.Add(notPrintObj);
            }
            cbNotPrint.ItemsSource = notPrintList;
        }


        private void InitializeViewModel()
        {
            viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);
            MyCommand printCommand = new MyCommand(printVisa)
            {
                Caption = "打印",
                LargeGlyph = null,
                SmallGlyph = null
            };
            MyCommand setCommand = new MyCommand(setQZID)
            {
                Caption = "设置签证号",
                LargeGlyph = null,
                SmallGlyph = null
            };
            MyCommand saveQZIDCommand = new MyCommand(saveQZID)
            {
                Caption = "保存签证号码",
                LargeGlyph = null,
                SmallGlyph = null
            };
            MyCommand printTable4Command = new MyCommand(printTable4)
            {
                Caption = "打印表四",
                LargeGlyph = null,
                SmallGlyph = null
            };
            MyCommand showAllSendNoCommand = new MyCommand(showAllSendNo)
            {
                Caption = "显示所有签证",
                LargeGlyph = null,
                SmallGlyph = null
            };


            clipboardBar.Commands.Add(printCommand);
            clipboardBar.Commands.Add(setCommand);
            clipboardBar.Commands.Add(saveQZIDCommand);
            clipboardBar.Commands.Add(printTable4Command);
            clipboardBar.Commands.Add(showAllSendNoCommand);

        }

        void showAllSendNo()
        {
            InitNotPrintSource(true);
        }

        /// <summary>
        /// 打印表四
        /// </summary>
        void printTable4()
        {
            if (cbNotPrint.SelectedItem == null)
            {
                MessageBox.Show("请先选择一个名单号");
            }
            else
            {
                SaveSysPrint();
                string SentNo = cbNotPrint.SelectedItem.ToString();

                SetGridSource(SentNo);

                printMethod(cbNotPrint.SelectedItem.ToString());
                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report4(cbNotPrint.SelectedItem.ToString()));
                //cbNotPrint.ItemsSource = visaORM.SendInfo.Where(s => s.FSysPrint == false && s.FSysSend == true).Select(s => s.FSendNo).Distinct();
                //cbNotPrint.EditValue = "";
            }
        }

        /// <summary>
        /// 保存为已打印
        /// </summary>
        private void SaveSysPrint()
        {
            foreach (var SendModel in sendCollection.Where(s => s.FSelectItem == true))
            {
                SendInfo sObj = visaORM.SendInfo.FirstOrDefault(s => s.FQZID == SendModel.FQZID);
                if (sObj != null)
                {
                    if (sObj.FSysPrintNum == null)
                        sObj.FSysPrintNum = 1;
                    else
                        sObj.FSysPrintNum = sObj.FSysPrintNum + 1;

                    sObj.FSysPrint = true;
                    sObj.FSysPrintDate = DateTime.Now;
                    sObj.FSysPrintUser = MainContext.UserID;
                    visaORM.ObjectStateManager.ChangeObjectState(sObj, System.Data.EntityState.Modified);

                    Customer cObj = visaORM.Customer.FirstOrDefault(c => c.FID == sObj.FCustomerID);
                    if (cObj != null)
                    {
                        cObj.FSysPrint = true;
                        cObj.FSysPrintDate = DateTime.Now;
                        cObj.FSysPutUser = MainContext.UserID;
                        visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);
                    }
                }
            }
            visaORM.SaveChanges();

        }

        void printMethod(string SendNo)
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


            report.xrLabelCaption.Text = "表格四";
            DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
            pt.Print();

        }

        void printVisa()
        {
            MainTab.SelectedIndex = 1;

        }

        /// <summary>
        /// 保存签证号
        /// </summary>
        void saveQZID()
        {
            try
            {
                this.MainBar.Focus();

                if (sendCollection.Count() == 0)
                {
                    MessageBox.Show("没有签证");
                    return;
                }

                foreach (var SendModel in sendCollection)
                {
                    if (string.IsNullOrEmpty(SendModel.FQZID))
                    {
                        if (QZIDnum == "" || QZIDstring == "")
                        {
                            MessageBox.Show("请先设置起始签证号码");
                            return;
                        }
                    }
                }

                string QZID = "";

                SendInfo sObj = new SendInfo();
                Customer cObj = new Customer();
                QZKC QZNOObj = new QZKC();

                int i = 0;
                if (!string.IsNullOrEmpty(QZIDnum))
                    i = Convert.ToInt32(QZIDnum);

                foreach (var SendModel in sendCollection)
                {
                    cObj = visaORM.Customer.FirstOrDefault(c => c.FID == SendModel.QID);
                    if (cObj != null)
                    {
                        if (string.IsNullOrEmpty(cObj.FQZID)) //如果没有签证号，就保存签证号
                        {

                            if (!string.IsNullOrEmpty(SendModel.FQZID))
                                QZID = SendModel.FQZID;
                            else
                                QZID = GetVisaNumberString(QZIDstring, i.ToString().Length, ref i);

                            QZNOObj = visaORM.QZKC.FirstOrDefault(q => q.FIsUse == false && q.FQZID == QZID);
                            if (QZNOObj == null)
                            {
                                MessageBox.Show(string.Format("签证号码{0}已经被使用或不存在，请添加或使用其他起始号码", QZID));
                                return;
                            }
                            cObj.FQZID = QZNOObj.FQZID;
                            QZNOObj.FIsUse = true;

                            sObj = visaORM.SendInfo.FirstOrDefault(s => s.FCustomerID == cObj.FID);

                            if (sObj != null) sObj.FQZID = QZNOObj.FQZID;

                            visaORM.ObjectStateManager.ChangeObjectState(sObj, System.Data.EntityState.Modified);
                            visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);
                            visaORM.ObjectStateManager.ChangeObjectState(QZNOObj, System.Data.EntityState.Modified);
                            visaORM.SaveChanges();
                            i++;
                        }
                        else //如果已有签证号，提示是否覆盖
                        {
                            if (
                                System.Windows.MessageBox.Show(
                                    string.Format("护照号为{0}已存在签证号(可能是上一次录入或者本次手动录入)，是否覆盖当前号码？", SendModel.PID), "已存在签证号",
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                //更新表一的签证号码
                                cObj.FQZID = SendModel.FQZID;

                                //更新签证号码表中的使用情况
                                QZNOObj = visaORM.QZKC.FirstOrDefault(q => q.FIsUse == false && q.FQZID == SendModel.FQZID);
                                if (QZNOObj == null)
                                {
                                    MessageBox.Show(string.Format("签证号码{0}已经被使用或不存在，请添加或使用其他起始号码", SendModel.FQZID));
                                    return;
                                }
                                cObj.FQZID = QZNOObj.FQZID;
                                QZNOObj.FIsUse = true;

                                //更新表三的签证号码
                                sObj = visaORM.SendInfo.FirstOrDefault(s => s.FCustomerID == cObj.FID);
                                if (sObj != null)
                                {
                                    sObj.FQZID = QZNOObj.FQZID;
                                    visaORM.ObjectStateManager.ChangeObjectState(sObj, System.Data.EntityState.Modified);
                                }
                                visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);
                                visaORM.ObjectStateManager.ChangeObjectState(QZNOObj, System.Data.EntityState.Modified);
                                visaORM.SaveChanges();
                            }
                        }

                    }

                }

                MessageBox.Show("保存成功");
                if (System.Windows.MessageBox.Show("保存签证成功，是否继续打印表四", "保存成功", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    printTable4();
                }
                //保存时设置为保存为已打印
                SaveSysPrint();
                //初始化未打印的名单列表
                InitNotPrintSource(false);
                sendCollection.Clear();
                cbNotPrint.EditValue = "";

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }

        private static string GetVisaNumberString(string FirstLetter, int NumberLenght, ref int i)
        {
            string finalQZID = FirstLetter + ("0000000" + i.ToString()).Substring(NumberLenght);
            visaEntities visaORM = new visaEntities();
            var isUsedObj = visaORM.QZKC.FirstOrDefault(q => q.FIsUse == true && q.FQZID == finalQZID);
            if (isUsedObj != null)
            {
                i++;
                return GetVisaNumberString(FirstLetter, NumberLenght, ref i);
            }

            return finalQZID;
        }


        void setQZID()
        {
            try
            {

                View.SetBeginQZID BeginQZIDObj = new View.SetBeginQZID();
                DXDialog dg = new DXDialog()
                {
                    Content = BeginQZIDObj,
                    Title = "设置签证起始号码"
                };
                dg.Width = 400;
                dg.Height = 200;
                dg.HorizontalAlignment = HorizontalAlignment.Center;
                dg.VerticalAlignment = VerticalAlignment.Center;
                dg.ShowDialog(MessageBoxButton.OKCancel);
                QZIDnum = BeginQZIDObj.SetQZIDnum;
                QZIDstring = BeginQZIDObj.SetQZIDstring;

                this.CurrentItem = null;
                string QZID = "";
                int i = Convert.ToInt32(QZIDnum);

                foreach (var sendModel in sendCollection.Where(s => s.FSelectItem == true))
                {
                    QZID = GetVisaNumberString(QZIDstring, i.ToString().Length, ref i);
                    sendModel.FQZID = QZID.ToUpper();
                    i++;
                }
                //for (int j = 0; j < this.gridMain.VisibleRowCount; j++)
                //{
                //    QZID = GetVisaNumberString(QZIDstring, i.ToString().Length,ref i);
                //    gridMain.SetCellValue(j,"FQZID",QZID);
                //    i++;
                //}
                //(this.gridMain.CurrentItem as sp_SelectPrintSendInfo_Result).FQZID = GetVisaNumberString(QZIDstring, i.ToString().Length, i);
                //foreach (var sendModel in sendCollection)
                //{
                //    QZID = GetVisaNumberString(QZIDstring, i.ToString().Length, i);
                //    sendModel.FQZID = QZID;
                //    i++;
                //}
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }


            //gridMain.ItemsSource = sendCollection;



        }


        private void cbNotPrint_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbNotPrint.SelectedItem != null)
                {
                    string SentNo = cbNotPrint.SelectedItem.ToString();

                    SetGridSource(SentNo);

                    this.txtSendNO.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        /// <summary>
        /// 绑定主表格的签证数据
        /// </summary>
        /// <param name="sendNoString"></param>
        private void SetGridSource(string SentNo)
        {
            var model = visaORM.SendInfo.FirstOrDefault(s => s.FSendNo == SentNo);
            if (model != null)
            {


                //var sendCollection = from c in visaORM.Customer
                //                     join s in visaORM.SendInfo
                //                     on c.FID equals s.FCustomerID
                //                     where s.FSendNo == SentNo
                //                     select c;
                sendCollection.Clear();
                foreach (var sendInfoModel in visaORM.sp_SelectPrintSendInfo(SentNo).ToList())
                {
                    sendInfoModel.PropertyChanged += sendInfoModel_PropertyChanged;
                    sendCollection.Add(sendInfoModel);
                }
                //gridMain.ItemsSource = sendCollection;
                //ToDataTable(sendCollection);
            }
            else
            {
                MessageBox.Show("找不到对应单号。");
            }
        }

        void sendInfoModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "FQZID" && this.CurrentItem != null && !string.IsNullOrEmpty(this.CurrentItem.FQZID))
                {
                    //判断当前签证号是否已经在集合中
                    if (sendCollection.FirstOrDefault(s => s.FQZID == CurrentItem.FQZID && s.QID != CurrentItem.QID) != null)
                    {
                        DXMessageBox.Show("该签证号已经在本名单中被使用，修改无效。");
                        CurrentItem.FQZID = "";
                        return;
                    }

                    SendInfo sendModel = visaORM.SendInfo.FirstOrDefault(c => c.FCustomerID == CurrentItem.QID);

                    if (sendModel.FQZID == CurrentItem.FQZID)
                    {
                        return;
                    }

                    if (sendModel == null)
                    {
                        DXMessageBox.Show("该签证已经被删除，修改无效。");
                        CurrentItem.FQZID = sendModel.FQZID;
                        return;
                    }


                    Customer custModel = visaORM.Customer.FirstOrDefault(c => c.FID == CurrentItem.QID);
                    //if (custModel == null)
                    //{
                    //    DXMessageBox.Show("还签证已经被删除，修改无效。");
                    //    return;
                    //}

                    QZKC qzkc = visaORM.QZKC.FirstOrDefault(q => q.FQZID == CurrentItem.FQZID);
                    if (qzkc == null)
                    {
                        DXMessageBox.Show("还没有该签证号，请联系管理员设置，或使用其他号码。");
                        CurrentItem.FQZID = sendModel.FQZID;
                        return;
                    }
                    if (qzkc.FIsUse == true)
                    {
                        DXMessageBox.Show("改签证号码已经被使用，使用其他号码。");
                        CurrentItem.FQZID = sendModel.FQZID;
                        return;
                    }

                    if (!object.Equals(CurrentItem.FQZID, CurrentItem.FQZID.ToUpper()))
                    {
                        CurrentItem.FQZID = CurrentItem.FQZID.ToUpper();
                        return;
                    }

                    //QZKC qzkcOri = visaORM.QZKC.FirstOrDefault(q => q.FQZID == custModel.FQZID);
                    ////将原签证号释放
                    //if (qzkcOri != null)
                    //{
                    //    qzkcOri.FIsUse = false;
                    //    visaORM.ObjectStateManager.ChangeObjectState(qzkcOri, System.Data.EntityState.Modified);
                    //    visaORM.SaveChanges();
                    //}

                    //QZKC qzkcNew = visaORM.QZKC.FirstOrDefault(q => q.FQZID == CurrentItem.FQZID);
                    ////将新签证号置为已使用
                    //if (qzkcNew != null)
                    //{
                    //    qzkcNew.FIsUse = true;
                    //    visaORM.ObjectStateManager.ChangeObjectState(qzkcNew, System.Data.EntityState.Modified);
                    //    visaORM.SaveChanges();
                    //}

                    //sendModel.FQZID = CurrentItem.FQZID;
                    //visaORM.ObjectStateManager.ChangeObjectState(sendModel, System.Data.EntityState.Modified);

                    ////修改当前签证的签证号
                    //if (custModel != null)
                    //{
                    //    custModel.FQZID = CurrentItem.FQZID;
                    //    visaORM.ObjectStateManager.ChangeObjectState(custModel, System.Data.EntityState.Modified);
                    //}
                    //visaORM.SaveChanges();
                    //DXMessageBox.Show("修改签证号成功。");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }
        }


        private void cbAuditPosition_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (cbAuditPosition.SelectedItem != null)
            //{
            //    string selectedAuditPosition = cbAuditPosition.SelectedItem.ToString();
            //    var AuditModel = visaORM.TB_AuditUser.FirstOrDefault(a=>a.FPosition==selectedAuditPosition);
            //    cbAuditPerson.SelectedItem = AuditModel.FName;
            //}
        }

        private void cbAuditPerson_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (cbAuditPerson.SelectedItem != null)
            //{
            //    string selectedAuditPerson = cbAuditPerson.SelectedItem.ToString();
            //    var AuditModel = visaORM.TB_AuditUser.FirstOrDefault(a => a.FName == selectedAuditPerson);
            //    cbAuditPosition.SelectedItem = AuditModel.FPosition;
            //}

        }

        /// <summary>
        /// 切换到打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                if (MainTab.SelectedIndex == 1)
                {

                    if (cbAuditPerson.SelectedItem == null || cbAuditPosition.SelectedItem == null)
                    {
                        MainTab.SelectedIndex = 0;
                        this.selectPrintSendInfopara = 1;
                        MessageBox.Show("请选择审批人姓名和职位");
                        return;
                    }

                    if (this.selectPrintSendInfopara == 1)
                    {
                        if (string.IsNullOrEmpty(this.txtSendNO.Text) && cbNotPrint.SelectedItem == null)
                        {
                            MessageBox.Show("请选择需要打印的签证名单");
                            MainTab.SelectedIndex = 0;
                            this.selectPrintSendInfopara = 1;
                            return;
                        }
                        foreach (var sendObj in sendCollection.Where(s => s.FSelectItem == true))
                        {
                            if (string.IsNullOrEmpty(sendObj.FQZID))
                            {
                                MessageBox.Show("请先设置签证号");
                                MainTab.SelectedIndex = 0;
                                this.selectPrintSendInfopara = 1;
                                return;
                            }
                        }
                    }
                    else
                    {
                        Customer cusModel = visaORM.Customer.OrderByDescending(c => c.FCreateDate).FirstOrDefault(c => c.FPassportNo == txtPassportNo.Text);
                        if (cusModel == null)
                        {
                            MessageBox.Show("找不到该签证");
                            MainTab.SelectedIndex = 0;
                            this.selectPrintSendInfopara = 1;
                            return;
                        }
                        if (cusModel.FSysSend == false)
                        {
                            MessageBox.Show("请先审批后才可以打印");
                            MainTab.SelectedIndex = 0;
                            this.selectPrintSendInfopara = 1;
                            return;
                        }
                    }

                    StringBuilder sb = new StringBuilder();

                    //if (gridMain.SelectedItem == null)
                    //{
                    //    MessageBox.Show("请选择需要打印的数据");
                    //    MainTab.SelectedIndex = 0;
                    //    return;
                    //}
                    //else
                    //{
                    //    cusModel = gridMain.SelectedItem as Customer;
                    //}

                    //  PrintSendInfoSP.sp_SelectPrintSendInfoDataTable table = (PrintSendInfoSP.sp_SelectPrintSendInfoDataTable)(report.DataSource);
                    // PrintSendInfoSP.sp_SelectPrintSendInfoDataTable table = ((PrintSendInfoSP)(report.DataSource)).sp_SelectPrintSendInfo;
                    int i = 1;

                    List<sp_SelectPrintSendInfo_Result> selectPrintSendInfo;
                    if (this.selectPrintSendInfopara == 1)
                    {
                        if (!string.IsNullOrEmpty(this.txtSendNO.Text))
                            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.printSendInfo_SP1.sp_SelectPrintSendInfo, this.txtSendNO.Text);
                        else
                            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.printSendInfo_SP1.sp_SelectPrintSendInfo, cbNotPrint.SelectedItem.ToString());
                        selectPrintSendInfo = sendCollection.Where(s => s.FSelectItem == true).ToList();
                    }
                    else
                    {
                        report.sp_SelectPrintSendInfoTableAdapter.Fill(report.printSendInfo_SP1.sp_SelectPrintSendInfo, txtPassportNo.Text);
                        selectPrintSendInfo = visaORM.sp_SelectPrintSendInfo(txtPassportNo.Text).ToList();
                        this.selectPrintSendInfopara = 1;
                    }


                    Data.PrintSendInfo_SP.sp_SelectPrintSendInfoDataTable table = ((Data.PrintSendInfo_SP)(report.DataSource)).sp_SelectPrintSendInfo;

                    List<Data.PrintSendInfo_SP.sp_SelectPrintSendInfoRow> RowList = new List<Data.PrintSendInfo_SP.sp_SelectPrintSendInfoRow>();
                    report.printSendInfo_SP1.Clear();


                    foreach (sp_SelectPrintSendInfo_Result r in selectPrintSendInfo)
                    {
                        //PrintSendInfoSP.sp_SelectPrintSendInfoRow tempRow = row;

                        //if (string.IsNullOrEmpty(r.FQZID))
                        //{
                        //    MessageBox.Show("请先生成签证号");
                        //    MainTab.SelectedIndex = 0;
                        //    this.selectPrintSendInfopara = 1;
                        //    return;
                        //}

                        var icao = new GenIcao();

                        string countryShortName = "CHN";
                        var CountryShortObj = from c in visaORM.Customer.Where(custom => custom.FID == r.QID)
                                              join co in visaORM.TCountry on c.FBirthNationlityPresent equals co.FCountry
                                              select co;
                        if (CountryShortObj != null)
                        {
                            TCountry CountryShort = CountryShortObj.FirstOrDefault();
                            if (CountryShort != null)
                                countryShortName = CountryShort.Fcountryshort;
                        }


                        icao.encodeVisa("V", "VNM", countryShortName, r.Name, r.Sex, Convert.ToDateTime(r.BirthDay).ToString("dd/MM/yyyy"), r.PID, r.FQZID, (Convert.ToDateTime(r.PDate)).ToString("dd/MM/yyyy"));
                        string icaoLine1 = icao.getUpperLine();
                        string icaoLine2 = icao.getLowerLine();
                        string VIDString = icaoLine1 + "\n" + icaoLine2;
                        string timesDetail = "";
                        if (r.FTimes == 1) timesDetail = @"Một lần/Single";
                        else timesDetail = @"Nhiều lần/Multiple";

                        table.Addsp_SelectPrintSendInfoRow(true, i, r.Name, Convert.ToDateTime(r.BirthDay).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")), (DateTime)r.FBirthDay, r.Sex, r.FBirthNationlityEn, r.FBirthNationlityPresentEn, r.PID, r.Work, r.CQJDate, timesDetail, (int)r.Days, (int)r.FTimes, (bool)r.FSysPut, (bool)r.FSysSend, r.FQZID, r.FDSN, VIDString, r.FCreateCompany, r.PrintCount, Convert.ToDateTime(r.DDate).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")), Convert.ToDateTime(r.PDate).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")), r.CType, Convert.ToDateTime(r.MDate).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")), r.IDOrder, r.FSysMemo);
                        i++;

                        //sb.Clear();
                        //sb.Append("签证人：" + r.Name + "\r\n");
                        //sb.Append("记录时间：" + DateTime.Now + "\r\n");
                        //sb.Append("生日：" + Convert.ToDateTime(r.BirthDay).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")) + "\r\n");
                        //sb.Append("出境时间：" + Convert.ToDateTime(r.DDate).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")) + "\r\n");
                        //sb.Append("回归时间：" + Convert.ToDateTime(r.PDate).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US")) + "\r\n");
                        //sb.Append("签证码：" + VIDString + "\r\n");
                        //Log.WriteLog.WriteOperateLog(sb.ToString());

                    }

                    //foreach (PrintSendInfoSP.sp_SelectPrintSendInfoRow tempRow2 in RowList)
                    //{
                    //    table.Addsp_SelectPrintSendInfoRow(tempRow2);
                    //}


                    //sb.Clear();
                    //sb.Append("记录时间：" + DateTime.Now + "\r\n");
                    //sb.Append("审批人：" + cbAuditPerson.SelectedItem.ToString() + "\r\n");
                    //sb.Append("审批职位：" + cbAuditPosition.SelectedItem.ToString() + "\r\n");
                    //Log.WriteLog.WriteOperateLog(sb.ToString());


                    report.xrLabelVID.Width = 950;

                    report.xrLabelMemo.Text = txtMemo.Text;
                    report.xrLabelUserName.Text = cbAuditPerson.SelectedItem.ToString();
                    report.xrLabelDuty.Text = cbAuditPosition.SelectedItem.ToString();
                    if (!string.IsNullOrEmpty(datePassportMakeDay.Text))
                        report.xrLabelMDate.Text = Convert.ToDateTime(datePassportMakeDay.Text).ToString("dd MMM yyyy", new System.Globalization.CultureInfo("en-US"));
                    else
                        report.xrLabelMDate.Text = "";
                    report.xrLabelVID.Visible = false; //设置签证码为不可见
                    //设置位置
                    SetControlPositionInReport(report, 1);
                    MainPreview.Model = new XtraReportPreviewModel(report);
                    report.CreateDocument();
                }

            }
            catch (System.Exception ex)
            {
                Log.WriteLog.WriteErorrLog(ex);
                MessageBox.Show(ex.ToString());
            }


        }

        private void CheckEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            report.xrLabelVID.Visible = (bool)cbShowVisaCode.IsChecked;
            MainPreview.Model = new XtraReportPreviewModel(report);
            report.CreateDocument();

        }


        private void SetControlPositionInReport(Report.ReportQZ report, int groupID)
        {
            report.xrLabelMemo.Location = GetLabelPoint("Memo", groupID);
            report.xrLabelPDate.Location = GetLabelPoint("PDate", groupID);
            report.xrLabelDDate.Location = GetLabelPoint("DDate", groupID);
            report.xrLabelUserName.Location = GetLabelPoint("UserName", groupID);
            report.xrLabelNamNimh.Location = GetLabelPoint("NamNimh", groupID);
            report.xrLabelPID.Location = GetLabelPoint("PID", groupID);
            report.xrLabelMDate.Location = GetLabelPoint("MDate", groupID);
            report.xrLabelType.Location = GetLabelPoint("Type", groupID);
            report.xrLabelTimes.Location = GetLabelPoint("Times", groupID);
            report.xrLabelDuty.Location = GetLabelPoint("Duty", groupID);
            report.xrLabelVID.Location = GetLabelPoint("VID", groupID);

            report.xrLabelBirthday.Visible = true;
            report.xrLabelBirthday.Location = GetLabelPoint("BDate", groupID);
            if (report.xrLabelBirthday.Location.X == 0 && report.xrLabelBirthday.Location.Y == 0)
                report.xrLabelBirthday.Visible = false;

            report.xrCustomerName.Visible = true;
            report.xrCustomerName.Location = GetLabelPoint("CustomerName", groupID);
            if (report.xrCustomerName.Location.X == 0 && report.xrCustomerName.Location.Y == 0)
                report.xrCustomerName.Visible = false;

            report.xrCountry.Visible = true;
            report.xrCountry.Location = GetLabelPoint("Country", groupID);
            if (report.xrCountry.Location.X == 0 && report.xrCountry.Location.Y == 0)
                report.xrCountry.Visible = false;


            visaEntities DataContextP = new visaEntities();

            TB_Position pageSize = DataContextP.TB_Position.FirstOrDefault(p => p.FName == "PageSize" && p.FGroupID == groupID);
            if (pageSize != null)
            {
                report.PageWidth = Convert.ToInt32(pageSize.FX);
                report.PageHeight = Convert.ToInt32(pageSize.FY);
            }
            else
            {
                report.PageWidth = 3000;
                report.PageHeight = 2100;
            }

            if (groupID == 1)
                report.Font = new System.Drawing.Font("Times New Roman", 10);
            else
                report.Font = new System.Drawing.Font("Times New Roman", 12);

        }


        System.Drawing.Point GetLabelPoint(string LabelName, int GroupID)
        {
            int X;
            int Y;
            visaEntities visaORM = new visaEntities();

            TB_Position pModel = visaORM.TB_Position.OrderByDescending(p => p.FCreateDate).FirstOrDefault(p => p.FName == LabelName && p.FGroupID == GroupID);
            if (pModel == null)
            {
                MessageBox.Show(LabelName + "未设置位置数值");
                return new System.Drawing.Point(0, 0);
            }
            else
            {
                X = (int)pModel.FX;
                Y = (int)pModel.FY;
                return new System.Drawing.Point(X, Y);
            }

        }



        public DataTable ToDataTable(IEnumerable list)
        {
            DataTable dt = new DataTable();
            bool schemaIsBuild = false;
            PropertyInfo[] props = null;

            foreach (object item in list)
            {
                if (!schemaIsBuild)
                {

                    props = item.GetType().GetProperties();


                    foreach (var pi in props)
                    {

                        Type columnType = pi.PropertyType;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                            columnType = pi.PropertyType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(pi.Name, columnType));
                    }

                    schemaIsBuild = true;
                }

                var row = dt.NewRow();
                foreach (var pi in props)
                {
                    try
                    {
                        row[pi.Name] = pi.GetValue(item, null);
                    }
                    catch { }
                }

                dt.Rows.Add(row);
            }

            dt.AcceptChanges();
            return dt;
        }

        private void txtPassportNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                (App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(this.txtPassportNo.Text));
            }
        }

        private void btnStyle1_Click(object sender, RoutedEventArgs e)
        {


            DXDialog dg = new DXDialog()
            {
                Content = new View.SetPrintStyle(1),
                Title = "设置打印位置风格一"
            };
            dg.Width = 300;
            dg.Height = 650;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
            SetControlPositionInReport(report, 1);
            MainPreview.Model = new XtraReportPreviewModel(report);
            report.CreateDocument();


        }

        private void btnStyle2_Click(object sender, RoutedEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.SetPrintStyle(2),
                Title = "设置打印位置风格二"
            };
            dg.Width = 300;
            dg.Height = 650;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
            SetControlPositionInReport(report, 2);
            MainPreview.Model = new XtraReportPreviewModel(report);
            report.CreateDocument();

        }

        private void rbPosition1_Click(object sender, RoutedEventArgs e)
        {
            SetControlPositionInReport(report, 1);
            MainPreview.Model = new XtraReportPreviewModel(report);
            report.CreateDocument();

        }

        private void rbPosition2_Click(object sender, RoutedEventArgs e)
        {
            SetControlPositionInReport(report, 2);
            MainPreview.Model = new XtraReportPreviewModel(report);
            report.CreateDocument();

        }

        /// <summary>
        /// 单个补打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassportNo_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(txtPassportNo.Text));
                //if (string.IsNullOrEmpty(txtPassportNo.Text))
                //    return;
                //this.selectPrintSendInfopara = 2;
                //MainTab.SelectedIndex = 1;
                if (e.Key == Key.Enter)
                {
                    sendCollection.Clear();
                    foreach (var sendInfoModel in visaORM.sp_SelectPrintSendInfo(txtPassportNo.Text).ToList())
                    {
                        sendInfoModel.PropertyChanged += sendInfoModel_PropertyChanged;
                        sendCollection.Add(sendInfoModel);
                    }
                }
            }

        }

        /// <summary>
        /// 批量补打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSendNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetGridSource(txtSendNO.Text);
                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report4(txtSendNO.Text));
            }

        }



        virtual internal protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 表格中保存图片事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentItem.FQZID))
            {
                MessageBox.Show("请先设置签证号");
                return;
            }

            try
            {
                Customer cObj = visaORM.Customer.FirstOrDefault(c => c.FID == CurrentItem.QID);
                if (cObj != null)
                {
                    if (!string.IsNullOrEmpty(cObj.FQZID)) //如果已有签证号，提示是否覆盖
                    {
                        if (
                            System.Windows.MessageBox.Show(
                                string.Format("护照号为{0}已存在签证号，是否覆盖当前号码？", CurrentItem.PID), "已存在签证号",
                                MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return; ;
                    }

                    //更新表一的签证号码
                    cObj.FQZID = CurrentItem.FQZID;

                    //更新签证号码表中的使用情况
                    QZKC QZNOObj = visaORM.QZKC.FirstOrDefault(q => q.FIsUse == false && q.FQZID == CurrentItem.FQZID);
                    if (QZNOObj == null)
                    {
                        MessageBox.Show(string.Format("签证号码{0}已经被使用或不存在，请添加或使用其他起始号码", CurrentItem.FQZID));
                        return;
                    }
                    cObj.FQZID = QZNOObj.FQZID;
                    QZNOObj.FIsUse = true;

                    //更新表三的签证号码
                    SendInfo sObj = visaORM.SendInfo.FirstOrDefault(s => s.FCustomerID == cObj.FID);
                    if (sObj != null)
                    {
                        sObj.FQZID = QZNOObj.FQZID;
                        visaORM.ObjectStateManager.ChangeObjectState(sObj, System.Data.EntityState.Modified);
                    }
                    visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);
                    visaORM.ObjectStateManager.ChangeObjectState(QZNOObj, System.Data.EntityState.Modified);
                    visaORM.SaveChanges();
                    MessageBox.Show("保存成功");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }


        }
    }
}
