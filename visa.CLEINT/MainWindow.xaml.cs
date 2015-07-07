using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using visa.MODEL;


namespace visa.CLEINT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        visaEntities visaORM = new visaEntities();

        ObservableCollection<TB_Menu> menuCollection = new ObservableCollection<TB_Menu>();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            initMenuTree();
            this.Title = "签证系统 v2.0 ---" + "【当前用户：" + MainContext.UserName + " | 当前单位：" + MainContext.UserCompanyName + "】";
        }


        /// <summary>
        /// 初始化导航
        /// </summary>
        private void initMenuTree()
        {
            try
            {
                TB_Menu menuObj;
                foreach (var urObj in visaORM.TB_UserRights.Where(ur => ur.FUserID == MainContext.UserID && ur.FStatus == true))
                {
                    menuObj = visaORM.TB_Menu.FirstOrDefault(m => m.FID == urObj.FMenuID && m.FType == 1 && m.FStatus == true);
                    if (menuObj != null && menuCollection.Contains(menuObj)==false) { menuCollection.Add(menuObj); }
                }

                this.MenuTree.ItemsSource = menuCollection;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }
        }

        private void MenuTree_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null)
                return;
            TB_Menu menuModel = e.NewItem as TB_Menu;
            if (menuModel.FNavTo != null)
            {
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                    object obj = assembly.CreateInstance("visa.CLEINT.View." + menuModel.FNavTo);

                    if (obj != null)
                    {
                        MainFrame.Navigate(obj);
                    }
                    else
                    {
                        MessageBox.Show("该界面不存在");
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.WriteLog.WriteErorrLog(ex);
                }

            }
        }


        /// <summary>
        /// 用户管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.UserAdmin(),
                Title = "用户管理"
            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }

        /// <summary>
        /// 数据备份还原事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bBackup_Click(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.DataBackup(),
                Title = "数据备份还原"
            };
            dg.Width = 500;
            dg.Height = 500;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }


        /// <summary>
        /// 用户管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bPort_Click(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.PortAdmin(),
                Title = "端口管理"
            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }



        private void bVietnam_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.Vietnam()

            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }


        private void bAuditUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.AuditUser()

            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }


        private void bAuditPosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.AuditPosition()

            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }

        private void bCountry_ItemClick(object sender, ItemClickEventArgs e)
        {
            DXDialog dg = new DXDialog()
            {
                Content = new View.CountryAdmin()

            };
            dg.Width = 700;
            dg.Height = 750;
            dg.HorizontalAlignment = HorizontalAlignment.Center;
            dg.VerticalAlignment = VerticalAlignment.Center;
            dg.ShowDialog(MessageBoxButton.OK);
        }


        private void bQuit_Click(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否退出系统?", "退出系统", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void MenuTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TB_Menu menuModel = MenuTree.CurrentItem as TB_Menu;
            if (menuModel.FNavTo != null)
            {
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                    object obj = assembly.CreateInstance("visa.CLEINT.View." + menuModel.FNavTo);

                    if (obj != null)
                    {
                        MainFrame.Navigate(obj);
                    }
                    else
                    {
                        MessageBox.Show("该界面不存在");
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
}
