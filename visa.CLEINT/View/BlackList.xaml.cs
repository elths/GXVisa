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
using DevExpress.Xpf.Mvvm;
using visa.CLEINT.Model;
using visa.CLEINT.Common;
using visa.MODEL;
using System.ComponentModel;
using DevExpress.Xpf.Editors;
using System.Collections.ObjectModel;

namespace visa.CLEINT.View
{
    /// <summary>
    /// VisaEdit.xaml 的交互逻辑
    /// </summary>
    public partial class BlackList : UserControl
    {

        public CustomerT model = new CustomerT();
        visaEntities visaORM = new visaEntities();
        List<TCountry> countryModel = new List<TCountry>();

        List<CustomerT> customerList = new List<CustomerT>();

        ObservableCollection<CustomerT> customerCollection = new ObservableCollection<CustomerT>();

        /// <summary>
        /// 性别数据源
        /// </summary>
        List<String> sourceSex = new List<String>();

        /// <summary>
        /// 护照类型
        /// </summary>
        List<String> sourcePassportType = new List<String>();


        /// <summary>
        /// 办理速度
        /// </summary>
        List<String> sourceSpeedType = new List<String>();

        /// <summary>
        /// 出境目的
        /// </summary>
        List<String> sourcePurpose = new List<String>();


        //public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 是否更新结束时间
        /// </summary>
        private bool isCanChangeTime = true;


        //private void NotifyPropertyChanged(Customer custModel)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(custModel));
        //    }
        //}

        public BlackList()
        {


            InitializeComponent();
            InitializeViewModel();
            InitCustomerModel();

            InitData();
            LeftGrid.DataContext = customerCollection;

            InitSource();
        }


        void InitSource()
        {

            sourcePassportType.Add("P");
            sourcePassportType.Add("O");
            sourcePassportType.Add("D");
            cbPassportType.ItemsSource = sourcePassportType;

            //sourceSpeedType.Add("N+");
            //sourceSpeedType.Add("N-");
            //sourceSpeedType.Add("O");
            //cbSpeedType.ItemsSource = sourceSpeedType;

        }

        private void InitCustomerModel()
        {
            MainTab.DataContext = model;
            InitSelectValue();
        }

        private void InitSelectValue()
        {
            model.FBirthNationlity = "中国";
            model.FBirthNationlityEn = "China";
            model.FBirthNationlityPresent = "中国";
            model.FBirthNationlityPresentEn = "China";
        }

        private void InitData()
        {

            customerList = visaORM.CustomerT.ToList();
            foreach (var item in customerList)
            {
                customerCollection.Add(item);
            }
            //LeftGrid.ItemsSource = customerList;

            //出生地
            cbBirthPlace.ItemsSource = visaORM.IDC.Select(i => i.FDQ).Distinct();


            countryModel = visaORM.TCountry.ToList();


            var userList = visaORM.User.ToList();
            cbCreateUser.ItemsSource = userList;
            cbModifyUser.ItemsSource = userList;

        }


        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);
            MyCommand prevCommand = new MyCommand(CustomerPrev)
            {
                Caption = "上一份",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand nextCommand = new MyCommand(CustomerNext)
            {
                Caption = "下一份",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand addCommand = new MyCommand(CustomerAdd)
            {
                Caption = "新增",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand delCommand = new MyCommand(CustomerDel)
            {
                Caption = "删除",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand saveCommand = new MyCommand(CustomerSave)
            {
                Caption = "保存",
                LargeGlyph = null,
                SmallGlyph = null
            };



            //MyCommand copyCommand = new MyCommand(null) { Caption = "Copy", LargeGlyph = GlyphHelper.GetGlyph("/Images/Icons/Copy_32x32.png"), SmallGlyph = GlyphHelper.GetGlyph("/Images/Icons/Copy_16x16.png") };

            clipboardBar.Commands.Add(prevCommand);
            clipboardBar.Commands.Add(nextCommand);
            clipboardBar.Commands.Add(addCommand);
            clipboardBar.Commands.Add(delCommand);
            clipboardBar.Commands.Add(saveCommand);

            //MyGroupCommand addGroupCommand = new MyGroupCommand() { Caption = "Add", LargeGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_32x32.png"), SmallGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_16x16.png") };
            //MyParentCommand parentCommand = new MyParentCommand(viewModel, MyParentCommandType.CommandCreation) { Caption = "Add Command", LargeGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_32x32.png"), SmallGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_16x16.png") };
            //MyParentCommand parentBar = new MyParentCommand(viewModel, MyParentCommandType.BarCreation) { Caption = "Add Bar", LargeGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_32x32.png"), SmallGlyph = GlyphHelper.GetGlyph("/Images/Icons/Add_16x16.png") };
            //addGroupCommand.Commands.Add(parentCommand);
            //addGroupCommand.Commands.Add(parentBar);
            //addingBar.Commands.Add(addGroupCommand);
            //addingBar.Commands.Add(parentCommand);
            //addingBar.Commands.Add(parentBar);

            DataContext = viewModel;
        }


        private void printSingle()
        {
            (App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1());
        }

        private void CustomerAdd()
        {
            try
            {
                this.model = new CustomerT();
                MainTab.DataContext = this.model;
                InitSelectValue();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }
        }

        private void CustomerSave()
        {
            try
            {
                this.MainBar.Focus();

                if (!ValidateControl(txtNameEn, "英文姓名")) return;
                if (!ValidateControl(txtPassportNo, "护照号码")) return;

                this.model.FModifyDate = DateTime.Now;
                this.model.FModifyUser = MainContext.UserID;


                if (this.model.FID == 0 || this.model.FID.ToString() == "")
                {

                    visaORM.CustomerT.AddObject(this.model);
                    this.model.FCreateDate = DateTime.Now;
                    this.model.FCreateUser = MainContext.UserID;

                    visaORM.SaveChanges();
                    //InitData();
                    customerCollection.Add(model);
                    this.LeftGrid.SelectedItem = model;
                }
                else
                {

                    visaORM.ObjectStateManager.ChangeObjectState(this.model, System.Data.EntityState.Modified);
                    visaORM.SaveChanges();
                }
                MessageBox.Show("保存成功");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }
        }

        bool ValidateControl(Control control, string messageString)
        {
            if (control is TextEdit)
            {
                if (string.IsNullOrEmpty((control as TextEdit).Text))
                {
                    MessageBox.Show("请填写" + messageString);
                    control.Focus();
                    return false;
                }
                return true;
            }
            if (control is ComboBoxEdit)
            {
                if ((control as ComboBoxEdit).SelectedIndex == -1)
                {
                    MessageBox.Show("请填写" + messageString);
                    control.Focus();
                    return false;
                }
                return true;
            }
            if (control is ComboBox)
            {
                if ((control as ComboBox).SelectedIndex == -1)
                {
                    MessageBox.Show("请填写" + messageString);
                    control.Focus();
                    return false;
                }
                return true;
            }
            //MessageBox.Show("请填写" + messageString);
            return true;


        }

        private void CustomerDel()
        {
            CustomerT delObj = visaORM.CustomerT.FirstOrDefault(c => c.FID == model.FID);
            if (delObj == null)
            {
                MessageBox.Show("该数据尚未保存，无需删除");
                return;
            }
            else
            {
                if (MessageBox.Show("是否确认删除?", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    visaORM.DeleteObject(delObj);
                    visaORM.SaveChanges();
                    customerCollection.Remove(delObj);
                    this.model = LeftGrid.SelectedItem as CustomerT;
                    if (this.model == null)
                        this.model = new CustomerT();
                }
            }
        }

        private void LeftGrid_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null || e.NewItem == null)
                return;

            CustomerT custModel = e.NewItem as CustomerT;


            var tempModel = visaORM.CustomerT.FirstOrDefault(c => c.FID == custModel.FID);
            if (tempModel == null)
                return;
            else
            {
                this.model = tempModel;
                isCanChangeTime = false;
                MainTab.DataContext = this.model;

            }

        }

        /// <summary>
        /// 左侧表单双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            CustomerT custModel = LeftGrid.CurrentItem as CustomerT;

            var tempModel = visaORM.CustomerT.FirstOrDefault(c => c.FID == custModel.FID);
            if (tempModel == null)
                return;
            else
            {
                this.model = tempModel;
                isCanChangeTime = false;
                MainTab.DataContext = this.model;

            }

        }


        private void ComboBoxEditItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void CustomerPrev()
        {
            LeftView.MovePrevRow();
        }

        private void CustomerNext()
        {
            LeftView.MoveNextRow();
        }

        private void EmptyValidate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null) return;
            e.IsValid = !String.IsNullOrEmpty(e.Value.ToString());
            e.ErrorContent = "必须填写该项内容";

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) && string.IsNullOrEmpty(txtPassportNoForSearch.Text))
            {
                MessageBox.Show("请填写条件");
                return;
            }

            if (!string.IsNullOrEmpty(txtName.Text))
            {
                CustomerT tempObj = visaORM.CustomerT.FirstOrDefault(c => c.FName == txtName.Text || c.FNameEn == txtName.Text);
                if (tempObj != null)
                {
                    model = tempObj;
                    MainTab.DataContext = model;

                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtPassportNoForSearch.Text))
            {
                CustomerT tempObj = visaORM.CustomerT.FirstOrDefault(c => c.FPassportNo == txtPassportNoForSearch.Text);
                if (tempObj != null)
                {
                    model = tempObj;
                    MainTab.DataContext = model;

                    return;
                }
            }
            MessageBox.Show("找不到对应签证");

        }



    }

}
