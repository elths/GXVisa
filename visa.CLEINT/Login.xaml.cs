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
using System.Windows.Shapes;
using visa.MODEL;

namespace visa.CLEINT
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        visaEntities visaORM = new visaEntities();

        public Login()
        {
            try
            {
                InitializeComponent();

                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                cbCompany.ItemsSource = visaORM.User.Where(u => u.FIsDelete != true).Select(u => u.FCompanyName).Distinct();
            }
            catch (System.Exception ex)
            {
                Log.WriteLog.WriteErorrLog(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCompany.SelectedItem == null)
                {
                    return;
                }

                string companyName = cbCompany.SelectedItem.ToString();

                var model = visaORM.User.FirstOrDefault(u => u.FUserName == txtUserName.Text && u.FPassword == txtPassWord.Text && u.FIsDelete != true);

                if (model == null)
                {
                    wordLoginError.Visibility = Visibility.Visible;
                    return;
                }
                this.Hide();
                MainContext.UserID = model.FID;
                MainContext.UserPhoneNo = model.FPhoneNo;
                MainContext.UserCompanyName = model.FCompanyName;
                MainContext.UserWorkCompanyName = model.FWorkCompanyName;
                MainContext.UserName = model.FUserName;
                MainContext.UserPhoneNo = model.FPhoneNo;
                MainWindow mw = new MainWindow();

                mw.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void cbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string CompanyNameString = "";
            if (cbCompany.SelectedItem == null)
            {
                return;
            }
            else
            {
                CompanyNameString = cbCompany.SelectedItem.ToString();
            }
            User userObj = visaORM.User.FirstOrDefault(u => u.FCompanyName == CompanyNameString);
            txtUserName.Text = userObj.FUserName;
        }


        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }

    }
}
