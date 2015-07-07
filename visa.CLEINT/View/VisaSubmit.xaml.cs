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
    /// VisaSubmit.xaml 的交互逻辑
    /// </summary>
    public partial class VisaSubmit : UserControl
    {

        //List<Customer> customerList = new List<Customer>();
        visaEntities visaORM = new visaEntities();
        string newSubmitNo = "";
        ObservableCollection<string> SubmitCollection = new ObservableCollection<string>();

        /// <summary>
        /// 是否有表二管理权限
        /// </summary>
        bool hasTable2AdminRight = false;

        public VisaSubmit()
        {
            InitializeComponent();
            InitializeViewModel();

            //InitCustomerListCustomer();

            InitData();


            //gridMain.ItemsSource = customerList;
            gridMain.ItemsSource = visaORM.sp_SelectTbSubmit(MainContext.UserCompanyName);
        }

        void InitData()
        {

            try
            {
                //var submitObjs = visaORM.TB_TableSubmit.Where(s => s.FCompany == MainContext.UserCompanyName && s.FStatus == true).OrderByDescending(s => s.FCreateDate).ToList();
                //cbSubmitList.ItemsSource = submitObjs.Select(s => s.FSubmitNo).Distinct();
                InitSubmitNoList(false);

                cbSubmitList.ItemsSource = SubmitCollection;

                setSubmitInfo();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        #region  按钮
        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);
            MyCommand prevCommand = new MyCommand(submitVisa)
            {
                Caption = "提交签证申请",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand printCommand = new MyCommand(printVisa)
            {
                Caption = "打印名单号签证列表(表1)",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand refreshCommand = new MyCommand(refreshSubmit)
            {
                Caption = "刷新",
                LargeGlyph = null,
                SmallGlyph = null
            };

            clipboardBar.Commands.Add(prevCommand);
            clipboardBar.Commands.Add(printCommand);
            clipboardBar.Commands.Add(refreshCommand);

            DataContext = viewModel;
        }
        #endregion




        //void InitCustomerListCustomer()
        //{
        //    customerList = visaORM.Customer.Where(c => c.FSysPut == false && c.FCreateCompany == MainContext.UserCompanyName && c.FsysZF!=true).ToList();
        //}


        /// <summary>
        /// 刷新
        /// </summary>
        void refreshSubmit()
        {
            try
            {
                cbSubmitList.SelectedIndex = -1;

                //customerList = visaORM.Customer.Where(c => c.FSysPut == false && c.FCreateCompany == MainContext.UserCompanyName).ToList();
                gridMain.ItemsSource = visaORM.sp_SelectTbSubmit(MainContext.UserCompanyName);

                InitData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }
        }

        /// <summary>
        /// 设置空白时的表2值
        /// </summary>
        void setSubmitInfo()
        {
            txtSubmitNo.Text = "";
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtCompanyName.Text = MainContext.UserCompanyName;
        }

        /// <summary>
        /// 提交签证
        /// </summary>
        void submitVisa()
        {
            try
            {
                var modelCollection = visaORM.Customer.Where(c => c.FSysPut == false && c.FCreateUser == MainContext.UserID && c.FsysZF != true && c.FStopSend != true);


                string todayStringShort = DateTime.Now.ToString("yyMMdd");
                string todayStringAll = DateTime.Now.ToShortDateString();

                string NoSeq = "001";

                var lastSeqModelList = visaORM.TB_TableSubmit.Where(s => s.FCDate == todayStringShort).OrderByDescending(s => s.FID).Take(1);
                if (lastSeqModelList != null)
                {
                    foreach (TB_TableSubmit lastSeqModel in lastSeqModelList)
                    {
                        string tempString = "00" + (Convert.ToInt32(lastSeqModel.FSubmitNo.Substring(lastSeqModel.FSubmitNo.Length - 3)) + 1).ToString();
                        NoSeq = tempString.Substring(tempString.Length - 3);
                        break;
                    }
                }


                foreach (var model in modelCollection)
                {

                    visaORM.Customer.Attach(model);
                    visaORM.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                    model.FSysPut = true;
                    model.FSysPutDate = DateTime.Now;
                    model.FSysPutUser = MainContext.UserID;


                    var modelSubmit = new TB_TableSubmit();

                    modelSubmit.FCDate = todayStringShort;
                    modelSubmit.FCompany = MainContext.UserCompanyName;
                    modelSubmit.FSubmitNo = "TJ-" + todayStringShort + "-" + NoSeq;
                    modelSubmit.FAutoID = model.FAutoID;
                    modelSubmit.FSysMemo = txtSubmitMemo.Text;

                    modelSubmit.FCreateDate = DateTime.Now;
                    modelSubmit.FCreateUser = MainContext.UserID;
                    modelSubmit.FModifyDate = DateTime.Now;
                    modelSubmit.FModifyUser = MainContext.UserID;
                    modelSubmit.FStatus = true;

                    visaORM.TB_TableSubmit.AddObject(modelSubmit);
                }
                visaORM.SaveChanges();

                newSubmitNo = "TJ-" + todayStringShort + "-" + NoSeq;

                MessageBox.Show("提交成功");
                if (System.Windows.MessageBox.Show("提交成功，是否打印表二", "保存成功", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //printVisa();
                    (App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report2(MainContext.UserCompanyName, newSubmitNo));

                }
                refreshSubmit();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }
        }

        void printVisa()
        {
            try
            {
                if (cbSubmitList.SelectedItem == null)
                {
                    MessageBox.Show("请先选择已提交名单");
                    return;
                }

                string submitNo = cbSubmitList.SelectedItem.ToString();
                string keyString = "";


                var visaModelArray = visaORM.TB_TableSubmit.Where(s => s.FSubmitNo == submitNo);
                //List<string> ids = new List<string>();
                foreach (var visaModel in visaModelArray)
                {
                    keyString += visaModel.FAutoID + ",";
                    //ids.Add(visaModel.FAutoID); 
                }

                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(ids));


                Report.ReportTable1 report = new Report.ReportTable1();
                report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);

                DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
                pt.Print();

                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report2(MainContext.UserCompanyName, newSubmitNo));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void cbSubmitList_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbSubmitList.SelectedItem != null)
                {

                    string cbSelectValue = cbSubmitList.SelectedItem.ToString();
                    //var  selectCustomerList = from c in visaORM.Customer
                    //join s in visaORM.TB_TableSubmit
                    //on c.FQZID equals s.FQZID
                    //where s.FSubmitNo == cbSelectValue
                    //select c;

                    gridMain.ItemsSource = visaORM.sp_SelectTbSubmitWithSubmitNo(MainContext.UserCompanyName, cbSelectValue);

                    //  selectCustomerList.ToList();

                    TB_TableSubmit ts = visaORM.TB_TableSubmit.FirstOrDefault(s => s.FSubmitNo == cbSelectValue);

                    if (ts != null)
                    {
                        txtSubmitNo.Text = ts.FSubmitNo;
                        txtDate.Text = ts.FCDate;
                        txtCompanyName.Text = ts.FCompany;
                        txtSubmitMemo.Text = ts.FSysMemo;
                    }


                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void txtPrintSingle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Report.ReportTable1 report = new Report.ReportTable1();
                report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, this.txtPrintSingle.Text);

                DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
                pt.Print();
                //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(this.txtPrintSingle.Text));

            }

        }

        private void btnPrintSubmitNo_Click(object sender, RoutedEventArgs e)
        {
            if (cbSubmitList.SelectedItem == null)
            {
                MessageBox.Show("请选择一个名单号");
                return;
            }
            (App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report2(MainContext.UserCompanyName, cbSubmitList.SelectedItem.ToString()));

        }

        void InitSubmitNoList(bool isAll)
        {
            try
            {

                SubmitCollection.Clear();
                List<string> list = new List<string>();


                if (isAll == false) //只显示未打印的
                {
                    if (hasTable2AdminRight == true)
                    {
                        list =
                          (from s in visaORM.TB_TableSubmit
                           join c in visaORM.Customer
                           on s.FAutoID equals c.FAutoID
                           where c.FSysPrint == false
                           orderby s.FID descending
                           select s.FSubmitNo).ToList();
                    }
                    else
                    {
                        list =
                            (from s in visaORM.TB_TableSubmit
                             join c in visaORM.Customer
                             on s.FAutoID equals c.FAutoID
                             where c.FSysPrint == false && s.FCompany == MainContext.UserCompanyName
                             orderby s.FID descending
                             select s.FSubmitNo).ToList();

                    }
                }
                else //显示所有的
                {
                    if (hasTable2AdminRight == true)
                    {
                        list =
                          (from s in visaORM.TB_TableSubmit
                           join c in visaORM.Customer
                           on s.FAutoID equals c.FAutoID
                           orderby s.FID descending
                           select s.FSubmitNo).ToList();
                    }
                    else
                    {
                        list =
                            (from s in visaORM.TB_TableSubmit
                             join c in visaORM.Customer
                             on s.FAutoID equals c.FAutoID
                             where s.FCompany == MainContext.UserCompanyName
                             orderby s.FID descending
                             select s.FSubmitNo).ToList();

                    }
                }

                foreach (string sString in list.Distinct())
                {
                    SubmitCollection.Add(sString);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }


        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            InitSubmitNoList(true);
            DevExpress.Xpf.Core.DXMessageBox.Show("已经可以选择所有历史单号");

        }


    }
}
