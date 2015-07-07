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
using visa.CLEINT.Model;
using visa.MODEL;
using System.Collections.ObjectModel;
using System.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;


namespace visa.CLEINT.View
{
    /// <summary>
    /// UserAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class UserAdmin : UserControl
    {
        visaEntities visaORM = new visaEntities();
        public User model = new User();
        int NowUserId = MainContext.UserID;

        /// <summary>
        /// 菜单树
        /// </summary>
        List<TB_Menu> menuList = new List<TB_Menu>();


        /// <summary>
        /// 菜单树中选择的权限
        /// </summary>
        ObservableCollection<TB_Menu> selectedMenu = new ObservableCollection<TB_Menu>();


        /// <summary>
        /// 左侧表格的数据源
        /// </summary>
        ObservableCollection<User> userList = new ObservableCollection<User>();


        /// <summary>
        /// 保存后是否触发左侧取值事件
        /// </summary>
        int getDefaultModel = 1;


        public UserAdmin()
        {
            InitializeComponent();
            InitializeViewModel();
            InituserList();
            //InitmenuList();
            visa.CLEINT.ViewModel.UserAdminVM menuVM = new visa.CLEINT.ViewModel.UserAdminVM();
            MenuTree.DataContext = menuVM;
            MenuTree.ItemsSource = menuVM.MenuList;
            InitData();
            InitOtherMenu();

            LeftGrid.DataContext = userList;


            if (MainContext.UserCompanyName.ToUpper() != "ADMIN")
            {
                MenuTree.Visibility = Visibility.Hidden;
            }


        }

        void InituserList()
        {
            foreach (User u in visaORM.User.Where(u => u.FIsDelete == false))
            {
                userList.Add(u);
            }
        }


        void InitOtherMenu()
        {
            //Checkcb(cb22,22, model.FID);
            //Checkcb(cb23, 23, model.FID);
            //Checkcb(cb24, 24, model.FID);
            //Checkcb(cb25, 25, model.FID);
            //Checkcb(cb26, 26, model.FID);


        }

        void Checkcb(CheckBox cbControl, int menuID, int userID)
        {
            TB_UserRights ur = visaORM.TB_UserRights.FirstOrDefault(u => u.FMenuID == menuID && u.FUserID == userID&&u.FStatus==true);
            if (ur != null)
            {
                cbControl.IsChecked = true;
            }
        }


        //void InitmenuList()
        //{
        //    menuList = visaORM.TB_Menu.Where(m=>m.FStatus==true).ToList();
        //}


        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };

            viewModel.Bars.Add(clipboardBar);


            MyCommand addCommand = new MyCommand(UserAdd)
            {
                Caption = "新增",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand delCommand = new MyCommand(UserDel)
            {
                Caption = "删除",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand saveCommand = new MyCommand(UserSave)
            {
                Caption = "保存",
                LargeGlyph = null,
                SmallGlyph = null
            };

            if (MainContext.UserCompanyName.ToUpper() == "ADMIN")
            {
            clipboardBar.Commands.Add(addCommand);
            clipboardBar.Commands.Add(delCommand);
            }
            clipboardBar.Commands.Add(saveCommand);
            DataContext = viewModel;
        }

        private void UserAdd()
        {
            //wordPassConfirm.Visibility = Visibility.Visible;
            //wordPassConfirmInfo.Visibility = Visibility.Visible;
            //txtPassConfirm.Visibility = Visibility.Visible;
            model = new User();
            MainGrid.DataContext = model;
            txtCompanyName.IsEnabled = true;
            txtPassConfirm.Text = "";
        }

        private void UserDel()
        {

          if( DXMessageBox.Show("是否确定删除?", "删除确认", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
          {
              User tempModel = LeftGrid.SelectedItem as User;
              if (tempModel == null) return;
              model = tempModel;
              model.FIsDelete = true;
              visaORM.ObjectStateManager.ChangeObjectState(model, EntityState.Modified);
              visaORM.SaveChanges();
              userList.Remove(model);
              InitData();

          }
        }

        private void UserSave()
        {
            MainBar.Focus();

            


                if (txtPassConfirm.Text != txtPassword.Text)
                {
                    MessageBox.Show("两次输入的密码不一致，保存失败。");
                    return;
              }

            if (model.FUserName == null || model.FCompanyName == null )
            {
                    MessageBox.Show("必须录入用户名和公司。");
                    return;
                }



            if (model.FID == 0 || model.FID.ToString() == "")
            {
                var tempObj = visaORM.User.FirstOrDefault(u => u.FUserName == model.FUserName);
                {
                    if (tempObj != null)
                    {
                        MessageBox.Show("该用户名已经存在，请使用其他用户名");
                        return;
                    }
                }

                visaORM.User.AddObject(model);
                model.FIsDelete = false;


                visaORM.SaveChanges();
                NowUserId = model.FID;

                userList.Add(model);

                //getDefaultModel = 0;

                InitData();
            }
            else
            {

                if (visaORM.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).Count() == 0)
                {
                    visaORM.User.Attach(model);

                }
                visaORM.ObjectStateManager.ChangeObjectState(model, EntityState.Modified);
                visaORM.SaveChanges();
                NowUserId = model.FID;

            }


                var list = from m in visaORM.TB_Menu
                           join ur in visaORM.TB_UserRights
                           on m.FID equals ur.FMenuID
                           where m.FType==1 && ur.FUserID==model.FID
                           select ur;


                foreach (TB_UserRights urObj in list)
                {
                    visaORM.DeleteObject(urObj);
                }

                selectedMenu.Clear();

                foreach (var nodeObjLevel1 in MenuTree.View.Nodes)
                {
                    foreach (var nodeObjLevel2 in nodeObjLevel1.Nodes)
                    {
                        foreach (var nodeObjLevel3 in nodeObjLevel2.Nodes)
                        {
                            if (nodeObjLevel3.IsChecked==true)
                            {
                                selectedMenu.Add(nodeObjLevel3.Content as TB_Menu);
                            }

                        }
                        if (nodeObjLevel2.IsChecked == true)
                        {
                            selectedMenu.Add(nodeObjLevel2.Content as TB_Menu);
                        }

                    }
                    if (nodeObjLevel1.IsChecked == true)
                    {
                        selectedMenu.Add(nodeObjLevel1.Content as TB_Menu);
                    }


                }


                TB_UserRights urModel;
                foreach (var menuObj in selectedMenu)
                {
                    urModel = new TB_UserRights();
                    urModel.FUserID = model.FID;
                    urModel.FMenuID = menuObj.FID;
                    urModel.FCreateDate = DateTime.Now;
                    urModel.FCreateUser = MainContext.UserID;
                    urModel.FModifyDate = DateTime.Now;
                    urModel.FModifyUser = MainContext.UserID;
                    urModel.FStatus = true;

                    visaORM.TB_UserRights.AddObject(urModel);

                }

                TB_UserRights outMenuObj;
                if (CreateURModel( cbTable2Admin,out outMenuObj)==true)
                {
                    if (outMenuObj != null) visaORM.TB_UserRights.AddObject(outMenuObj);
                }
                else
                {
                    if (outMenuObj != null) visaORM.DeleteObject(outMenuObj);
                }

                visaORM.SaveChanges();


            InitData();

            MessageBox.Show("保存成功");


        }

        /// <summary>
        /// 判断复选框是否勾选，判断是否已经有数据
        /// </summary>
        /// <param name="menuCheckEdit">菜单复选框</param>
        /// <param name="urModel"></param>
        /// <returns></returns>
        bool CreateURModel(CheckEdit menuCheckEdit, out TB_UserRights urModel)
        {
            urModel = null;
            string rightString = menuCheckEdit.Name.Replace("cb", "");
            TB_Menu mObj = visaORM.TB_Menu.FirstOrDefault(m => m.FName == rightString);
            if (mObj == null)
            {
                DXMessageBox.Show(rightString + "已在数据中被删除");
                return true;
            }

            if (menuCheckEdit.IsChecked == true)
            {

                TB_UserRights urObj = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == mObj.FID && ur.FStatus == true && ur.FUserID == MainContext.UserID);
                if (urObj!=null)
                {
                    return true;
                }

                urModel = new TB_UserRights();

                urModel.FUserID = model.FID;
                urModel.FMenuID = mObj.FID;
                urModel.FCreateDate = DateTime.Now;
                urModel.FCreateUser = MainContext.UserID;
                urModel.FModifyDate = DateTime.Now;
                urModel.FModifyUser = MainContext.UserID;
                urModel.FStatus = true;

                return true;
            }
            else
            {

                urModel = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == mObj.FID && ur.FUserID == MainContext.UserID);
                return false;
            }



        }




        void InitData()
        {
            model = visaORM.User.FirstOrDefault(u => u.FID == NowUserId);
            if (model == null) return;


            if (MainContext.UserCompanyName.ToUpper() == "ADMIN")
            {
                //customerList = visaORM.User.ToList();
                //LeftGrid.DataContext = customerList;

                if (model.FCompanyName.ToUpper() == "ADMIN")
                txtCompanyName.IsEnabled = false;

            }
            else
            {
                LeftGrid.ItemsSource = visaORM.User.FirstOrDefault(u=>u.FID==MainContext.UserID);
            }


            if (model != null)
            {
                MainGrid.DataContext = model;

                txtPassConfirm.Text = model.FPassword;
            }

            InitTreeNodesCheck();

            //初始化其他权限
            InitOtherRights();
            
        }

        /// <summary>
        /// 初始化其他权限
        /// </summary>
        void InitOtherRights()
        {
            if (SetCheckEdit("Table2Admin") == true) cbTable2Admin.IsChecked = true;
            else cbTable2Admin.IsChecked = false;
        }

        bool SetCheckEdit(string menuString)
        {
            TB_Menu menuObj = visaORM.TB_Menu.FirstOrDefault(m => m.FName == menuString && m.FStatus == true);
            if (menuObj == null) return false;

            TB_UserRights urObj = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == menuObj.FID && ur.FUserID == this.model.FID&&ur.FStatus==true);
            if (urObj != null) return true;
            else return false;
        }

        void InitTreeNodesCheck()
        {
            foreach (var nodeObjLevel1 in MenuTree.View.Nodes)
            {
                foreach (var nodeObjLevel2 in nodeObjLevel1.Nodes)
                {
                    foreach (var nodeObjLevel3 in nodeObjLevel2.Nodes)
                    {
                        int menuIDLevel3 = (nodeObjLevel3.Content as TB_Menu).FID;
                        var tempURObjLevel3 = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == menuIDLevel3&&ur.FUserID==model.FID);
                        if (tempURObjLevel3 != null) nodeObjLevel3.IsChecked = true;
                        else nodeObjLevel3.IsChecked = false;
                    }
                    int menuIDLevel2 = (nodeObjLevel2.Content as TB_Menu).FID;
                    var tempURObjLevel2 = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == menuIDLevel2 && ur.FUserID == model.FID);
                    if (tempURObjLevel2 != null) nodeObjLevel2.IsChecked = true;
                    else nodeObjLevel2.IsChecked = false;

                }
                int menuIDLevel1 = (nodeObjLevel1.Content as TB_Menu).FID;
                var tempURObjLevel1 = visaORM.TB_UserRights.FirstOrDefault(ur => ur.FMenuID == menuIDLevel1 && ur.FUserID == model.FID);
                if (tempURObjLevel1 != null) nodeObjLevel1.IsChecked = true;
                else nodeObjLevel1.IsChecked = false;

            }

            selectedMenu.Clear(); //初始化后清除选择的集合

            foreach (var urObj in visaORM.TB_UserRights.Where(ur => ur.FUserID == model.FID))
            {
                TB_Menu menuObj = visaORM.TB_Menu.FirstOrDefault(m=>m.FID == urObj.FMenuID);
                if (menuObj!=null) selectedMenu.Add(menuObj);
            }
        }



        private void LeftGrid_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null || e.NewItem == null)
                return;

            //if (getDefaultModel == 0)
            //{
            //    getDefaultModel = 1;
            //    return;
            //}

            User userModel = e.NewItem as User;


            var tempModel = visaORM.User.FirstOrDefault(c => c.FID == userModel.FID);
            if (tempModel == null)
                return;
            else
            {
                model = tempModel;
                MainGrid.DataContext = model;
                txtPassConfirm.Text = model.FPassword;

            }
            if (model.FCompanyName.ToUpper() == "ADMIN")
            {
                txtCompanyName.IsEnabled = false;
            }
            InitTreeNodesCheck();
            //初始化其他权限
            InitOtherRights();

        }

        private void EmptyValidate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {

            e.IsValid = !String.IsNullOrEmpty(e.Value.ToString());
            e.ErrorContent = "必须填写该项内容";

        }

        private void LeftView_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked == true)
            {
                selectedMenu.Add(e.Node.Content as TB_Menu);
            }
            else
            {
                selectedMenu.Remove(e.Node.Content as TB_Menu);
            }
        }

        DXDialog dg;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dg = this.Parent as DXDialog;
            if (dg != null)
            {
                dg.OkButton.Visibility = Visibility.Hidden;

            }

        }




    }
}
