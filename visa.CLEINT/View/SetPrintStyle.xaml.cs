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
    /// SetPrintStyle.xaml 的交互逻辑
    /// </summary>
    public partial class SetPrintStyle : UserControl
    {
        visaEntities visaORM = new visaEntities();
        int gourpIDAll;

        public SetPrintStyle()
        {
            InitializeComponent();
        }

           public SetPrintStyle(int gourpID)
        {
            InitializeComponent();
            gourpIDAll = gourpID;
            InitData(gourpIDAll);
        }

      void InitData(int gourpID)
        {
            List<TB_Position> pList = visaORM.TB_Position.Where(p => p.FGroupID == gourpID&&p.FStatus==true).ToList();

            foreach (var p in pList)
          {
              switch (p.FName)
              {
                  case "Type":
                      this.GridType.DataContext = p;
                      break;
                  case "DDate":
                      this.GridDDate.DataContext = p;
                      break;
                  case "PDate":
                      this.GridPDate.DataContext = p;
                      break;
                  case "MDate":
                      this.GridMDate.DataContext = p;
                      break;
                  case "NamNimh":
                      this.GridNamNimh.DataContext = p;
                      break;
                  case "UserName":
                      this.GridUserName.DataContext = p;
                      break;
                  case "PID":
                      this.GridPID.DataContext = p;
                      break;
                  case "Duty":
                      this.GridDuty.DataContext = p;
                      break;
                  case "Times":
                      this.GridTimes.DataContext = p;
                      break;
                  case "Memo":
                      this.GridMemo.DataContext = p;
                      break;
                  case "PageSize":
                      this.GridPageSize.DataContext = p;
                      break;
                  case "VID":
                      this.GridVID.DataContext = p;
                      break;
                  case "BDate":
                      this.GridBDate.DataContext = p;
                      break;
                  case "CustomerName":
                      this.GridCustumerName.DataContext = p;
                      break;
                  case "Country":
                      this.GridCountry.DataContext = p;
                      break;
                  default:
                      break;
              }

          }
        }

      private void btnClose_Click(object sender, RoutedEventArgs e)
      {
          dg.Close();

      }

      private void btnSave_Click(object sender, RoutedEventArgs e)
      {
          List<TB_Position> pList = visaORM.TB_Position.Where(p => p.FGroupID == gourpIDAll && p.FStatus == true).ToList();

          foreach (var p in pList)
          {
              visaORM.ObjectStateManager.ChangeObjectState(p, System.Data.EntityState.Modified);

          }
          visaORM.SaveChanges();
          MessageBox.Show("保存成功");
          dg.Close();
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
