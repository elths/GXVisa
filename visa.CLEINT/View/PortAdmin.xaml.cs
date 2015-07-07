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
    /// PortAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class PortAdmin : UserControl
    {

        visaEntities visaORM = new visaEntities();

        TPara modelPort;

        TPara modelProtocol;

        public PortAdmin()
        {
            InitializeComponent();

            modelPort = visaORM.TPara.FirstOrDefault(p => p.FParaID == "paraPort");
            if (modelPort == null)
            {
                MessageBox.Show("端口参数丢失，请补充数据");
                return;
            }
            else
            {
                txtPort.Text = modelPort.FParaValue.ToString();
            }

            modelProtocol = visaORM.TPara.FirstOrDefault(p => p.FParaID == "paraProtocol");
            if (modelProtocol == null)
            {
                MessageBox.Show("协议参数丢失，请补充数据");
                return;
            }
            else
            {
                txtProtocol.Text = modelProtocol.FParaValue.ToString();
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                modelPort.FParaValue = txtPort.Text;
                visaORM.ObjectStateManager.ChangeObjectState(modelPort, System.Data.EntityState.Modified);
                visaORM.SaveChanges();

                modelProtocol.FParaValue = txtProtocol.Text;
                visaORM.ObjectStateManager.ChangeObjectState(modelProtocol, System.Data.EntityState.Modified);
                visaORM.SaveChanges();

                MessageBox.Show("保存成功");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("保存失败，原因:"+ex.ToString());
            }


        }
    }
}
