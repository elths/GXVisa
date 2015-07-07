using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
    /// Report2.xaml 的交互逻辑
    /// </summary>
    public partial class Report2 : UserControl
    {

        visaEntities visaORM = new visaEntities();

        ObservableCollection<string> SubmitCollection = new ObservableCollection<string>();

        /// <summary>
        /// 是否有表二管理权限
        /// </summary>
        bool hasTable2AdminRight = false;


        public Report2()
        {
            try
            {
                InitializeComponent();

                InitPreview(MainContext.UserCompanyName, "");


                TB_Menu mObj = visaORM.TB_Menu.FirstOrDefault(m => m.FName == "Table2Admin" && m.FStatus == true);
                if (mObj != null)
                {
                    if (visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == mObj.FID && ur.FUserID == MainContext.UserID && ur.FStatus == true) != null)
                        hasTable2AdminRight = true;
                }

                InitData();
                InitSubmitNoList(false);

                cbSubmitNo.DataContext = SubmitCollection;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        void InitData()
        {

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

        public Report2(string companyName, string SubmitNo)
        {
            try
            {
                InitializeComponent();
                InitPreview(companyName, SubmitNo);
                InitSubmitNoList(false);
                cbSubmitNo.DataContext = SubmitCollection;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }


        void InitPreview(string companyName, string SubmitNo)
        {
            try
            {
                Report.ReportTable2 report = new Report.ReportTable2();
                report.sp_SelectTbSubmitWithSubmitNoTableAdapter.Fill(report.___sp_SelectTbSubmitWithSubmitNo_1.sp_SelectTbSubmitWithSubmitNo, companyName, SubmitNo);
                MainPreview.Model = new XtraReportPreviewModel(report);

                report.xrLebelID.Text = SubmitNo;
                report.xrBarcodeID.Text = SubmitNo;
                report.xrLebelCompany.Text = companyName;
                report.xrLebelDate.Text = DateTime.Now.ToShortDateString();
                report.xrLabelCaption.Text = txtReportTitle.Text;

                var submitObj = visaORM.TB_TableSubmit.FirstOrDefault(s => s.FSubmitNo == SubmitNo);
                if (submitObj != null)
                    report.xrLabelSysMemo.Text = submitObj.FSysMemo;


                report.CreateDocument();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void cbSubmitNo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbSubmitNo.SelectedItem == null)
                {
                    return;
                }
                if (e.OriginalSource != null)
                {
                    InitPreview(MainContext.UserCompanyName, cbSubmitNo.SelectedItem.ToString());
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
            DXMessageBox.Show("已经可以选择所有历史单号");

        }
    }
}
