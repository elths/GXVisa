using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// VisaNumbers.xaml 的交互逻辑
    /// </summary>
    public partial class VisaNumbers : UserControl
    {
        visaEntities visaORM = new visaEntities();

        ObservableCollection<QZKC> IDCs = new ObservableCollection<QZKC>();

        ObservableCollection<QZKC> tempIDCs = new ObservableCollection<QZKC>();

             int numTotal ;
            int numUsed ;
            int numNotUsed;


            string stringStart = "";
            string stringEnd = "";


            int numStart = 0;
            int numEnd = 0;
            string FirstLetter ="";

            int StringLenght = 0;



        public VisaNumbers()
        {
            InitializeComponent();

            InitData();

            InitIDCs();

            MainListBox.DataContext = IDCs;

            InitSumInfo();

        }

        void InitIDCs()
        {
            foreach (QZKC qObj in visaORM.QZKC.Where(q => q.FIsUse == false).Take(200))
            {
                IDCs.Add(qObj);
            }

        }

        /// <summary>
        /// 统计最新数字：全部、已使用、剩余
        /// </summary>
        void InitSumInfo()
        {
             numTotal = visaORM.QZKC.Count();
             numUsed = visaORM.QZKC.Where(q=>q.FIsUse==true).Count();
             numNotUsed = visaORM.QZKC.Where(q=>q.FIsUse==false).Count();
            tbSumInfo.Text = "当前共有签证号码 " + numTotal.ToString() + " 个 ，  已使用 " + numUsed + " 个 ， 剩余 " + numNotUsed + " 个 。";

        }

        void InitData()
        {


            //foreach (QZKC idcObject in visaORM.QZKC)
            //{
            //    IDCs.Add(idcObject);
            //    if (idcObject.FIsUse == true)
            //        MainListBox.SelectedItems.Add(idcObject);
            //}
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (QZKC idcObject in IDCs)
            {
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateControl())
                return;


            stringStart = txtStartNum.Text;
            stringEnd = txtEndNum.Text;


            numStart = Convert.ToInt32(txtStartNum.Text);
            numEnd = Convert.ToInt32(txtEndNum.Text);
            FirstLetter = txtLetter.Text;

            int StringLenght = stringStart.Length;



            int stateNum = 0;
            if (stateUsed.IsChecked==true)
            {
                stateNum = 1;
            }
            else if (stateAll.IsChecked==true)
            {
                stateNum = 2;
            }

            SetIDCs(numStart, numEnd, FirstLetter,stateNum);
        }

        void SetIDCs(int numStart, int numEnd, string FirstLetter,int state)
        {
            QZKC tempModel;
            IDCs.Clear();

            switch (state)
            {
                case 2:
                for (int i = numStart; i <= numEnd; i++)
                {
                    tempModel = new QZKC();
                    int NumberLenght = i.ToString().Length;
                    tempModel.FQZID = GetVisaNumberString(FirstLetter, NumberLenght, i);
                    IDCs.Add(tempModel);
                    //if (visaORM.QZKC.FirstOrDefault(q => q.FQZID == tempModel.FQZID && q.FIsUse==true) != null) MainListBox.SelectedItems.Add(tempModel);
                }
                    break;
                case 1:
                    for (int i = numStart; i <= numEnd; i++)
                    {
                        tempModel = new QZKC();
                        int NumberLenght = i.ToString().Length;
                        tempModel.FQZID = GetVisaNumberString(FirstLetter, NumberLenght, i);

                        if (visaORM.QZKC.FirstOrDefault(q => q.FQZID == tempModel.FQZID && q.FIsUse == true) != null)
                        {
                            tempModel.FIsUse = true;
                            IDCs.Add(tempModel);
                        }
                    }
                    break;
                case 0:
                    for (int i = numStart; i <= numEnd; i++)
                    {
                        tempModel = new QZKC();
                        int NumberLenght = i.ToString().Length;
                        tempModel.FQZID = GetVisaNumberString(FirstLetter, NumberLenght, i);

                        if (visaORM.QZKC.FirstOrDefault(q => q.FQZID == tempModel.FQZID && q.FIsUse == false) != null) IDCs.Add(tempModel);
                    }
                    break;
                default:
                    break;


            }
        }

        private void txtStartNum_EditValueChanged(object sender, KeyEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStartNum.Text) || string.IsNullOrEmpty(txtEndNum.Text))
                {
                    return;
                }
                int num = Convert.ToInt32(txtEndNum.Text) - Convert.ToInt32(txtStartNum.Text) + 1;
                if (num < 1) return;

                txtNumber.Text = num.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请输入正确号码");
            }
        }
        //
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("将添加"+txtNumber.Text+"个号码，是否确定?", "添加确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (!ValidateControl())
                    return;

                string stringStart = txtStartNum.Text;
                string stringEnd = txtEndNum.Text;


                numStart = Convert.ToInt32(txtStartNum.Text);
                numEnd = Convert.ToInt32(txtEndNum.Text);
                string FirstLetter = txtLetter.Text.ToUpper();

                int StringLenght = stringStart.Length;

                for (int i = numStart; i <= numEnd; i++)
                {
                    int NumberLenght = i.ToString().Length;
                    string QZID = GetVisaNumberString(FirstLetter, NumberLenght, i);
                    QZKC qzObj = visaORM.QZKC.FirstOrDefault(q => q.FQZID == QZID);
                    if (qzObj == null)
                    {
                        QZKC tempObj = new QZKC();
                        visaORM.QZKC.AddObject(tempObj);
                        tempObj.FQZID = QZID;
                        tempObj.FIsUse = false;
                        visaORM.SaveChanges();
                    }

                }
                visaORM.SaveChanges();
                MessageBox.Show("添加完成");
                InitSumInfo();
                stateAll_Checked(sender,null);
                btnPrev_Click(null, null);
            }
        }

        private static string GetVisaNumberString(string FirstLetter, int NumberLenght, int i)
        {
            return FirstLetter + ("0000000" + i.ToString()).Substring(NumberLenght);
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("将删除" + txtNumber.Text + "个号码，是否确定?", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (!ValidateControl())
                    return;

                string stringStart = txtStartNum.Text;
                string stringEnd = txtEndNum.Text;


                int numStart = Convert.ToInt32(txtStartNum.Text);
                int numEnd = Convert.ToInt32(txtEndNum.Text);
                string FirstLetter = txtLetter.Text;

                int StringLenght = stringStart.Length;


                for (int i = numStart; i <= numEnd; i++)
                {
                    int NumberLenght = i.ToString().Length;
                    string QZID = GetVisaNumberString(FirstLetter, NumberLenght, i);
                    QZKC qzObj = visaORM.QZKC.FirstOrDefault(q => q.FQZID == QZID);
                    if (qzObj != null)
                    {
                        if (qzObj.FIsUse == false)
                        {
                            visaORM.DeleteObject(qzObj);

                        }
                        else
                        {
                            if (MessageBox.Show( QZID + "已经被使用，是否确认删除?", "删除确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                visaORM.DeleteObject(qzObj);
                            }
                        }
                    }

                }
                visaORM.SaveChanges();
                MessageBox.Show("删除完成");

                InitSumInfo();


            }
        }


        bool ValidateControl()
        {
            if (string.IsNullOrEmpty(txtLetter.Text) || string.IsNullOrEmpty(txtStartNum.Text) || string.IsNullOrEmpty(txtEndNum.Text))
            {
                MessageBox.Show("前缀字母、开始数字、结束数字为必填项");
                return false;
            }


            string stringStart = txtStartNum.Text;
            string stringEnd = txtEndNum.Text;
            if (stringStart.Length !=7 || stringEnd.Length !=7)
            {
                MessageBox.Show("数字必须为7位");
                return false;
            }

            int numStart = Convert.ToInt32(txtStartNum.Text);
            int numEnd = Convert.ToInt32(txtEndNum.Text);
            string FirstLetter = txtLetter.Text;

            int StringLenght = stringStart.Length;
            int NumberLenght = numStart.ToString().Length;


            if (numStart > numEnd)
            {
                MessageBox.Show("结束数字必须大于开始数字");
                return false;
            }
            return true;

        }

        private void stateAll_Checked(object sender, RoutedEventArgs e)
        {
            if (numStart == 0 && numEnd==0)
            {
                IDCs.Clear();
                foreach (QZKC qObj in visaORM.QZKC.Take(200))
                {
                    IDCs.Add(qObj);
                }
            }
            else
            {
                SetIDCs(numStart, numEnd, FirstLetter, 2);

            }
        }

        private void stateUsed_Checked(object sender, RoutedEventArgs e)
        {
            if (numStart == 0 && numEnd == 0)
            {
                IDCs.Clear();
                foreach (QZKC qObj in visaORM.QZKC.Where(q=>q.FIsUse==true).Take(200))
                {
                    IDCs.Add(qObj);
                }
            }
            else
            {
                SetIDCs(numStart, numEnd, FirstLetter, 1);
            }
        }

        private void stateNotUsed_Checked(object sender, RoutedEventArgs e)
        {
            if (numStart == 0 && numEnd == 0)
            {
                IDCs.Clear();
                foreach (QZKC qObj in visaORM.QZKC.Where(q => q.FIsUse == false).Take(200))
                {
                    IDCs.Add(qObj);
                }
            }
            else
            {
            SetIDCs(numStart, numEnd, FirstLetter, 0);
            }

        }

    }
}
