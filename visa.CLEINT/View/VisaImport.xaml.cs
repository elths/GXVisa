using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
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
using EX = Microsoft.Office.Interop.Excel;


namespace visa.CLEINT.View
{
    /// <summary>
    /// VisaImport.xaml 的交互逻辑
    /// </summary>
    public partial class VisaImport : UserControl
    {
        visaEntities visaORM = new visaEntities();

        public VisaImport()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                //op.InitialDirectory = lblSavePath.Text;//默认的打开路径
                op.RestoreDirectory = true;
                op.Filter = "Excel文件|*.xlsx";
                op.ShowDialog();
                txtVisaExcelPath.Text = op.FileName;

                string sheetname = "Sheet1";
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtVisaExcelPath.Text + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
                OleDbConnection conn = new OleDbConnection(strConn);
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + sheetname + "$]", conn);
                DataSet ds = new DataSet();
                oada.Fill(ds);

                DataTable dt = ds.Tables[0];

                textBox.AppendText(string.Format("共 {0} 份签证。\r\n", dt.Rows.Count));

                ds.Dispose();
                ds = null;
                oada.Dispose();
                oada = null;
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            catch (System.Exception ex)
            {

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (txtVisaExcelPath.Text == "")
            {
                textBox.AppendText("请先上传excel文件。");
                return;
            }


            try
            {
                string sheetname = "Sheet1";
                string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtVisaExcelPath.Text + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
                OleDbConnection conn = new OleDbConnection(strConn);
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + sheetname + "$]", conn);
                DataSet ds = new DataSet();
                oada.Fill(ds);

                DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "dd/MM/yyyy";

                DataTable dt = ds.Tables[0];

                int rowIndex = 1;
                int sss = 0;
                int bbb = 0;
                int count = 0;

                textBox.AppendText("开始导入数据。");

                foreach (DataRow dr in dt.Rows)
                {

                    try
                    {
                        var model = new Customer();


                        visaORM.Customer.AddObject(model);

                        //model.FQZID = dr[0].ToString();
                        #region Name
                        string Name = dr[1].ToString();
                        if (String.IsNullOrEmpty(Name))
                        {
                            throw new Exception("“姓名”不能为空。");
                        }
                        #endregion
                        #region NameEn
                        string NameEn = dr[2].ToString();
                        if (String.IsNullOrEmpty(NameEn))
                        {
                            throw new Exception("“Name”不能为空。");
                        }
                        #endregion
                        #region BirthDay
                        string sBirthDay = dr[3].ToString();
                        DateTime BirthDay;
                        if (String.IsNullOrEmpty(sBirthDay))
                        {
                            throw new Exception("“出生日期”不能为空。");
                        }
                        else
                        {
                            try
                            {
                                BirthDay = Convert.ToDateTime(sBirthDay, dtFormat);
                            }
                            catch
                            {
                                throw new Exception("“出生日期”格式不正确。");
                            }
                        }
                        #endregion

                        #region Sex
                        string Sex = dr[4].ToString();

                        if (Sex != "男" && Sex != "女")
                            throw new Exception("“性别”格式不正确。");
                        #endregion

                        #region BirthPlace
                        string BirthPlace = dr[5].ToString();
                        if (String.IsNullOrEmpty(BirthPlace))
                        {
                            throw new Exception("“出生地点”不能为空。");
                        }
                        #endregion
                        #region BirthPlaceEn
                        string BirthPlaceEn = dr[6].ToString();
                        if (String.IsNullOrEmpty(BirthPlaceEn))
                        {
                            throw new Exception("“Place of birth”不能为空。");
                        }

                        #endregion
                        #region BirthNationlity
                        string BirthNationlity = dr[7].ToString();
                        if (String.IsNullOrEmpty(BirthNationlity))
                        {
                            throw new Exception("“原国籍”不能为空。");
                        }
                        #endregion
                        #region BirthNationlityEn
                        string BirthNationlityEn = dr[8].ToString();
                        if (String.IsNullOrEmpty(BirthNationlityEn))
                        {
                            throw new Exception("“Nationality at birth”不能为空。");
                        }
                        #endregion
                        #region BirthNationlityPresent
                        string BirthNationlityPresent = dr[9].ToString();
                        if (String.IsNullOrEmpty(BirthNationlityPresent))
                        {
                            throw new Exception("“现国籍”不能为空。");
                        }
                        #endregion
                        #region BirthNationlityPresentEn
                        string BirthNationlityPresentEn = dr[10].ToString();
                        if (String.IsNullOrEmpty(BirthNationlityPresentEn))
                        {
                            throw new Exception("“Present Nationality”不能为空。");
                        }
                        #endregion

                        #region PassportNo
                        string PassportNo = dr[11].ToString();
                        if (String.IsNullOrEmpty(PassportNo))
                        {
                            throw new Exception("“护照号码”不能为空。");
                        }
                        #endregion
                        #region PassportType
                        string PassportType = dr[12].ToString();
                        if (String.IsNullOrEmpty(PassportType))
                        {
                            throw new Exception("“护照类型”不能为空。");
                        }
                        #endregion
                        #region PassportMake
                        string PassportMake = dr[13].ToString();
                        if (String.IsNullOrEmpty(PassportMake))
                        {
                            throw new Exception("“发照机关”不能为空。");
                        }
                        #endregion
                        #region PassportMakeDate
                        string sPassportMakeDate = dr[14].ToString();
                        DateTime PassportMakeDate;
                        if (String.IsNullOrEmpty(sPassportMakeDate))
                        {
                            throw new Exception("“发照日期”不能为空。");
                        }
                        else
                        {
                            try
                            {
                                PassportMakeDate = Convert.ToDateTime(sPassportMakeDate, dtFormat);
                            }
                            catch
                            {
                                throw new Exception("“发照日期”格式不正确。");
                            }
                        }
                        #endregion
                        #region PassportValidDate
                        string sPassportValidDate = dr[15].ToString();
                        DateTime PassportValidDate;
                        if (String.IsNullOrEmpty(sPassportValidDate))
                        {
                            throw new Exception("“护照有效期”不能为空。");
                        }
                        else
                        {
                            try
                            {
                                PassportValidDate = Convert.ToDateTime(sPassportValidDate, dtFormat);
                            }
                            catch
                            {
                                throw new Exception("“护照有效期”格式不正确。");
                            }
                        }
                        #endregion


                        #region ProfessionWork
                        string ProfessionWork = dr[16].ToString();
                        #endregion
                        #region ProfessionCompany
                        string ProfessionCompany = dr[17].ToString();
                        #endregion
                        #region ProfessionTele
                        string ProfessionTele = dr[18].ToString();
                        #endregion
                        #region AddressNow
                        string AddressNow = dr[19].ToString();
                        #endregion
                        #region AddressTele
                        string AddressTele = dr[20].ToString();
                        #endregion
                        #region ChildrenName1
                        string ChildrenName1 = dr[21].ToString();
                        #endregion
                        #region ChildrenBirthDay1
                        string sChildrenBirthDay1 = dr[22].ToString();
                        DateTime? ChildrenBirthDay1;
                        if (String.IsNullOrEmpty(sChildrenBirthDay1))
                        {
                            ChildrenBirthDay1 = null;
                        }
                        else
                        {
                            try
                            {
                                ChildrenBirthDay1 = Convert.ToDateTime(sChildrenBirthDay1, dtFormat);
                            }
                            catch
                            {
                                ChildrenBirthDay1 = null;
                            }
                        }
                        #endregion
                        #region ChildrenBirthLink1
                        string ChildrenBirthLink1 = dr[23].ToString();
                        #endregion
                        #region ChildrenName2
                        string ChildrenName2 = dr[24].ToString();
                        #endregion
                        #region ChildrenBirthDay2
                        string sChildrenBirthDay2 = dr[25].ToString();
                        DateTime? ChildrenBirthDay2;
                        if (String.IsNullOrEmpty(sChildrenBirthDay2))
                        {
                            ChildrenBirthDay2 = null;
                        }
                        else
                        {
                            try
                            {
                                ChildrenBirthDay2 = Convert.ToDateTime(sChildrenBirthDay2, dtFormat);
                            }
                            catch
                            {
                                ChildrenBirthDay2 = null;
                            }
                        }
                        #endregion
                        #region ChildrenBirthLink2
                        string ChildrenBirthLink2 = dr[26].ToString();
                        #endregion
                        #region ChildrenName3
                        string ChildrenName3 = dr[27].ToString();
                        #endregion
                        #region ChildBirthDay3
                        string sChildrenBirthDay3 = dr[28].ToString();
                        DateTime? ChildrenBirthDay3;
                        if (String.IsNullOrEmpty(sChildrenBirthDay3))
                        {
                            ChildrenBirthDay3 = null;
                        }
                        else
                        {
                            try
                            {
                                ChildrenBirthDay3 = Convert.ToDateTime(sChildrenBirthDay3, dtFormat);
                            }
                            catch
                            {
                                ChildrenBirthDay3 = null;
                            }
                        }
                        #endregion
                        #region ChildrenBirthLink3
                        string ChildrenBirthLink3 = dr[29].ToString();
                        #endregion
                        #region Purpose
                        string Purpose = dr[30].ToString();
                        if (String.IsNullOrEmpty(Purpose))
                        {
                            throw new Exception("“出境目的”不能为空。");
                        }
                        #endregion


                        #region VisitCompany
                        string VisitCompany = dr[31].ToString();
                        #endregion
                        #region VisitnamName
                        string VisitnamName = dr[32].ToString();
                        #endregion
                        #region VisitnamAddress
                        string VisitnamAddress = dr[33].ToString();
                        #endregion
                        #region DurationBegin
                        string sDurationBegin = dr[34].ToString();
                        DateTime DurationBegin;
                        if (String.IsNullOrEmpty(sDurationBegin))
                        {
                            throw new Exception("“预计出入境开始日期”不能为空。");
                        }
                        else
                        {
                            try
                            {
                                DurationBegin = Convert.ToDateTime(sDurationBegin, dtFormat);
                            }
                            catch
                            {
                                throw new Exception("“预计出入境开始日期”格式不正确。");
                            }
                        }
                        #endregion
                        #region DurationEnd
                        string sDurationEnd = dr[35].ToString();
                        DateTime DurationEnd;
                        if (String.IsNullOrEmpty(sDurationEnd))
                        {
                            throw new Exception("“预计出入境结束日期”不能为空。");
                        }
                        else
                        {
                            try
                            {
                                DurationEnd = Convert.ToDateTime(sDurationEnd, dtFormat);
                            }
                            catch
                            {
                                throw new Exception("“预计出入境结束日期”格式不正确。");
                            }
                        }
                        #endregion
                        #region DurationDay
                        int DurationDay;
                        try
                        {
                            switch (Convert.ToInt32(dr[36].ToString()))
                            {
                                case 1:
                                    DurationDay = 1;
                                    break;
                                case 3:
                                    DurationDay = 2;
                                    break;
                                case 6:
                                    DurationDay = 3;
                                    break;
                                case 12:
                                    DurationDay = 4;
                                    break;
                                case 24:
                                    DurationDay = 5;
                                    break;
                                case 36:
                                    DurationDay = 6;
                                    break;
                                case 48:
                                    DurationDay = 7;
                                    break;
                                case 60:
                                    DurationDay = 8;
                                    break;
                                default:
                                    DurationDay = 1;
                                    break;
                            }
                        }
                        catch
                        {
                            throw new Exception("“在国外停留时间”格式不正确。");
                        }
                        #endregion
                        #region Times
                        int Times;
                        try
                        {
                            Times = dr[37].ToString() == "1" ? 1 : 2;
                        }
                        catch
                        {
                            throw new Exception("“出入境次数”格式不正确。");
                        }
                        #endregion
                        #region SpeedType
                        int SpeedType;
                        try
                        {
                            SpeedType = dr[38].ToString() == "N-" ? 3 : dr[38].ToString() == "N+" ? 1 : 2;
                        }
                        catch
                        {
                            throw new Exception("“办理速度”格式不正确。");
                        }
                        #endregion
                        #region SysMemo
                        string SysMemo = dr[39].ToString();
                        #endregion

                        #region CreateDate
                        //string sCreateDate = dr[40].ToString();
                        //DateTime? CreateDate;
                        //if (String.IsNullOrEmpty(sCreateDate))
                        //{
                        //    CreateDate = null;
                        //}
                        //else
                        //{
                        //    try
                        //    {
                        //        CreateDate = Convert.ToDateTime(sCreateDate, dtFormat);
                        //    }
                        //    catch
                        //    {
                        //        CreateDate = null;
                        //    }
                        //}
                        #endregion
                        #region CreateCompany
                        string CreateCompany = dr[41].ToString();
                        if (String.IsNullOrEmpty(CreateCompany))
                        {
                            CreateCompany = MainContext.UserCompanyName;
                        }
                        #endregion
                        #region CreateUser
                        string CreateUser = dr[42].ToString();
                        #endregion


                        #region Check Data
                        if (CreateUser != MainContext.UserName)
                        {
                            throw new Exception("该签证数据不是由当前用户导出。");
                        }

                        if (PassportValidDate <= DurationEnd)
                        {
                            throw new Exception("“预计出入境结束日期（离开当地日期）”不在“护照有效期”内，不能保存数据。");
                        }
                        if (PassportMakeDate > DateTime.Today)
                        {
                            throw new Exception("“发证日期”大于今天，这是不允许的。");
                        }
                        if (DurationBegin < DateTime.Today)
                        {
                            throw new Exception("“预计出入境开始日期”不能小于今天。");
                        }
                        if (DurationEnd < DurationBegin)
                        {
                            throw new Exception("“预计出入境结束日期”不能小于“开始日期”。");
                        }

                        if (visaORM.CustomerT.FirstOrDefault(c => c.FPassportNo == PassportNo) != null)
                        {
                            throw new Exception("该护照在特殊人物数据中，不能保存。");
                        }

                        if (visaORM.Customer.FirstOrDefault(c => c.FPassportNo == PassportNo && c.FSysSend == false && c.FsysZF != true && c.FStopSend != true) != null)
                        {
                            throw new Exception("该护照已接受申请，正在办理中。");
                        }

                        if (visaORM.Customer.FirstOrDefault(c => c.FPassportNo == PassportNo && c.FsysZF != true && c.FStopSend != true && c.FDurationEnd > DateTime.Now) != null)
                        {
                            if (MessageBox.Show("该客户上一次签证尚未过期，是否确认保存?", "保存确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                throw new Exception("次签证尚未过期，没有被允许通过。");
                            }

                        }


                        if (Purpose == "旅游" &&DurationDay > 2)
                        {
                            throw new Exception("该旅游最多只能停留3个月。");
                        }
                        if (Purpose == "企业" && DurationDay > 5)
                        {
                            throw new Exception("企业活动最多只能停留2年。");
                        }
                        if (Purpose == "劳务" && DurationDay > 6)
                        {
                            throw new Exception("劳务活动最多只能停留3年。");
                        }


                        //if (BirthDay.AddYears(80).Date < DateTime.Today)
                        //{
                        //    if (MessageBox.Show("此人年龄大约80岁，是否通过保存？\r\n（自动编号：" + ID + "，姓名：" + Name + "，护照号：" + PassportNo + "，出生日期：" + BirthDay.ToString("dd\\/MM\\/yyyy") + "）"
                        //        , Common.GetText("Tips"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //    {
                        //        throw new Exception("此人年龄大约80岁，没有被允许通过。");
                        //    }
                        //}
                        //if (BirthDay.AddYears(4).Date > DateTime.Today)
                        //{
                        //    if (MessageBox.Show(Common.GetText("Send11AgeX"), Common.GetText("Tips"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //    {
                        //        return;
                        //    }
                        //}

                        //if (!(String.IsNullOrEmpty(DataAccess.DaDDD.GetValue<String>("CustomerT", "'aaaa'", "PassportNo = '" + PassportNo + "' "))))
                        //{
                        //    if (MessageBox.Show("此签证申请人物已经被设置为了特殊人物，是否可以通过？\r\n（自动编号：" + ID + "，姓名：" + Name + "，护照号：" + PassportNo + "）"
                        //        , Common.GetText("Tips"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //    {
                        //        throw new Exception("此签证申请人物已经被设置为了特殊人物，没有被允许通过。");
                        //    }
                        //}

                        //if (!(String.IsNullOrEmpty(DataAccess.DaDDD.GetValue<String>("Customer", "'aaaa'", "PassportNo = '" + PassportNo + "' and StopSend = 1 "))))
                        //{
                        //    if (MessageBox.Show("此签证申请使用的护照曾经被拒签过，是否可以通过？\r\n（自动编号：" + ID + "，姓名：" + Name + "，护照号：" + PassportNo + "）"
                        //        , Common.GetText("Tips"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //    {
                        //        throw new Exception("此签证申请使用的护照曾经被拒签过，没有被允许通过。");
                        //    }
                        //    else
                        //    {
                        //        DataAccess.DaCheck.ResetCustomerStop1(PassportNo);
                        //    }
                        //}

                        //string tempstr2 = String.IsNullOrEmpty(ID) ? "" : ID;
                        //string tempstr1 = DataAccess.DaDDD.GetValue<String>("Customer", "ID"
                        //    , " ID != '" + tempstr2 + "' and PassportNo = '" + PassportNo + "' and isnull(SysZF,0) = 0 and StopSend = 0 and ( convert(char(10),DurationEnd,102) >= '" + DurationBegin.ToString("yyyy.MM.dd") + "' or convert(char(10),DurationEnd,102) >= '" + DateTime.Today.ToString("yyyy.MM.dd") + "') ");
                        //string tempstr3 = DataAccess.DaDDD.GetValue<String>("Customer", "ID"
                        //    , " ID != '" + tempstr2 + "' and PassportNo = '" + PassportNo + "' and isnull(SysZF,0) = 0 and StopSend = 0 and ( convert(char(10),DurationBegin,102) >= '" + DurationBegin.AddDays(-7).ToString("yyyy.MM.dd") + "' or convert(char(10),DurationBegin,102) >= '" + DateTime.Today.AddDays(-7).ToString("yyyy.MM.dd") + "') ");

                        //if (!(String.IsNullOrEmpty(tempstr3)))
                        //{
                        //    throw new Exception("此护照号已被自动编号为[" + tempstr3 + "]的签证使用，在签证出境开始日期一周内，不能重复使用护照。");
                        //}
                        //if (!(String.IsNullOrEmpty(tempstr1)))
                        //{
                        //    if (MessageBox.Show("此护照号已被自动编号为[" + tempstr1 + "]的签证使用，在签证有效期内出现重复使用护照，是否允许通过？", Common.GetText("Tips"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //    {
                        //        throw new Exception("此护照号已被自动编号为[" + tempstr1 + "]的签证使用，在签证有效期内出现重复使用护照，没有被允许通过。");
                        //    }
                        //}
                        #endregion



                        model.FName = Name;
                        model.FNameEn = NameEn;
                        model.FBirthDay = BirthDay;
                        model.FSex = Sex;
                        model.FBirthPlace = BirthPlace;
                        model.FBirthPlaceEn = BirthPlaceEn;
                        model.FBirthNationlity = BirthNationlity;
                        model.FBirthNationlityEn = BirthNationlityEn;
                        model.FBirthNationlityPresent = BirthNationlityPresent;
                        model.FBirthNationlityPresentEn = BirthNationlityPresentEn;

                        model.FPassportNo = PassportNo;
                        model.FPassportType = PassportType;
                        model.FPassportMake = PassportMake;
                        model.FPassportMakeDate = PassportMakeDate;
                        model.FPassportValidDate = PassportValidDate;

                        model.FProfessionWork = ProfessionWork;
                        model.FProfessionCompany = ProfessionCompany;
                        model.FProfessionTele = ProfessionTele;
                        model.FAddressNow = AddressNow;
                        model.FAddressTele = AddressTele;

                        model.FChildrenName1 = ChildrenName1;
                        model.FChildrenBirthDay1 = ChildrenBirthDay1;
                        model.FChildrenBirthLink1 = ChildrenBirthLink1;
                        model.FChildrenName2 = ChildrenName2;
                        model.FChildrenBirthDay2 = ChildrenBirthDay2;
                        model.FChildrenBirthLink3 = ChildrenBirthLink2;
                        model.FChildrenName3 = ChildrenName3;
                        model.FChildrenBirthDay3 = ChildrenBirthDay3;
                        model.FChildrenBirthLink3 = ChildrenBirthLink3;

                        model.FPurpose = Purpose;
                        model.FVisitCompany = VisitCompany;
                        model.FVisitnamName = VisitnamName;
                        model.FVisitnamAddress = VisitnamAddress;
                        model.FDurationBegin = DurationBegin;
                        model.FDurationEnd = DurationEnd;
                        model.FDurationDay = DurationDay;
                        model.FTimes = Times;

                        model.FSpeedType = SpeedType;
                        model.FSysMemo = SysMemo;


                        model.FSysPut = false;
                        model.FSysSend = false;
                        model.FSysPrint = false;
                        model.FSysChk = false;
                        model.FSysSure = false;

                        model.FQZID = "";

                        model.FCreateCompany = CreateCompany;
                        model.FCreateDate = DateTime.Now;
                        model.FCreateUser = MainContext.UserID;
                        model.FModifyDate = DateTime.Now;
                        model.FModifyUser = MainContext.UserID;
                        model.FStopSend = false;
                        model.FsysZF = false;
                        model.FAutoID = GetAutoID(DateTime.Now.ToString("yyMMdd"), model.FDurationDay.ToString(), model.FTimes.ToString());


                        visaORM.SaveChanges();
                        sss++;
                        textBox.AppendText(String.Format("\r\n第{0}行数据导入成功。", count+1));
                    }
                    catch (Exception err)
                    {
                        bbb++;
                        textBox.AppendText(String.Format("\r\n第{0}行数据导入失败。--" + err.Message, count + 1));
                    }

                    count++;

                    textBox.AppendText(String.Format("\r\n导入完成，进行导入的数据有{0}条。导入成功{1}条，导入失败{2}条。", count, sss, bbb));


                }

                ds.Dispose();
                ds = null;
                oada.Dispose();
                oada = null;
                conn.Close();
                conn.Dispose();
                conn = null;

            }
            catch (Exception ex)
            {
                textBox.AppendText(ex.ToString());
            }
        }

        string GetAutoID(string TodayString, string Days, string Times)
        {
            string daysString = "";
            string timesString = "";

            switch (Days)
            {
                case "1":
                    daysString = "01";
                    break;
                case "2":
                    daysString = "03";
                    break;

                case "3":
                    daysString = "06";
                    break;

                case "4":
                    daysString = "12";
                    break;
                default:
                    daysString = "00";
                    break;
            }

            timesString = Times == "1" ? "1" : "X";
            string AutoString = "0001";
            Customer custObj = visaORM.Customer.OrderByDescending(c => c.FID).FirstOrDefault(c => c.FAutoID.Contains(TodayString));
            if (custObj != null)
            {
                string numString = (Convert.ToInt32(custObj.FAutoID.Substring(10, 4)) + 1).ToString();
                AutoString = ("0000" + numString).Substring(numString.Length);
            }

            return "SQ-" + TodayString + "-" + AutoString + "-" + daysString + "-" + timesString;
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            textBox.Document.Blocks.Clear();

        }

        private void btTemplate_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
            saveFile.Filter = "Excel文件|*.xlsx";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\ImportModel.xlsx", saveFile.FileName);
                }
                catch (System.Exception ex)
                {
                    throw new Exception("已存在同名文件，不允许覆盖。");
                }
            }

        }
        //EX.Application app = new Microsoft.Office.Interop.Excel.Application();
        //EX.Workbook book;
        //try
        //{
        //    book = app.Workbooks.Open(txtVisaExcelPath.Text, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //}
        //catch
        //{
        //    MessageBox.Show("打开文件失败，请确认文件正确。");
        //    return;
        //}
        //EX.Worksheet sheet = (EX.Worksheet)book.Worksheets[1];


        //textBox.AppendText("开始导入数据。");

    }


    //          while (true)
    //          {
    //              rowIndex++;
    //              try
    //              {
    //                  #region ID
    //                  string ID = ((EX.Range)(sheet.Cells[rowIndex, "A"])).Text.ToString();
    //                  if ((!String.IsNullOrEmpty(ID)))
    //                  {
    //                      if (!String.IsNullOrEmpty(DataAccess.DaDDD.GetValue<String>("Customer", "ID", " ID = '" + ID + "' ")))
    //                      {
    //                          throw new Exception("自动编号 " + ID + " 的签证已经在系统中存在。");
    //                      }
    //                  }
    //                  else
    //                  {
    //                      ID = "";
    //                  }
    //                  #endregion
    //                  #region Name
    //                  string Name = ((EX.Range)(sheet.Cells[rowIndex, "B"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(Name))
    //                  {
    //                      break;
    //                  }
    //                  #endregion
    //                  #region NameEn
    //                  string NameEn = ((EX.Range)(sheet.Cells[rowIndex, "C"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(NameEn))
    //                  {
    //                      throw new Exception("“Name”不能为空。");
    //                  }
    //                  #endregion
    //                  #region BirthDay
    //                  string sBirthDay = ((EX.Range)(sheet.Cells[rowIndex, "D"])).Text.ToString();
    //                  DateTime BirthDay;
    //                  if (String.IsNullOrEmpty(sBirthDay))
    //                  {
    //                      throw new Exception("“出生日期”不能为空。");
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          BirthDay = new DateTime(Int32.Parse(sBirthDay.Substring(6, 4)), Int32.Parse(sBirthDay.Substring(3, 2)), Int32.Parse(sBirthDay.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          throw new Exception("“出生日期”格式不正确。");
    //                      }
    //                  }
    //                  #endregion
    //                  #region Sex
    //                  int Sex;
    //                  switch (((EX.Range)(sheet.Cells[rowIndex, "E"])).Text.ToString())
    //                  {
    //                      case "男":
    //                          Sex = 1;
    //                          break;
    //                      case "女":
    //                          Sex = 2;
    //                          break;
    //                      default:
    //                          throw new Exception("“性别”格式不正确。");
    //                  }
    //                  #endregion
    //                  #region BirthPlace
    //                  string BirthPlace = ((EX.Range)(sheet.Cells[rowIndex, "F"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthPlace))
    //                  {
    //                      throw new Exception("“出生地点”不能为空。");
    //                  }
    //                  #endregion
    //                  #region BirthPlaceEn
    //                  string BirthPlaceEn = ((EX.Range)(sheet.Cells[rowIndex, "G"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthPlaceEn))
    //                  {
    //                      throw new Exception("“Place of birth”不能为空。");
    //                  }

    //                  #endregion
    //                  #region BirthNationlity
    //                  string BirthNationlity = ((EX.Range)(sheet.Cells[rowIndex, "H"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthNationlity))
    //                  {
    //                      throw new Exception("“原国籍”不能为空。");
    //                  }
    //                  #endregion
    //                  #region BirthNationlityEn
    //                  string BirthNationlityEn = ((EX.Range)(sheet.Cells[rowIndex, "I"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthNationlityEn))
    //                  {
    //                      throw new Exception("“Nationality at birth”不能为空。");
    //                  }
    //                  #endregion
    //                  #region BirthNationlityPresent
    //                  string BirthNationlityPresent = ((EX.Range)(sheet.Cells[rowIndex, "J"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthNationlityPresent))
    //                  {
    //                      throw new Exception("“现国籍”不能为空。");
    //                  }
    //                  #endregion
    //                  #region BirthNationlityPresentEn
    //                  string BirthNationlityPresentEn = ((EX.Range)(sheet.Cells[rowIndex, "K"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(BirthNationlityPresentEn))
    //                  {
    //                      throw new Exception("“Present Nationality”不能为空。");
    //                  }
    //                  #endregion
    //                  #region PassportNo
    //                  string PassportNo = ((EX.Range)(sheet.Cells[rowIndex, "L"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(PassportNo))
    //                  {
    //                      throw new Exception("“护照号码”不能为空。");
    //                  }
    //                  #endregion
    //                  #region PassportType
    //                  string PassportType = ((EX.Range)(sheet.Cells[rowIndex, "M"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(PassportType))
    //                  {
    //                      throw new Exception("“护照类型”不能为空。");
    //                  }
    //                  #endregion
    //                  #region PassportMake
    //                  string PassportMake = ((EX.Range)(sheet.Cells[rowIndex, "N"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(PassportMake))
    //                  {
    //                      throw new Exception("“发照机关”不能为空。");
    //                  }
    //                  #endregion
    //                  #region PassportMakeDate
    //                  string sPassportMakeDate = ((EX.Range)(sheet.Cells[rowIndex, "O"])).Text.ToString();
    //                  DateTime PassportMakeDate;
    //                  if (String.IsNullOrEmpty(sPassportMakeDate))
    //                  {
    //                      throw new Exception("“发照日期”不能为空。");
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          PassportMakeDate = new DateTime(Int32.Parse(sPassportMakeDate.Substring(6, 4)), Int32.Parse(sPassportMakeDate.Substring(3, 2)), Int32.Parse(sPassportMakeDate.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          throw new Exception("“发照日期”格式不正确。");
    //                      }
    //                  }
    //                  #endregion
    //                  #region PassportValidDate
    //                  string sPassportValidDate = ((EX.Range)(sheet.Cells[rowIndex, "P"])).Text.ToString();
    //                  DateTime PassportValidDate;
    //                  if (String.IsNullOrEmpty(sPassportValidDate))
    //                  {
    //                      throw new Exception("“护照有效期”不能为空。");
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          PassportValidDate = new DateTime(Int32.Parse(sPassportValidDate.Substring(6, 4)), Int32.Parse(sPassportValidDate.Substring(3, 2)), Int32.Parse(sPassportValidDate.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          throw new Exception("“护照有效期”格式不正确。");
    //                      }
    //                  }
    //                  #endregion
    //                  #region ProfessionWork
    //                  string ProfessionWork = ((EX.Range)(sheet.Cells[rowIndex, "Q"])).Text.ToString();
    //                  #endregion
    //                  #region ProfessionCompany
    //                  string ProfessionCompany = ((EX.Range)(sheet.Cells[rowIndex, "R"])).Text.ToString();
    //                  #endregion
    //                  #region ProfessionTele
    //                  string ProfessionTele = ((EX.Range)(sheet.Cells[rowIndex, "S"])).Text.ToString();
    //                  #endregion
    //                  #region AddressNow
    //                  string AddressNow = ((EX.Range)(sheet.Cells[rowIndex, "T"])).Text.ToString();
    //                  #endregion
    //                  #region AddressTele
    //                  string AddressTele = ((EX.Range)(sheet.Cells[rowIndex, "U"])).Text.ToString();
    //                  #endregion
    //                  #region ChildrenName1
    //                  string ChildrenName1 = ((EX.Range)(sheet.Cells[rowIndex, "V"])).Text.ToString();
    //                  #endregion
    //                  #region ChildrenBirthDay1
    //                  string sChildrenBirthDay1 = ((EX.Range)(sheet.Cells[rowIndex, "W"])).Text.ToString();
    //                  DateTime ChildrenBirthDay1;
    //                  if (String.IsNullOrEmpty(sChildrenBirthDay1))
    //                  {
    //                      ChildrenBirthDay1 = DateTime.MinValue;
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          ChildrenBirthDay1 = new DateTime(Int32.Parse(sChildrenBirthDay1.Substring(6, 4)), Int32.Parse(sChildrenBirthDay1.Substring(3, 2)), Int32.Parse(sChildrenBirthDay1.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          ChildrenBirthDay1 = DateTime.MinValue;
    //                      }
    //                  }
    //                  #endregion
    //                  #region ChildrenBirthLink1
    //                  string ChildrenBirthLink1 = ((EX.Range)(sheet.Cells[rowIndex, "X"])).Text.ToString();
    //                  #endregion
    //                  #region ChildrenName2
    //                  string ChildrenName2 = ((EX.Range)(sheet.Cells[rowIndex, "Y"])).Text.ToString();
    //                  #endregion
    //                  #region ChildrenBirthDay2
    //                  string sChildrenBirthDay2 = ((EX.Range)(sheet.Cells[rowIndex, "Z"])).Text.ToString();
    //                  DateTime ChildrenBirthDay2;
    //                  if (String.IsNullOrEmpty(sChildrenBirthDay2))
    //                  {
    //                      ChildrenBirthDay2 = DateTime.MinValue;
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          ChildrenBirthDay2 = new DateTime(Int32.Parse(sChildrenBirthDay2.Substring(6, 4)), Int32.Parse(sChildrenBirthDay2.Substring(3, 2)), Int32.Parse(sChildrenBirthDay2.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          ChildrenBirthDay2 = DateTime.MinValue;
    //                      }
    //                  }
    //                  #endregion
    //                  #region ChildrenBirthLink2
    //                  string ChildrenBirthLink2 = sheet.Cells[rowIndex, "AA"].ToString();
    //                  #endregion
    //                  #region ChildrenName3
    //                  string ChildrenName3 = ((EX.Range)(sheet.Cells[rowIndex, "AB"])).Text.ToString();
    //                  #endregion
    //                  #region ChildBirthDay3
    //                  string sChildrenBirthDay3 = ((EX.Range)(sheet.Cells[rowIndex, "AC"])).Text.ToString();
    //                  DateTime ChildrenBirthDay3;
    //                  if (String.IsNullOrEmpty(sChildrenBirthDay3))
    //                  {
    //                      ChildrenBirthDay3 = DateTime.MinValue;
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          ChildrenBirthDay3 = new DateTime(Int32.Parse(sChildrenBirthDay3.Substring(6, 4)), Int32.Parse(sChildrenBirthDay3.Substring(3, 2)), Int32.Parse(sChildrenBirthDay3.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          ChildrenBirthDay3 = DateTime.MinValue;
    //                      }
    //                  }
    //                  #endregion
    //                  #region ChildrenBirthLink3
    //                  string ChildrenBirthLink3 = ((EX.Range)(sheet.Cells[rowIndex, "AD"])).Text.ToString();
    //                  #endregion
    //                  #region Purpose
    //                  string Purpose = ((EX.Range)(sheet.Cells[rowIndex, "AE"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(Purpose))
    //                  {
    //                      throw new Exception("“出境目的”不能为空。");
    //                  }
    //                  #endregion
    //                  #region VisitCompany
    //                  string VisitCompany = ((EX.Range)(sheet.Cells[rowIndex, "AF"])).Text.ToString();
    //                  #endregion
    //                  #region VisitnamName
    //                  string VisitnamName = ((EX.Range)(sheet.Cells[rowIndex, "AG"])).Text.ToString();
    //                  #endregion
    //                  #region VisitnamAddress
    //                  string VisitnamAddress = ((EX.Range)(sheet.Cells[rowIndex, "AH"])).Text.ToString();
    //                  #endregion
    //                  #region DurationBegin
    //                  string sDurationBegin = ((EX.Range)(sheet.Cells[rowIndex, "AI"])).Text.ToString();
    //                  DateTime DurationBegin;
    //                  if (String.IsNullOrEmpty(sDurationBegin))
    //                  {
    //                      throw new Exception("“预计出入境开始日期”不能为空。");
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          DurationBegin = new DateTime(Int32.Parse(sDurationBegin.Substring(6, 4)), Int32.Parse(sDurationBegin.Substring(3, 2)), Int32.Parse(sDurationBegin.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          throw new Exception("“预计出入境开始日期”格式不正确。");
    //                      }
    //                  }
    //                  #endregion
    //                  #region DurationEnd
    //                  string sDurationEnd = ((EX.Range)(sheet.Cells[rowIndex, "AJ"])).Text.ToString();
    //                  DateTime DurationEnd;
    //                  if (String.IsNullOrEmpty(sDurationEnd))
    //                  {
    //                      throw new Exception("“预计出入境结束日期”不能为空。");
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          DurationEnd = new DateTime(Int32.Parse(sDurationEnd.Substring(6, 4)), Int32.Parse(sDurationEnd.Substring(3, 2)), Int32.Parse(sDurationEnd.Substring(0, 2)));
    //                      }
    //                      catch
    //                      {
    //                          throw new Exception("“预计出入境结束日期”格式不正确。");
    //                      }
    //                  }
    //                  #endregion
    //                  #region DurationDay
    //                  int DurationDay;
    //                  try
    //                  {
    //                      DurationDay = Convert.ToInt32(((EX.Range)(sheet.Cells[rowIndex, "AK"])).Text);
    //                  }
    //                  catch
    //                  {
    //                      throw new Exception("“在国外停留时间”格式不正确。");
    //                  }
    //                  #endregion
    //                  #region Times
    //                  int Times;
    //                  try
    //                  {
    //                      Times = ((EX.Range)(sheet.Cells[rowIndex, "AL"])).Text.ToString() == "1" ? 1 : 2;
    //                  }
    //                  catch
    //                  {
    //                      throw new Exception("“出入境次数”格式不正确。");
    //                  }
    //                  #endregion
    //                  #region SpeedType
    //                  int SpeedType;
    //                  try
    //                  {
    //                      SpeedType = ((EX.Range)(sheet.Cells[rowIndex, "AM"])).Text.ToString() == "N-" ? 1 : (((EX.Range)(sheet.Cells[rowIndex, "AM"])).Text.ToString() == "N+" ? 2 : 3);
    //                  }
    //                  catch
    //                  {
    //                      throw new Exception("“办理速度”格式不正确。");
    //                  }
    //                  #endregion
    //                  #region SysMemo
    //                  string SysMemo = ((EX.Range)(sheet.Cells[rowIndex, "AN"])).Text.ToString();
    //                  #endregion
    //                  #region CreateDate
    //                  string sCreateDate = ((EX.Range)(sheet.Cells[rowIndex, "AO"])).Text.ToString();
    //                  DateTime CreateDate;
    //                  if (String.IsNullOrEmpty(sCreateDate))
    //                  {
    //                      CreateDate = DateTime.Now;
    //                  }
    //                  else
    //                  {
    //                      try
    //                      {
    //                          CreateDate = new DateTime(Int32.Parse(sDurationEnd.Substring(0, 4)), Int32.Parse(sDurationEnd.Substring(5, 2)), Int32.Parse(sDurationEnd.Substring(8, 2)), Int32.Parse(sDurationEnd.Substring(11, 2)), Int32.Parse(sDurationEnd.Substring(14, 2)), Int32.Parse(sDurationEnd.Substring(17, 2)));
    //                      }
    //                      catch
    //                      {
    //                          CreateDate = DateTime.Now;
    //                      }
    //                  }
    //                  #endregion
    //                  #region CreateCompany
    //                  string CreateCompany = ((EX.Range)(sheet.Cells[rowIndex, "AP"])).Text.ToString();
    //                  if (String.IsNullOrEmpty(CreateCompany))
    //                  {
    //                      CreateCompany = DataContext.CompanyName;
    //                  }
    //                  #endregion 
    //                  #region CreateUser
    //                  int CreateUser = DataContext.UserID;
    //                  #endregion



    //                  //#region Save Database
    //                  //Database database = DatabaseFactory.CreateDatabase();

    //                  //using (DbCommand cmd = database.GetStoredProcCommand("sp_Customer_Insert"))
    //                  //{
    //                  //    database.DiscoverParameters(cmd);
    //                  //    //database.SetParameterValue(cmd, "ID", ID);
    //                  //    database.SetParameterValue(cmd, "Name", Name);
    //                  //    database.SetParameterValue(cmd, "NameEn", NameEn);
    //                  //    database.SetParameterValue(cmd, "BirthDay", BirthDay == DateTime.MinValue ? null : (object)BirthDay);
    //                  //    database.SetParameterValue(cmd, "Sex", Sex);
    //                  //    database.SetParameterValue(cmd, "BirthPlace", BirthPlace);
    //                  //    database.SetParameterValue(cmd, "BirthPlaceEn", BirthPlaceEn);
    //                  //    database.SetParameterValue(cmd, "BirthNationlity", BirthNationlity);
    //                  //    database.SetParameterValue(cmd, "BirthNationlityEn", BirthNationlityEn);
    //                  //    database.SetParameterValue(cmd, "BirthNationlityPresent", BirthNationlityPresent);
    //                  //    database.SetParameterValue(cmd, "BirthNationlityPresentEn", BirthNationlityPresentEn);
    //                  //    database.SetParameterValue(cmd, "PassportNo", PassportNo);
    //                  //    database.SetParameterValue(cmd, "PassportType", PassportType);
    //                  //    database.SetParameterValue(cmd, "PassportMake", PassportMake);
    //                  //    database.SetParameterValue(cmd, "PassportMakeDate", PassportMakeDate == DateTime.MinValue ? null : (object)PassportMakeDate);
    //                  //    database.SetParameterValue(cmd, "PassportValidDate", PassportValidDate == DateTime.MinValue ? null : (object)PassportValidDate);
    //                  //    database.SetParameterValue(cmd, "ProfessionWork", ProfessionWork);
    //                  //    database.SetParameterValue(cmd, "ProfessionCompany", ProfessionCompany);
    //                  //    database.SetParameterValue(cmd, "ProfessionTele", ProfessionTele);
    //                  //    database.SetParameterValue(cmd, "AddressNow", AddressNow);
    //                  //    database.SetParameterValue(cmd, "AddressTele", AddressTele);
    //                  //    database.SetParameterValue(cmd, "ChildrenName1", ChildrenName1);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthDay1", ChildrenBirthDay1 == DateTime.MinValue ? null : (object)ChildrenBirthDay1);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthLink1", ChildrenBirthLink1);
    //                  //    database.SetParameterValue(cmd, "ChildrenName2", ChildrenName2);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthDay2", ChildrenBirthDay2 == DateTime.MinValue ? null : (object)ChildrenBirthDay2);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthLink2", ChildrenBirthLink2);
    //                  //    database.SetParameterValue(cmd, "ChildrenName3", ChildrenName3);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthDay3", ChildrenBirthDay3 == DateTime.MinValue ? null : (object)ChildrenBirthDay3);
    //                  //    database.SetParameterValue(cmd, "ChildrenBirthLink3", ChildrenBirthLink3);
    //                  //    database.SetParameterValue(cmd, "Purpose", Purpose);
    //                  //    database.SetParameterValue(cmd, "VisitCompany", VisitCompany);
    //                  //    database.SetParameterValue(cmd, "VisitnamName", VisitnamName);
    //                  //    database.SetParameterValue(cmd, "VisitnamAddress", VisitnamAddress);
    //                  //    database.SetParameterValue(cmd, "DurationBegin", DurationBegin == DateTime.MinValue ? null : (object)DurationBegin);
    //                  //    database.SetParameterValue(cmd, "DurationEnd", DurationEnd == DateTime.MinValue ? null : (object)DurationEnd);
    //                  //    database.SetParameterValue(cmd, "DurationDay", DurationDay);
    //                  //    database.SetParameterValue(cmd, "Times", Times);
    //                  //    database.SetParameterValue(cmd, "SpeedType", SpeedType);
    //                  //    database.SetParameterValue(cmd, "SysMemo", SysMemo);
    //                  //    database.SetParameterValue(cmd, "CreateDate", CreateDate);
    //                  //    database.SetParameterValue(cmd, "CreateCompany", CreateCompany);
    //                  //    database.SetParameterValue(cmd, "CreateUser", CreateUser);
    //                  //    database.SetParameterValue(cmd, "ID", ID);

    //                  //    database.ExecuteNonQuery(cmd);

    //                  //    ID = database.GetParameterValue(cmd, "ID").ToString();
    //                  //}

    //                  //#endregion

    //                  sss++;
    //                  textBox.AppendText(String.Format("\r\n第{0}行数据导入成功。自动编号：{1}。", rowIndex, ID));
    //              }
    //              catch (Exception err)
    //              {
    //                  bbb++;
    //                  textBox.AppendText(String.Format("\r\n第{0}行数据导入失败。--"+err.Message, rowIndex));
    //              }

    //              //rowIndex++;
    //              count++;
    //          }

    //          textBox.AppendText(String.Format("\r\n导入完成，进行导入的数据有{0}条。\r\n导入成功{1}条，导入失败{2}条。", count, sss, bbb));

    //          book.Close(Missing.Value, Missing.Value, Missing.Value);

    //          IntPtr t = new IntPtr(app.Hwnd);
    //          int id = 0;
    //          GetWindowThreadProcessId(t, out id);
    //          System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(id);
    //          p.Kill();
    //      }


}
