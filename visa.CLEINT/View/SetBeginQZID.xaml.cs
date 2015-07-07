using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
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
    /// SetBeginQZID.xaml 的交互逻辑
    /// </summary>
    public partial class SetBeginQZID : UserControl
    {
        visaEntities visaORM = new visaEntities();

        private string setQZIDnum;
        public string SetQZIDnum
        {
            get
            {
                return setQZIDnum;
            }
        }

        private string setQZIDstring;
        public string SetQZIDstring
        {
            get
            {
                return setQZIDstring;
            }
        }

        DXDialog dg;

        public SetBeginQZID()
        {
            InitializeComponent();
            this.Loaded += SetBeginQZID_Load;
        }


        private void SetBeginQZID_Load(Object sender, EventArgs e)
        {

            dg = this.Parent as DXDialog;
            if (dg != null)
            {
                dg.OkButton.Visibility = Visibility.Hidden;
                dg.CancelButton.Visibility = Visibility.Hidden;

            }

        }

        private void btnQZIDString_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstLetter.Text) || string.IsNullOrEmpty(txtQZIDNum.Text))
            {
                MessageBox.Show("请填写前缀字母和签证号码");
                return;
            }

            string QZIDstring = txtFirstLetter.Text+txtQZIDNum.Text;
            var model = visaORM.QZKC.FirstOrDefault(n => n.FQZID == QZIDstring);
            if (model==null)
            {
                MessageBox.Show(string.Format("没有{0}签证号", QZIDstring));
                return;
            }
            else
            {
                if (model.FIsUse==true)
                {
                    MessageBox.Show(string.Format("签证号{0}已经被使用", QZIDstring));
                return;
            }

            }

            if (dg != null)
          {

              setQZIDnum = txtQZIDNum.Text;
              setQZIDstring = txtFirstLetter.Text;
              MessageBox.Show("设置成功");

              dg.Close();
          }
            
        }

        private void btnQZIDNum_Click(object sender, RoutedEventArgs e)
        {
            if (dg != null)
            {
                dg.Close();
            }

        }

    }
}
