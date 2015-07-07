using DevExpress.Xpf.Editors;
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
using visa.CLEINT.Data;
using visa.CLEINT.Model;
using visa.MODEL;

namespace visa.CLEINT.View
{
    /// <summary>
    /// AuditReceived.xaml 的交互逻辑
    /// </summary>
    public partial class AuditReceived : UserControl
    {
        List<Customer> customerList = new List<Customer>();
        visaEntities visaORM = new visaEntities();
        ObservableCollection<sp_SelectTable3ForExcel_Result> sendCollection = new ObservableCollection<sp_SelectTable3ForExcel_Result>();

        ObservableCollection<sp_SelectTable3ForExcel> newSendCollection = new ObservableCollection<sp_SelectTable3ForExcel>();
        
        public AuditReceived()
        {
            InitializeComponent();

            InitializeViewModel();

            InitCustomerListCustomer();

            //gridMain.ItemsSource = customerList;

            InitData();


        }

        void InitData()
        {
            cbSentVisa.SelectedIndex = -1;
            cbSentVisa.ItemsSource = visaORM.SendInfo.Where(s=>s.FSysChk==false).Select(s => s.FSendNo).Distinct();
            cbHistorySendList.ItemsSource = visaORM.SendInfo.Where(s => s.FSysChk == true).Select(s => s.FSendNo).Distinct();
            gridMain.DataContext = newSendCollection;
        }

        void InitCustomerListCustomer()
        {
            //customerList = visaORM.Customer.Where(c => c.FSysChk == false).ToList();
        }

        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);
            MyCommand newCommand = new MyCommand(auditList)
            {
                Caption = "审核签证",
                LargeGlyph = null,
                SmallGlyph = null
            };


            clipboardBar.Commands.Add(newCommand);

            DataContext = viewModel;
        }

        void auditList()
        {
            MainBar.Focus();

            if (cbSentVisa.SelectedIndex == -1)
            {
                MessageBox.Show("请选择需要审核的名单");
                return;
            }
            


            //string SentNo = cbSentVisa.SelectedItem.ToString();

            //var modelCollection = from c in visaORM.Customer
            //                     join s in visaORM.SendInfo
            //                     on c.FID equals s.FCustomerID
            //                     where s.FSendNo == SentNo
            //                     select c;



            string todayStringShort = DateTime.Now.ToString("yyMMdd");
            string todayStringAll = DateTime.Now.ToShortDateString();

            int i = 0;
            foreach (var model in sendCollection)
            {

                sp_SelectTable3ForExcel_Result rowObj = gridMain.GetRow(i) as sp_SelectTable3ForExcel_Result;
                i++;
                if (rowObj.SelectChk == false)
                {
                    continue;
                }

                Customer cModel = visaORM.Customer.FirstOrDefault(c => c.FID == model.QID);
                if (cModel != null)
                {
                    visaORM.Customer.Attach(cModel);
                    cModel.FSysChk = true;
                    cModel.FSysChkDate = DateTime.Now;
                    cModel.FSysChkUser = MainContext.UserID;

                    visaORM.ObjectStateManager.ChangeObjectState(cModel, System.Data.EntityState.Modified);

                    var modelSend = visaORM.SendInfo.FirstOrDefault(s => s.FCustomerID == cModel.FID);

                    if (modelSend != null)
                    {



                        modelSend.FModifyDate = DateTime.Now;
                        modelSend.FModifyUser = MainContext.UserID;

                        modelSend.FSysChk = true;
                        modelSend.FSysChkDate = cModel.FSysChkDate;
                        modelSend.FSysChkUser = MainContext.UserID;
                        visaORM.ObjectStateManager.ChangeObjectState(modelSend, System.Data.EntityState.Modified);

                    }

                }

            }
            visaORM.SaveChanges();

            MessageBox.Show("审核成功");

            InitData();


        }

        private void cbSentVisa_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            cbHistorySendList.SelectedIndex = -1;
            GetSendListDetail(cbSentVisa);

        }

        private void GetSendListDetail(ComboBoxEdit cb)
        {
            if (cb.SelectedItem != null)
            {
                string SentNo = cb.SelectedItem.ToString();

                var model = visaORM.SendInfo.FirstOrDefault(s => s.FSendNo == SentNo);
                if (model != null)
                {
                    txtDSN.Text = model.FDSN;
                    txtSendNo.Text = model.FSendNo;
                    txtCompany.Text = model.FCreateCompany;
                    txtDate.Text = model.FCDate;
                    if (model.FCDate != null)
                        txtDate.Text = model.FCDate.ToString();



                  //sendCollection = (from c in visaORM.Customer
                  //                       join s in visaORM.SendInfo
                  //                       on c.FID equals s.FCustomerID
                  //                       where s.FSendNo == SentNo
                  //                       select c).ToList();


                   // sendCollection = visaORM.sp_SelectTable3ForExcel(SentNo).ToList();
                    //gridMain.ItemsSource = sendCollection;

                    foreach (var sObj in visaORM.sp_SelectTable3ForExcel(SentNo))
                    {
                        sendCollection.Add(sObj);
                    }

                    gridMain.DataContext = sendCollection;

                }


            }
            else
            {
                gridMain.ItemsSource = null;
            }
        }

        private void cbHistorySendList_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            cbSentVisa.SelectedIndex = -1;
            GetSendListDetail(cbHistorySendList);



        }


        private void checkName_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (e.Handled != null)
            {
            }

        }




    }
}
