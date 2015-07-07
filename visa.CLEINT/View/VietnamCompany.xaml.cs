﻿using System;
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
    public partial class Vietnam : UserControl
    {
        visaEntities visaORM = new visaEntities();
        ObservableCollection<TB_VietnamCompany> vietnamCollection = new ObservableCollection<TB_VietnamCompany>();
        TB_VietnamCompany model = new TB_VietnamCompany();

        public Vietnam()
        {
            InitializeComponent();
            InitializeViewModel();

            InitData();
        }

        void InitData()
        {
            foreach (var vietnamObj in visaORM.TB_VietnamCompany.Where(v=>v.FStatus==true))
            {
                vietnamCollection.Add(vietnamObj);
            }

            EditGrid.DataContext = model;

            MainGrid.DataContext = vietnamCollection;
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

        void NewModel() {
            model = new TB_VietnamCompany();
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
                model =MainGrid.SelectedItem as TB_VietnamCompany;
                model.FStatus = false;
                visaORM.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                visaORM.SaveChanges();
                vietnamCollection.Remove(model);

            }     
        }

        void SaveModel()
        {
            MainBar.Focus();

            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("请填写公司全称");
                return;
            }

            if (string.IsNullOrEmpty(txtShortName.Text))
            {
                MessageBox.Show("请填写公司简写");
                return;
            }



            model.FModifyDate = DateTime.Now;
            model.FModifyUser = MainContext.UserID;


            if (model.FID == 0 || model.FID.ToString() == "")
            {

                visaORM.TB_VietnamCompany.AddObject(model);
                model.FCreateDate = DateTime.Now;
                model.FCreateUser = MainContext.UserID;
                model.FStatus = true;

                visaORM.SaveChanges();
                vietnamCollection.Add(model);
            }
            else
            {



                visaORM.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                visaORM.SaveChanges();
            }
            MessageBox.Show("保存成功");
                        MainGrid.DataContext = vietnamCollection;

        }

        private void MainGrid_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null)
                return;

            model =   e.NewItem as TB_VietnamCompany;
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
