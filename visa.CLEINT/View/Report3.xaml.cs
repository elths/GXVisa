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
    /// Report3.xaml 的交互逻辑
    /// </summary>
    public partial class Report3 : UserControl
    {
        List<Customer> customerList = new List<Customer>();
        visaEntities visaORM = new visaEntities();

        private List<visa.CLEINT.Excel.MessageInfo> messages = new List<visa.CLEINT.Excel.MessageInfo>();

        ObservableCollection<Customer> Sendoc = new ObservableCollection<Customer>();

        public Report3()
        {
            InitializeComponent();
            InitializeViewModel();

            InitCustomerListCustomer();

            InitData();
            gridMain.DataContext = Sendoc;



        }

        void InitCustomerListCustomer()
        {
            customerList = visaORM.Customer.Where(c => c.FSysSend == false&&c.FCreateCompany == MainContext.UserCompanyName).ToList();
        }

        void InitData()
        {
            cbVietnamCompany.ItemsSource = visaORM.TB_VietnamCompany.Where(v => v.FStatus == true).Select(v => v.FShortName);

            string todayStringShort = DateTime.Now.ToString("yyMMdd");
            txtDate.Text = DateTime.Now.ToShortDateString();

            cbListSend.ItemsSource = visaORM.SendInfo.Where(s => s.FSysSend == true).Select(s => s.FSendNo).Distinct();

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

        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);

            MyCommand exportCommand = new MyCommand(exportList)
            {
                Caption = "生成excel表格",
                LargeGlyph = null,
                SmallGlyph = null
            };


            clipboardBar.Commands.Add(exportCommand);

            DataContext = viewModel;
        }




        void exportList()
        {

            if (cbListSend.SelectedItem == null)
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


            List<sp_SelectTable3ForExcel_Result> sendCollection = visaORM.sp_SelectTable3ForExcel(cbListSend.SelectedItem.ToString()).ToList();

            if (cbVietnamCompany.SelectedItem == null)
            {
                MessageBox.Show("请选择越南公司");
                return;
            }



            if (sendCollection.Count() > 0)
            {
                Excel.ExportC.ExportTable3(fileName, sendCollection, txtDate.Text.Substring(4, 2) + "/" + txtDate.Text.Substring(2, 2) + "/" + "20" + txtDate.Text.Substring(0, 2), txtDSN.Text, cbVietnamCompany.SelectedItem.ToString(), messages);
                //Excel.ExportC.PirntExcel(fileName);
            }

        }


        private void cbListSend_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cbListSend.SelectedItem == null)
            {
                return;
            }

            string sendNoString = cbListSend.SelectedItem.ToString();

            Sendoc.Clear();


            var sendCollection = from c in visaORM.Customer
                                 join s in visaORM.SendInfo
                                 on c.FID equals s.FCustomerID
                                 where s.FSendNo == sendNoString
                                 select c;

            //visaORM.SendInfo.Where(s => s.FSendNo == cbListSend.SelectedItem.ToString());
            foreach (Customer model in sendCollection)
            {
                Sendoc.Add(model);

            }

            string sendNo = cbListSend.SelectedItem.ToString();
            SendInfo sendObj = visaORM.SendInfo.FirstOrDefault(s => s.FSendNo == sendNo);
            if (sendObj != null)
            {
                txtDate.Text = ((DateTime)(sendObj.FSysSendDate)).ToShortDateString();
                txtDSN.Text = sendObj.FDSN;
                cbVietnamCompany.SelectedItem = sendObj.FVietnamCompany;
            }


            Dictionary<int?, List<int>> dic = new Dictionary<int?, List<int>>();

            var sendListSP = visaORM.sp_SelectTable3ForExcel(sendNo);

            int tt = 0;
            foreach (var sendObjSP in sendListSP)
            {
                tt++;
                if (!(dic.ContainsKey(sendObjSP.iType)))
                {
                    dic.Add(sendObjSP.iType, new List<int>());
                }
                dic[sendObjSP.iType].Add(tt);

            }





            messages.Clear();
            txtMessageInfo.Document.Blocks.Clear();

            StringBuilder mess = new StringBuilder();
            mess.AppendFormat("{0} Pax", gridMain.VisibleRowCount);
            foreach (KeyValuePair<int?, List<int>> keyValue in dic)
            {
                if (keyValue.Value.Count == 1)
                {
                    mess.AppendFormat("\r\npax {0} visa {1}.", keyValue.Value[0], GetStatus(keyValue.Key));
                    messages.Add(new visa.CLEINT.Excel.MessageInfo(String.Format("pax {0} visa {1}.", keyValue.Value[0], GetStatus(keyValue.Key)), keyValue.Key));
                }
                else if (keyValue.Value.Count > 1)
                {
                    mess.AppendFormat("\r\npax {0} - {1} visa {2}.", keyValue.Value[0], keyValue.Value[keyValue.Value.Count - 1], GetStatus(keyValue.Key));
                    messages.Add(new visa.CLEINT.Excel.MessageInfo(String.Format("pax {0} - {1} visa {2}.", keyValue.Value[0], keyValue.Value[keyValue.Value.Count - 1], GetStatus(keyValue.Key)), keyValue.Key));
                }
            }
            txtMessageInfo.AppendText(mess.ToString()) ;


        }


        public static string GetStatus(int? iType)
        {
            switch (iType)
            {
                case 1:
                    return "DL 1 thang 1 lan";
                case 2:
                    return "DL 1 thang nhieu lan";
                case 3:
                    return "thuong mai/dau tu 1 thang 1 lan";
                case 4:
                    return "others 1 thang 1 lan";
                case 5:
                    return "others 1 thang nhieu lan";
                case 6:
                    return "thuong mai/tham than/dau tu 3 thang 1 lan";
                case 7:
                    return "others 3 thang 1 lan";
                case 8:
                    return "thuong mai/tham than/dau tu/duhoc 3 thang nhieu lan";
                case 9:
                    return "others 3 thang nhieu lan";
                case 10:
                    return "others 6 thang 1 lan";
                case 11:
                    return "thuong mai 6 thang nhieu lan";
                case 12:
                    return "others 6 thang nhieu lan";
                case 13:
                    return "others 12 thang 1 lan";
                case 14:
                    return "others 12 thang nhieu lan";
                default:
                    return "others";
            }
        }

    }
}
