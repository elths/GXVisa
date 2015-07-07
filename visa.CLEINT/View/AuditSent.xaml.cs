using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using visa.CLEINT.Model;
using visa.MODEL;

namespace visa.CLEINT.View
{
    /// <summary>
    /// AuditSent.xaml 的交互逻辑
    /// </summary>
    public partial class AuditSent : UserControl
    {
        List<Customer> customerList = new List<Customer>();
        visaEntities visaORM = new visaEntities();

        private List<visa.CLEINT.Excel.MessageInfo> messages = new List<visa.CLEINT.Excel.MessageInfo>();

        public ObservableCollection<sp_SelectTable3ForAdd_Result> Sendoc { get; set; }

        List<ITypeClass> iTypeClassList;

        List<sp_SelectTable3ForAdd_Result> sendListSP;

        string sendNoString;

        public AuditSent()
        {
            InitializeComponent();
            InitializeViewModel();

            InitCustomerListCustomer();

            InitData();
            Sendoc = new ObservableCollection<sp_SelectTable3ForAdd_Result>();
            gridMain.DataContext = Sendoc;

            iTypeClassList = new List<ITypeClass>();

        }

        void InitCustomerListCustomer()
        {
            customerList = visaORM.Customer.Where(c => c.FSysSend == false && c.FCreateCompany == MainContext.UserCompanyName).ToList();
        }

        void InitData()
        {
            cbVietnamCompany.ItemsSource = visaORM.TB_VietnamCompany.Where(v => v.FStatus == true).Select(v => v.FShortName);

            string todayStringShort = DateTime.Now.ToString("yyMMdd");
            txtDate.Text = DateTime.Now.ToShortDateString();

            InitSendComboBox();
            string NoSeq = "001";

            var lastSeqModelList = visaORM.SendInfo.Where(s => s.FCDate == todayStringShort).OrderByDescending(s => s.FID).Take(1);
            if (lastSeqModelList != null)
            {
                foreach (SendInfo lastSeqModel in lastSeqModelList)
                {
                    string tempString = "00" + (Convert.ToInt32(lastSeqModel.FSendNo.Substring(lastSeqModel.FSendNo.Length - 3)) + 1).ToString();
                    NoSeq = tempString.Substring(tempString.Length - 3);
                    break;
                }
            }
            txtDSN.Text = todayStringShort + "-" + NoSeq;



        }

        void InitSendComboBox()
        {
            var sendObjs = visaORM.SendInfo.Where(s => s.FSysSend == true && s.FSysPrint == false).OrderByDescending(s => s.FCreateDate).Select(s => s.FSendNo).ToList();

            List<string> sendNoList = new List<string>();

            foreach (string sendObj in sendObjs.Distinct())
            {
                sendNoList.Add(sendObj);
            }
            //cbListSend.ItemsSource = sendObjs.Select(s => s.FSendNo).Distinct();
            cbListSend.ItemsSource = sendNoList;

        }

        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);
            MyCommand newCommand = new MyCommand(newList)
            {
                Caption = "新建报审名单",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand sendCommand = new MyCommand(SendList)
            {
                Caption = "提交报审名单",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand exportCommand = new MyCommand(exportList)
            {
                Caption = "生成excel表格",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand refreshCommand = new MyCommand(refreshList)
            {
                Caption = "刷新",
                LargeGlyph = null,
                SmallGlyph = null
            };


            clipboardBar.Commands.Add(newCommand);
            clipboardBar.Commands.Add(sendCommand);
            clipboardBar.Commands.Add(exportCommand);
            clipboardBar.Commands.Add(refreshCommand);

            DataContext = viewModel;
        }

        void refreshList()
        {

            if (MessageBox.Show("将清空当前已扫描的签名列表，是否确认？", "刷新确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Sendoc.Clear();
                txtQZID.Text = "";
                InitCustomerListCustomer();
                InitSendComboBox();

            }
        }


        void newList()
        {
            refreshList();

        }

        /// <summary>
        /// 提交送审名单
        /// </summary>
        void SendList()
        {
            // var modelCollection = visaORM.Customer.Where(c => c.FSysPut == false);
            try
            {
                if (cbVietnamCompany.SelectedItem == null)
                {
                    MessageBox.Show("请先选择越南公司");
                    return;
                }

                string todayStringShort = DateTime.Now.ToString("yyMMdd");
                string todayStringAll = DateTime.Now.ToShortDateString();

                string selectietnamCompany = cbVietnamCompany.SelectedItem.ToString();

                int orderNum = 1;

                foreach (var model in Sendoc)
                {

                    var BlackListObj = visaORM.CustomerT.FirstOrDefault(c => c.FPassportNo == model.FPassportNo);
                    if (BlackListObj != null)
                    {
                        if (MessageBox.Show(string.Format("护照号{0}是特殊人物，是否让其提交？", model.FPassportNo), "特殊人物确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            continue;
                        }
                    }


                    if (model.FSysSend == true)
                    {
                        MessageBox.Show(" 此签证已经送审过，不能再次送审:" + model.FPassportNo);
                        continue;
                    }




                    //visaORM.Customer.Attach(model);
                    Customer modelC = visaORM.Customer.FirstOrDefault(c => c.FID == model.FID);
                    if (modelC == null)
                    {
                        MessageBox.Show("该签证不存在或者已经被删除：" + model.FPassportNo);
                        continue;
                    }
                    visaORM.ObjectStateManager.ChangeObjectState(modelC, System.Data.EntityState.Modified);
                    modelC.FSysSend = true;
                    modelC.FSysSendDate = DateTime.Now;
                    modelC.FSysSendUser = MainContext.UserID;
                    modelC.FYNCom = selectietnamCompany;


                    var modelSend = new SendInfo();



                    modelSend.FCustomerID = model.FID;
                    modelSend.FQZID = model.FQZID;
                    modelSend.FSendDate = modelC.FSysSendDate;
                    modelSend.FDSN = txtDSN.Text;
                    modelSend.FVietnamCompany = selectietnamCompany;
                    modelSend.FCreateCompany = MainContext.UserCompanyName;
                    modelSend.FSendNo = modelSend.FDSN;
                    modelSend.FOrder = orderNum;
                    modelSend.FCDate = todayStringShort;
                    modelSend.FSysPrintNum = 0;

                    modelSend.FCreateDate = DateTime.Now;
                    modelSend.FCreateUser = MainContext.UserID;
                    modelSend.FModifyDate = DateTime.Now;
                    modelSend.FModifyUser = MainContext.UserID;
                    modelSend.FStatus = true;


                    modelSend.FSysSend = true;
                    modelSend.FSysSendDate = modelC.FSysSendDate;
                    modelSend.FSysSendUser = 1;

                    modelSend.FSysChk = false;
                    modelSend.FSysPrint = false;

                    visaORM.SendInfo.AddObject(modelSend);

                    orderNum++;
                }
                visaORM.SaveChanges();

                Sendoc.Clear();
                MessageBox.Show("送审成功");

                //重新初始化DSN下拉
                InitSendComboBox();
                this.sendNoString = txtDSN.Text;
                //cbListSend.SelectedItem = txtDSN.Text;
                //导出
                exportList();
                //((TableView)gridMain.View).Print();

                //越南公司清空
                this.cbVietnamCompany.SelectedIndex = -1;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }

        void exportList()
        {
            try
            {
                if (string.IsNullOrEmpty(this.sendNoString))
                {
                    MessageBox.Show("请先选择送审名单 ");
                    return;
                }

                string fileName;
                string filepath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "desktop", "").ToString();
                if (!(String.IsNullOrEmpty(filepath)))
                {
                    fileName = filepath + "\\" + txtDSN.Text + ".xlsx";
                }
                else
                {
                    System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                    saveFile.Filter = "Excel文件|*.xlsx";
                    if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        fileName = saveFile.FileName;
                    }
                    else
                    {
                        return;
                    }
                }

                int i = 0;

                List<sp_SelectTable3ForExcel_Result> sendCollection = visaORM.sp_SelectTable3ForExcel(this.sendNoString).ToList();

                if (cbVietnamCompany.EditValue == null || cbVietnamCompany.EditValue.ToString() == "")
                {
                    MessageBox.Show("请填写越南公司");
                    return;
                }

                if (sendCollection.Count() > 0)
                {
                    Excel.ExportC.ExportTable3(fileName, sendCollection, txtDate.DateTime.ToString("MM/dd/yyyy"), txtDSN.Text, txtSpecial.Text, messages);
                    //Excel.ExportC.(fileName); 
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }

        private void txtFQZID_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    string keyString = txtQZID.Text;
                    var model = visaORM.sp_SelectTable3ForAdd(keyString).FirstOrDefault();
                    if (model == null)
                    {
                        MessageBox.Show("找不到对应的签证");
                        return;
                    }
                    if (model.FSysPut == false)
                    {
                        MessageBox.Show("此签证还未提交，不能送审。");
                        return;
                    }

                    if (string.IsNullOrEmpty(model.FPassportNo))
                    {
                        MessageBox.Show("该签证没有填写护照，请重新填写");
                        return;
                    }

                    if (model.FSysSend == true)
                    {
                        MessageBox.Show("该签证已经送审");
                        return;
                    }
                    if (Sendoc.FirstOrDefault(s => s.FAutoID == txtQZID.Text) != null)
                    {
                        MessageBox.Show("该签证已在扫描列表内，无需重复扫描");
                        return;
                    }

                    //if (model.FsysZF == true)
                    //{
                    //    MessageBox.Show("此签证申请号为已作废，不能送审。");
                    //    return;
                    //}

                    if (visaORM.CustomerT.Where(t => t.FPassportNo == model.FPassportNo).Count() > 0)
                    {
                        if (MessageBox.Show("此护照在黑名单中，是否继续添加。", "添加确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    List<sp_SelectTable3ForAdd_Result> sendList = Sendoc.ToList();
                    sendList.Add(model);
                    Sendoc.Clear();
                    foreach (var sObj in sendList.OrderBy(s => s.iType).ThenBy(s => s.FDurationDay).ThenBy(s => s.FTimes))
                    {
                        Sendoc.Add(sObj);
                    }

                    txtQZID.Text = "";

                    //设置右上角信息
                    SetMessageInfo(Sendoc.ToList());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }

        private void txtSubmitNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    string keyString = txtSubmitNo.Text;
                    //var cObjs = from c in visaORM.Customer
                    //            join s in visaORM.TB_TableSubmit.Where(sub => sub.FSubmitNo == keyString)
                    //            on c.FAutoID equals s.FAutoID
                    //            select c;

                    ////visaORM.Customer.FirstOrDefault(c => c.FAutoID == keyString);
                    //if (cObjs.Count() < 1)
                    //{
                    //    MessageBox.Show("找不到对应的表二名单");
                    //    return;
                    //}

                    foreach (var cObj in visaORM.sp_SelectTable3ForAdd(keyString))
                    {
                        if (cObj.FSysSend == true)
                        {
                            continue;
                        }

                        if (Sendoc.FirstOrDefault(s => s.FAutoID == cObj.FAutoID) != null)
                        {
                            continue;
                        }

                        List<sp_SelectTable3ForAdd_Result> sendList = Sendoc.ToList();
                        sendList.Add(cObj);
                        Sendoc.Clear();
                        foreach (var sObj in sendList.OrderBy(s => s.iType).ThenBy(s => s.FPurpose).ThenBy(s => s.FDurationDay).ThenBy(s => s.FTimes))
                        {
                            Sendoc.Add(sObj);
                        }

                        //设置右上角信息
                        SetMessageInfo(Sendoc.ToList());

                    }
                    txtSubmitNo.Text = "";

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }


        }

        /// <summary>
        /// 送审签证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbListSend_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbListSend.SelectedItem == null)
            {
                return;
            }

            try
            {
                if (Sendoc.Count > 0)
                {
                    if (MessageBox.Show("选择历史送审清单，将清空当前已扫描的签名列表，是否确认？", "刷新确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                this.sendNoString = cbListSend.SelectedItem.ToString();
                SetMainGridData(cbListSend.SelectedItem.ToString());


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
        private void SetMainGridData(string sendNoString)
        {
            try
            {

                Sendoc.Clear();


                var sendCollection = from c in visaORM.Customer
                                     join s in visaORM.SendInfo
                                     on c.FID equals s.FCustomerID
                                     where s.FSendNo == sendNoString
                                     select c;
                List<sp_SelectTable3ForAdd_Result> sendList = new List<sp_SelectTable3ForAdd_Result>();
                //visaORM.SendInfo.Where(s => s.FSendNo == cbListSend.SelectedItem.ToString());
                foreach (Customer CModel in sendCollection)
                {
                    sendList.Add(visaORM.sp_SelectTable3ForAdd(CModel.FAutoID).FirstOrDefault());
                }

                //根据颜色排序
                foreach (var sObj in sendList.OrderBy(s => s.iType))
                {
                    Sendoc.Add(sObj);
                }

                string sendNo = cbListSend.SelectedItem.ToString();
                SendInfo sendObj = visaORM.SendInfo.FirstOrDefault(s => s.FSendNo == sendNo);
                if (sendObj != null)
                {
                    if (sendObj.FSysSendDate != null)
                        txtDate.Text = ((DateTime)(sendObj.FSysSendDate)).ToShortDateString();
                    txtDSN.Text = sendObj.FDSN;
                    cbVietnamCompany.SelectedItem = sendObj.FVietnamCompany;
                }

                //Dictionary<int?, List<int>> dic = new Dictionary<int?, List<int>>();

                SetMessageInfo(sendList);


                txtDSN.Text = cbListSend.SelectedItem.ToString();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        /// <summary>
        /// 设置右上角信息
        /// </summary>
        /// <param name="sendNo"></param>
        private void SetMessageInfo(List<sp_SelectTable3ForAdd_Result> sendListSP)
        {
            //this.sendListSP = visaORM.sp_SelectTable3ForExcel(sendNo).ToList();
            this.sendListSP = sendListSP;
            iTypeClassList.Clear();

            int tt = 0;
            foreach (var sendSPObj in sendListSP)
            {
                tt++;
                if (iTypeClassList.FirstOrDefault(it => it.FID == sendSPObj.iType) == null && sendSPObj.iType != 33)
                {
                    iTypeClassList.Add(new ITypeClass() { FID = sendSPObj.iType, FList = new List<int>() });
                }
                if (sendSPObj.iType == 33)
                {
                    iTypeClassList.Add(new ITypeClass() { FID = sendSPObj.iType, FList = new List<int>() });
                    iTypeClassList.LastOrDefault().FList.Add(tt);
                    continue;
                }
                iTypeClassList.FirstOrDefault(it => it.FID == sendSPObj.iType).FList.Add(tt);
            }





            messages.Clear();
            int nowNum;
            txtMessageInfo.Document.Blocks.Clear();

            StringBuilder mess = new StringBuilder();
            mess.AppendFormat("{0} Pax", gridMain.VisibleRowCount);
            foreach (ITypeClass iTypeClassObj in iTypeClassList)
            {
                nowNum = iTypeClassObj.FList.FirstOrDefault() - 1;

                if (iTypeClassObj.FList.Count == 1)
                {
                    mess.AppendFormat("\r\npax {0} visa {1} {2}{3}.", iTypeClassObj.FList.FirstOrDefault(), GetPurposeCode(sendListSP[nowNum].FPurpose), GetDurationDay(sendListSP[nowNum].FDurationDay), GetTimes(sendListSP[nowNum].FTimes));
                    messages.Add(new visa.CLEINT.Excel.MessageInfo(String.Format("pax {0} visa {1} {2}{3}.", iTypeClassObj.FList.FirstOrDefault(), GetPurposeCode(sendListSP[nowNum].FPurpose), GetDurationDay(sendListSP[nowNum].FDurationDay), GetTimes(sendListSP[nowNum].FTimes)), iTypeClassObj.FID));
                }
                else if (iTypeClassObj.FList.Count > 1)
                {
                    mess.AppendFormat("\r\npax {0} - {1} visa {2} {3}{4}.", iTypeClassObj.FList.FirstOrDefault(), iTypeClassObj.FList.LastOrDefault(), GetPurposeCode(sendListSP[nowNum].FPurpose), GetDurationDay(sendListSP[nowNum].FDurationDay), GetTimes(sendListSP[nowNum].FTimes));
                    messages.Add(new visa.CLEINT.Excel.MessageInfo(String.Format("pax {0} - {1} visa {2} {3}{4}.", iTypeClassObj.FList.FirstOrDefault(), iTypeClassObj.FList.LastOrDefault(), GetPurposeCode(sendListSP[nowNum].FPurpose), GetDurationDay(sendListSP[nowNum].FDurationDay), GetTimes(sendListSP[nowNum].FTimes)), iTypeClassObj.FID));
                }
            }
            txtMessageInfo.AppendText(mess.ToString());
        }




        private void cbVietnamCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbVietnamCompany.SelectedItem != null)
                {
                    string NoSeq = "001";
                    string todayStringShort = DateTime.Now.ToString("yyMMdd");
                    string vietnamCompanyString = cbVietnamCompany.SelectedItem.ToString();


                    var lastSeqModelList = visaORM.SendInfo.Where(s => s.FCDate == todayStringShort && s.FVietnamCompany == vietnamCompanyString).OrderByDescending(s => s.FID).Take(1);
                    if (lastSeqModelList != null)
                    {
                        foreach (SendInfo lastSeqModel in lastSeqModelList)
                        {
                            string tempString = "00" + (Convert.ToInt32(lastSeqModel.FSendNo.Substring(lastSeqModel.FSendNo.Length - 3)) + 1).ToString();
                            NoSeq = tempString.Substring(tempString.Length - 3);
                            break;
                        }
                    }
                    txtDSN.Text = vietnamCompanyString + "-" + todayStringShort + "-" + NoSeq;

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void btnCheckUnRead_Click(object sender, RoutedEventArgs e)
        {
            //if (Sendoc.Count < 1)
            //{
            //    MessageBox.Show("还未有扫描签证");
            //    return;
            //}

            ChkUnScan ChkUnScanList = new ChkUnScan();
            DXDialog dg = new DXDialog()
            {
                Content = ChkUnScanList,
                Title = "查看未扫描签证"
            };
            dg.Width = 750;
            dg.Height = 500;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);

        }

        /// <summary>
        /// 停止发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopSend_Click(object sender, RoutedEventArgs e)
        {
            sp_SelectTable3ForAdd_Result cObj = gridMain.SelectedItem as sp_SelectTable3ForAdd_Result;
            try
            {
                if (gridMain.SelectedItem == null)
                {
                    MessageBox.Show("请先选择一个签证");
                    return;
                }
                if (MessageBox.Show("是否确认取消该签证?", "取消确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    cObj.FStopSend = true;
                    visaORM.ObjectStateManager.ChangeObjectState(cObj, System.Data.EntityState.Modified);
                    visaORM.SaveChanges();

                    Sendoc.Remove(cObj);

                    MessageBox.Show("已经取消签证号为 " + cObj.FPassportNo + " 的签证");
                }
            }
            catch
            {
                Sendoc.Remove(cObj);

                MessageBox.Show("已经取消签证号为 " + cObj.FPassportNo + " 的签证");
            }

        }

        //public static string GetStatus(int? iType)
        //{
        //    switch (iType)
        //    {
        //        case 1:
        //            return "DL1S";
        //        case 2:
        //            return "DL 1 thang nhieu lan";
        //        case 3:
        //            return "thuong mai/dau tu 1 thang 1 lan";
        //        case 4:
        //            return "others 1 thang 1 lan";
        //        case 5:
        //            return "others 1 thang nhieu lan";
        //        case 6:
        //            return "thuong mai/tham than/dau tu 3 thang 1 lan";
        //        case 7:
        //            return "others 3 thang 1 lan";
        //        case 8:
        //            return "thuong mai/tham than/dau tu/duhoc 3 thang nhieu lan";
        //        case 9:
        //            return "others 3 thang nhieu lan";
        //        case 10:
        //            return "others 6 thang 1 lan";
        //        case 11:
        //            return "thuong mai 6 thang nhieu lan";
        //        case 12:
        //            return "others 6 thang nhieu lan";
        //        case 13:
        //            return "others 12 thang 1 lan";
        //        case 14:
        //            return "others 12 thang nhieu lan";
        //        default:
        //            return "others";
        //    }
        //}

        public string GetPurposeCode(string purpose)
        {
            switch (purpose)
            {
                case "旅游":
                    return "DL";
                case "企业":
                    return "DN";
                case "劳务":
                    return "LĐ";
                case "领事馆":
                    return "SQ";
                case "投资":
                    return "ĐT";
                case "留学":
                    return "DH";
                case "外交NG1":
                    return "NG1";
                case "外交NG2":
                    return "NG2";
                case "外交NG3":
                    return "NG3";
                case "外交NG4":
                    return "NG4";
                case "其他VR":
                    return "VR";
                case "其他TT":
                    return "TT";
                case "其他NN1":
                    return "NN1";
                case "其他NN2":
                    return "NN2";
                case "其他NN3":
                    return "NN3";
                case "其他LV1":
                    return "LV1";
                case "其他LV2":
                    return "LV2";
                case "其他HN":
                    return "HN";
                default:
                    return "";
            }

        }

        public string GetDurationDay(int? durationDay)
        {
            switch (durationDay)
            {
                case 1:
                    return "1";
                case 2:
                    return "3";
                case 3:
                    return "6";
                case 4:
                    return "1Y";
                case 5:
                    return "2Y";
                case 6:
                    return "3Y";
                case 7:
                    return "4Y";
                case 8:
                    return "5Y";
                default:
                    return "1";

            }
        }

        public string GetTimes(int? times)
        {
            switch (times)
            {
                case 1:
                    return "S";
                case 2:
                    return "M";
                default:
                    return "S";

            }
        }


        private void gridMain_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "STT")
            {
                e.Value = gridMain.GetRowVisibleIndexByHandle(gridMain.GetRowHandleByListIndex(e.ListSourceRowIndex)) + 1;
            }
        }



    }

    public class ITypeClass
    {
        public int? FID;
        public List<int> FList;
    }

}
