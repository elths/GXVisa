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

namespace visa.CLEINT.View
{
    /// <summary>
    /// Vietnam.xaml 的交互逻辑
    /// </summary>
    public partial class CountryAdmin : UserControl
    {
        visaEntities visaORM = new visaEntities();
        ObservableCollection<TCountry> countryCollection = new ObservableCollection<TCountry>();
        TCountry model = new TCountry();

        public CountryAdmin()
        {
            InitializeComponent();
            InitializeViewModel();

            InitData();
            MainGrid.DataContext = countryCollection;
        }

        void InitData()
        {
            countryCollection.Clear();
            foreach (var countryObj in visaORM.TCountry)
            {
                countryCollection.Add(countryObj);
            }

            EditGrid.DataContext = model;

        }

        private void InitializeViewModel()
        {
            ViewModel.ViewModel viewModel = new ViewModel.ViewModel();
            BarModel clipboardBar = new BarModel() { Name = "工具栏" };


            viewModel.Bars.Add(clipboardBar);

            MyCommand addCommand = new MyCommand(NewModel)
            {
                Caption = "新增",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand delCommand = new MyCommand(DelModel)
            {
                Caption = "删除",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand saveCommand = new MyCommand(SaveModel)
            {
                Caption = "保存",
                LargeGlyph = null,
                SmallGlyph = null
            };


            clipboardBar.Commands.Add(addCommand);
            clipboardBar.Commands.Add(delCommand);
            clipboardBar.Commands.Add(saveCommand);

            DataContext = viewModel;

        }

        void NewModel()
        {
            model = new TCountry();
            EditGrid.DataContext = model;

        }

        void DelModel()
        {
            if (MainGrid.SelectedItem == null)
            {
                MessageBox.Show("请选择一行");
                return;
            }

            if (MessageBox.Show("是否确认删除?", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                model = MainGrid.SelectedItem as TCountry;
                visaORM.DeleteObject(model);
                countryCollection.Remove(model);

            }
        }

        void SaveModel()
        {
            MainBar.Focus();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("请填写国家英文名");
                return;
            }

            if (string.IsNullOrEmpty(txtEnName.Text))
            {
                MessageBox.Show("请填写国家英文名");
                return;
            }




            if (model.FID == 0 || model.FID.ToString() == "")
            {

                visaORM.TCountry.AddObject(model);

                visaORM.SaveChanges();
                countryCollection.Add(model);
            }
            else
            {

                visaORM.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                visaORM.SaveChanges();
            }
            MessageBox.Show("保存成功");

        }

        private void MainGrid_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null)
                return;

            model = e.NewItem as TCountry;
            EditGrid.DataContext = model;

        }

        private void EmptyValidate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null) return;
            e.IsValid = !String.IsNullOrEmpty(e.Value.ToString());
            e.ErrorContent = "必须填写该项内容";

        }



    }
}
