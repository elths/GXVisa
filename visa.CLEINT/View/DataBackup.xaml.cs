using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using visa.MODEL;

namespace visa.CLEINT.View
{
    /// <summary>
    /// DataBackup.xaml 的交互逻辑
    /// </summary>
    public partial class DataBackup : System.Windows.Controls.UserControl
    {
        public DataBackup()
        {
            InitializeComponent();
        }

        private void btnSelectFileBack_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "备份文件|*.bak";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                txtBackupPath.Text = saveFile.FileName;
            }

        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {


            if (String.IsNullOrEmpty(txtBackupPath.Text))
            {
                System.Windows.MessageBox.Show("请选择备份路径");
                return;
            }
            try
            {
                using (var visaORM = new visaEntities())
                {
                    string sql = @"BACKUP DATABASE [visa] TO  DISK = N'" + txtBackupPath.Text + "' WITH  NOINIT ,  NOUNLOAD ,  NAME = N'Visa Back',  NOSKIP ,  STATS = 10,  NOFORMAT ";
                    int rowCount = visaORM.ExecuteStoreCommand(sql);
                }

                System.Windows.Forms.MessageBox.Show("备份成功");
            }
            catch (Exception err)
            {
                Log.WriteLog.WriteErorrLog(err);
                System.Windows.Forms.MessageBox.Show(err.Message);
            }


        }

        private void btnSelectFileRestore_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "还原文件|*.bak";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtRestorePath.Text = openFile.FileName;
            }

        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(txtRestorePath.Text))
            {
                System.Windows.MessageBox.Show("请选择还原路径");
                return;
            }
            try
            {
                using (var visaORM = new visaEntities())
                {
                    string sql = @"use master RESTORE DATABASE [QZ] FROM  DISK = N'" + txtRestorePath.Text + "' WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY ,  REPLACE ";
                    int rowCount = visaORM.ExecuteStoreCommand(sql);
                }

                System.Windows.Forms.MessageBox.Show("数据还原成功");
            }
            catch (Exception err)
            {
                Log.WriteLog.WriteErorrLog(err);
                System.Windows.Forms.MessageBox.Show(err.Message);
            }


        }
    }
}
