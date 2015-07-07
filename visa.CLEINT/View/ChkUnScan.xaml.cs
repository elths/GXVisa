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
    /// ChkUnScan.xaml 的交互逻辑
    /// </summary>
    public partial class ChkUnScan : UserControl
    {
        ObservableCollection<Customer> CustomerObjs = new ObservableCollection<Customer>();

        string FAutoID = "";

        visaEntities visaORM = new visaEntities();

        List<Customer> customerObjs = new List<Customer>();

        public ChkUnScan()
        {
            InitializeComponent();
            gridMain.DataContext = visaORM.Customer.Where(c=>c.FSysPut==true&&c.FSysSend==false);
        }


        public ChkUnScan(ObservableCollection<Customer> ScannedCustomers)
        {
            InitializeComponent();

            FAutoID = ScannedCustomers.First().FAutoID;
            TB_TableSubmit submitObj = visaORM.TB_TableSubmit.FirstOrDefault(tbs => tbs.FAutoID == FAutoID);
            if (submitObj==null)
            {
                MessageBox.Show("该签证尚未提交，请先提交");
                return;
            }
            string submitNo = submitObj.FSubmitNo;

            customerObjs = (from c in visaORM.Customer
                           join k in visaORM.TB_TableSubmit.Where(tbs => tbs.FSubmitNo == submitNo)
                           on c.FAutoID equals k.FAutoID
                           select c).ToList();

            InitDataContext(ScannedCustomers);

            gridMain.DataContext = customerObjs;
        }

        void InitDataContext(ObservableCollection<Customer> ScannedCustomers)
        {

            foreach (Customer cObj in ScannedCustomers)
            {
                Customer tempObj = customerObjs.FirstOrDefault(t => t.FAutoID == cObj.FAutoID);
                if (tempObj!=null)
                    customerObjs.Remove(tempObj);
            }

        }
    }
}
