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
using DevExpress.XtraEditors.DXErrorProvider;
using System.Windows.Forms.Integration;
using DevExpress.Xpf.Editors;
using System.Collections.ObjectModel;

namespace visa.CLEINT.View
{
    /// <summary>
    /// VisaEdit.xaml 的交互逻辑
    /// </summary>
    public partial class VisaEdit : UserControl
    {

        public Customer model = new Customer();
        visaEntities visaORM = new visaEntities();
        List<TCountry> countryList = new List<TCountry>();

        ObservableCollection<Customer> customerList = new ObservableCollection<Customer>();

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

        /// <summary>
        /// 职业
        /// </summary>
        List<String> sourceProfessionWork = new List<String>();

        //public event PropertyChangedEventHandler PropertyChanged;


        //private void NotifyPropertyChanged(Customer custModel)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(custModel));
        //    }
        //}

        /// <summary>
        /// 国家字典
        /// </summary>
        public Dictionary<string, string> dictCountry = new Dictionary<string, string>();

        /// <summary>
        /// 出身地字典
        /// </summary>
        public Dictionary<string, string> dictPlace = new Dictionary<string, string>();

        /// <summary>
        /// 是否更新结束时间
        /// </summary>
        private bool isCanChangeTime = true;

        /// <summary>
        /// 出身地List
        /// </summary>
        public List<PlaceClass> placeList = new List<PlaceClass>();

        AxRT6700Lib.AxRt6700 axRt67001 = new AxRT6700Lib.AxRt6700();

        private MMM.Readers.Modules.Swipe.SwipeSettings swipeSettings;
        private System.Windows.Forms.Control _threadHelperControl;


        public VisaEdit()
        {


            InitializeComponent();

            InitializeViewModel();
            InitCustomerModel();

            List<Customer> tempCustomerList = visaORM.Customer.Where(c => c.FCreateUser == MainContext.UserID && c.FSysPut == false && c.FsysZF != true).ToList();
            foreach (Customer tempCustomer in tempCustomerList)
            {
                customerList.Add(tempCustomer);
            }
            LeftGrid.DataContext = customerList;

            InitData();
            InitSource();

            _threadHelperControl = new System.Windows.Forms.Control();
            _threadHelperControl.CreateControl();


            InitialiseSwipeAPI();



        }


        void InitSource()
        {
            try
            {
                sourceSex.Add("男");
                sourceSex.Add("女");
                cbSex.ItemsSource = sourceSex;

                sourcePassportType.Add("P");
                sourcePassportType.Add("O");
                sourcePassportType.Add("D");
                cbPassportType.ItemsSource = sourcePassportType;

                //sourceSpeedType.Add("N+");
                //sourceSpeedType.Add("N-");
                //sourceSpeedType.Add("O");
                //cbSpeedType.ItemsSource = sourceSpeedType;

                sourcePurpose.Add("旅游");
                sourcePurpose.Add("企业");
                sourcePurpose.Add("劳务");
                sourcePurpose.Add("领事馆");
                sourcePurpose.Add("投资");
                sourcePurpose.Add("留学");
                sourcePurpose.Add("外交NG1");
                sourcePurpose.Add("外交NG2");
                sourcePurpose.Add("外交NG3");
                sourcePurpose.Add("外交NG4");
                sourcePurpose.Add("其他VR");
                sourcePurpose.Add("其他TT");
                sourcePurpose.Add("其他NN1");
                sourcePurpose.Add("其他NN2");
                sourcePurpose.Add("其他NN3");
                sourcePurpose.Add("其他LV1");
                sourcePurpose.Add("其他LV2");
                sourcePurpose.Add("其他HN");
                cbPurpose.ItemsSource = sourcePurpose;

                sourceProfessionWork.Add("TN");
                cbProfessionWork.ItemsSource = sourceProfessionWork;

                List<string> placeList = visaORM.IDC.Select(i => i.FDQ).Distinct().ToList();
                //记录地区拼音首字母字典
                string firstShortPinYin;
                this.placeList = new List<PlaceClass>();
                foreach (var place in placeList)
                {
                    firstShortPinYin = "";
                    foreach (string PinYinWord in ChineseToPinYin.Convert(place).Split(' '))
                    {
                        if (!string.IsNullOrEmpty(PinYinWord))
                            firstShortPinYin += PinYinWord.Substring(0, 1);
                    }
                    if (!string.IsNullOrEmpty(firstShortPinYin))
                        this.placeList.Add(new PlaceClass() { FPinYin = firstShortPinYin, FPlaceName = place });
                }
                cbBirthPlace.ItemsSource = this.placeList;


                countryList = visaORM.TCountry.OrderBy(c => c.FCountryEn).ToList();
                foreach (var countryModel in countryList)
                {
                    dictCountry.Add(countryModel.FCountryEn, countryModel.FCountry);
                }

                CountryCN1.ItemsSource = countryList;
                CountryCN2.ItemsSource = countryList;
                CountryEN1.ItemsSource = countryList;
                CountryEN2.ItemsSource = countryList;

                ItemSourceClass iscObj = new ItemSourceClass();
                cbDuration.ItemsSource = iscObj.ListDuration();
                cbTimes.ItemsSource = iscObj.ListTimes();
                cbSpeedType.ItemsSource = iscObj.ListSpeed();

                var userList = visaORM.User.ToList();
                cbPutUser.ItemsSource = userList;
                cbSendUser.ItemsSource = userList;
                cbChkUser.ItemsSource = userList;
                cbPrintUser.ItemsSource = userList;
                cbCreateUser.ItemsSource = userList;
                cbModifyUser.ItemsSource = userList;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }



        }

        private void InitCustomerModel()
        {
            MainTab.DataContext = model;
            InitSelectValue();
        }

        private void InitSelectValue()
        {
            this.model.FBirthNationlity = "中国";
            if (model.FBirthNationlityEn != "China")
                model.FBirthNationlityEn = "China";
            this.model.FBirthNationlityPresent = "中国";
            if (model.FBirthNationlityPresentEn != "China")
                model.FBirthNationlityPresentEn = "China";
            this.model.FProfessionWork = "TN";

            this.model.FProfessionTele = MainContext.UserPhoneNo;
            this.model.FProfessionCompany = MainContext.UserWorkCompanyName;

            if (chbLock.IsChecked == false)
            {
                this.model.FPurpose = "旅游";
                this.model.FDurationDay = 1;
                this.model.FTimes = 1;
            }
        }


        private void InitData()
        {

            model.FPassportType = "P";
            model.FSpeedType = 2;
            #region 初始化rte6701

            //try
            //{
            //    ((System.ComponentModel.ISupportInitialize)(this.axRt67001)).BeginInit();
            //    axRt67001.Name = "axRt67001";
            //    //axRt67001.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axRt67001.OcxState")));
            //    axRt67001.OnLineError += new System.EventHandler(this.axRt67001_OnLineError);
            //    axRt67001.OnMessage += new System.EventHandler(this.axRt67001_OnMessage);
            //    axRt67001.OnTimeout += new System.EventHandler(this.axRt67001_OnTimeout);

            //    //WindowsFormsHost wfh = new WindowsFormsHost();
            //    mapHost.Child = axRt67001;
            //    ((System.ComponentModel.ISupportInitialize)(this.axRt67001)).EndInit();


            //    //bool isCom = DataAccess.DaDDD.GetCom() == "COM";

            //    //if (isCom)
            //    //{
            //    //}

            //    //axRt67001.Port = DataAccess.DaDDD.GetPort();
            //    var modelPort = visaORM.TPara.FirstOrDefault(p => p.FParaID == "paraPort");
            //    if (modelPort == null)
            //    {
            //        MessageBox.Show("端口参数丢失，请补充数据");
            //        return;
            //    }
            //    else
            //    {
            //        axRt67001.Port = Convert.ToInt32(modelPort.FParaValue);
            //    }

            //    var modelProtocol = visaORM.TPara.FirstOrDefault(p => p.FParaID == "paraProtocol");
            //    if (modelProtocol == null)
            //    {
            //        MessageBox.Show("协议参数丢失，请补充数据");
            //        return;
            //    }
            //    else
            //    {
            //        axRt67001.Protocol = Convert.ToInt32(modelProtocol.FParaValue); ;
            //    }


            //    axRt67001.Initialise();

            //    try
            //    {
            //        axRt67001.SetUserCallback(1, 2);
            //    }
            //    catch (Exception ex)
            //    {
            //        //Log.WriteLog.WriteErorrLog(ex);
            //        //MessageBox.Show(err.ToString());
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    Log.WriteLog.WriteErorrLog(ex);
            //    MessageBox.Show(ex.ToString());
            //}
            #endregion
        }

        #region  3M扫描仪

        private void InitialiseSwipeAPI()
        {
            MMM.Readers.ErrorCode lErrorCode = MMM.Readers.ErrorCode.NO_ERROR_OCCURRED;

            // Initialise logging and error handling first. The error handler callback
            // will receive all error messages generated by the 3M Page Reader SDK
            MMM.Readers.Modules.Reader.SetErrorHandler(
                new MMM.Readers.ErrorDelegate(ProcessErrorThreadHelper),
                IntPtr.Zero
            );
            //lErrorCode = MMM.Readers.Modules.Reader.InitialiseLogging(
            //    true,
            //    3,
            //    -1,
            //    "SwipeReader.Net.log"
            //);

            if (lErrorCode == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                // Next load the settings for the Swipe Reader from the ini files. You can 
                // also modify and save settings back to the ini files using 
                // MMM.Readers.Modules.Reader.SaveSwipeSettings()
                lErrorCode = MMM.Readers.Modules.Reader.LoadSwipeSettings(
                    ref swipeSettings
                );
            }

            if (lErrorCode == MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                // Initialise the Swipe Reader. Data and events will be sent back in a 
                // non-blocking fashion through the callbacks provided
                //
                // Thread helper delegates are used to avoid thread-safety issues, 
                // particularly with .NET framework 2.0
                lErrorCode = MMM.Readers.Modules.Swipe.Initialise(
                swipeSettings,
                new MMM.Readers.Modules.Swipe.DataDelegate(ProcessDataThreadHelper),
                new MMM.Readers.FullPage.EventDelegate(ProcessEventThreadHelper)
                );
            }

            if (lErrorCode != MMM.Readers.ErrorCode.NO_ERROR_OCCURRED)
            {
                if (lErrorCode == MMM.Readers.ErrorCode.ERROR_ALREADY_INITIALISED)
                {
                }
                else
                {
                    MessageBox.Show(lErrorCode + "@" + lErrorCode.ToString() + "@" + "Failed to initialise Swipe Reader API");

                }

            }
            else
            {
                // Display the hardware device and protocol in use
                string lProtocolName = new string(swipeSettings.Protocol.ProtocolName);

                if (lProtocolName.StartsWith("RTE"))
                {
                    // For RTE_INTERRUPT and RTE_POLLED modes, the Swipe Reader API can 
                    // automatically send Enable Device commands once finished reading so
                    // that you do not have to
                    if (
                        !lProtocolName.Equals("RTE_NATIVE") &&
                        swipeSettings.Protocol.RTE.AutoSendEnableDevice > 0
                    )
                    {
                        lProtocolName = string.Concat(
                            lProtocolName,
                            ", Auto Send Enable Command"
                        );
                    }

                    if (swipeSettings.Protocol.RTE.UseBCC > 0)
                    {
                        lProtocolName = string.Concat(
                            lProtocolName,
                            ", with BCC"
                        );
                    }
                    else
                    {
                        lProtocolName = string.Concat(
                            lProtocolName,
                            ", no BCC"
                        );
                    }
                    //  MessageBox.Show("3M护照扫描仪已接入");

                }


            }
        }

        private void ProcessData(MMM.Readers.Modules.Swipe.SwipeItem aDataItem, object aData)
        {
            string dataStr = string.Empty;

            if (aData is int || aData is bool || aData is float || aData is string)
            {
                dataStr = aData.ToString();
            }
            else if (aData is MMM.Readers.CodelineData)
            {
                MMM.Readers.CodelineData codeline = (MMM.Readers.CodelineData)aData;

                ReadPoo((codeline.Line1 + codeline.Line2).Replace("<", " "));

            }
            else if (aData is MMM.Readers.Modules.Swipe.MsrData)
            {
                MMM.Readers.Modules.Swipe.MsrData msrData = (MMM.Readers.Modules.Swipe.MsrData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.SwipeBarcodePDF417Data)
            {
                MMM.Readers.Modules.Swipe.SwipeBarcodePDF417Data swipeBarcodeData = (MMM.Readers.Modules.Swipe.SwipeBarcodePDF417Data)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.SwipeBarcodeCode128Data)
            {
                MMM.Readers.Modules.Swipe.SwipeBarcodeCode128Data swipeBarcodeData = (MMM.Readers.Modules.Swipe.SwipeBarcodeCode128Data)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.SwipeBarcodeCode39Data)
            {
                MMM.Readers.Modules.Swipe.SwipeBarcodeCode39Data swipeBarcodeData = (MMM.Readers.Modules.Swipe.SwipeBarcodeCode39Data)aData;
            }
            else if (aData is MMM.Readers.AAMVAData)
            {
                MMM.Readers.AAMVAData AAMVAData = (MMM.Readers.AAMVAData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.AtbData)
            {
                MMM.Readers.Modules.Swipe.AtbData atbData = (MMM.Readers.Modules.Swipe.AtbData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.RTEQAData)
            {
                MMM.Readers.Modules.Swipe.RTEQAData qaData = (MMM.Readers.Modules.Swipe.RTEQAData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.RTESwipeData)
            {
                MMM.Readers.Modules.Swipe.RTESwipeData rteData = (MMM.Readers.Modules.Swipe.RTESwipeData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.MuseSwipeData)
            {
                MMM.Readers.Modules.Swipe.MuseSwipeData museData = (MMM.Readers.Modules.Swipe.MuseSwipeData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.CuteSwipeData)
            {
                MMM.Readers.Modules.Swipe.CuteSwipeData cuteData = (MMM.Readers.Modules.Swipe.CuteSwipeData)aData;
            }
            else if (aData is MMM.Readers.Modules.Swipe.MagtekMsrSwipeData)
            {
                MMM.Readers.Modules.Swipe.MagtekMsrSwipeData magtekData = (MMM.Readers.Modules.Swipe.MagtekMsrSwipeData)aData;
            }
            else if (aData is byte)
            {
                byte dataValue = (byte)aData;
                dataStr = dataValue.ToString("X2");
            }
            else if (aData is byte[])
            {
                byte[] dataArray = aData as byte[];
                if (aDataItem == MMM.Readers.Modules.Swipe.SwipeItem.WHOLE_DATA)
                {
                    dataStr = ConvertSwipeDataByteArray(dataArray);
                }
                else
                {
                    foreach (byte byteVal in dataArray)
                    {
                        dataStr = string.Concat(dataStr, byteVal.ToString("X2"), " ");
                    }
                }
            }
        }


        private void ProcessDataThreadHelper(
    MMM.Readers.Modules.Swipe.SwipeItem aDataItem,
    object aData
)
        {
            if (_threadHelperControl.InvokeRequired)
            {
                _threadHelperControl.Invoke(
                    new MMM.Readers.Modules.Swipe.DataDelegate(ProcessData),
                    new object[] { aDataItem, aData }
                );
            }
            else
            {
                ProcessData(aDataItem, aData);
            }
        }


        // Thread helper callback to make sure data is received on the correct thread.
        private void ProcessErrorThreadHelper(
            MMM.Readers.ErrorCode aErrorCode,
            string aErrorMessage
        )
        {
            if (_threadHelperControl.InvokeRequired)
            {
                _threadHelperControl.Invoke(
                    new MMM.Readers.ErrorDelegate(ProcessError),
                    new object[] { aErrorCode, aErrorMessage }
                );
            }
            else
            {
                ProcessError(aErrorCode, aErrorMessage);
            }
        }


        // Thread helper callback to make sure data is received on the correct thread.
        private void ProcessEventThreadHelper(MMM.Readers.FullPage.EventCode aEventCode)
        {
            if (_threadHelperControl.InvokeRequired)
            {
                _threadHelperControl.Invoke(
                    new MMM.Readers.FullPage.EventDelegate(ProcessEvent),
                    new object[] { aEventCode }
                );
            }
            else
            {
                ProcessEvent(aEventCode);
            }
        }

        private void ProcessError(MMM.Readers.ErrorCode aErrorCode, string aErrorMessage)
        {

            if (aErrorCode == MMM.Readers.ErrorCode.ERROR_SWIPE_READER_NOT_CONNECTED)
            {
            }
            else if (aErrorCode == MMM.Readers.ErrorCode.ERROR_ALREADY_INITIALISED)
            {

            }
            else
            {
                MessageBox.Show(aErrorCode + "@" + aErrorCode.ToString() + "@" + aErrorMessage);
            }
        }


        private void ProcessEvent(MMM.Readers.FullPage.EventCode aEventCode)
        {
            //MessageBox.Show(aEventCode + "@" + aEventCode.ToString() + "@");
        }

        private string ConvertSwipeDataByteArray(byte[] array)
        {
            StringBuilder builder = new StringBuilder();

            foreach (byte val in array)
            {
                switch (val)
                {
                    case 0: builder.Append("{NUL}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.SOH: builder.Append("{SOH}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.STX: builder.Append("{STX}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.ETX: builder.Append("{ETX}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.LF: builder.Append("{LF}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.CR: builder.Append("{CR}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.ACK: builder.Append("{ACK}"); break;
                    case MMM.Readers.Modules.Swipe.ByteConstants.NACK: builder.Append("{NACK"); break;
                    case MMM.Readers.Modules.Swipe.CuteProtocolByteConstants.SOD1: builder.Append("{SOD1}"); break;
                    case MMM.Readers.Modules.Swipe.CuteProtocolByteConstants.EOD1: builder.Append("{EOD1}"); break;
                    case MMM.Readers.Modules.Swipe.CuteProtocolByteConstants.SOD2: builder.Append("{SOD2}"); break;
                    case MMM.Readers.Modules.Swipe.CuteProtocolByteConstants.EOD2: builder.Append("{EOD2}"); break;
                    default: builder.Append(Convert.ToChar(val)); break;
                }
            }

            return builder.ToString();
        }

        #endregion

        #region 旧扫描仪

        private void axRt67001_OnLineError(object sender, EventArgs e)
        {
            MessageBox.Show("连接错误。");
        }

        private void test()
        {
            //string message = "\rPOCHNYUAN<<GUANGZHAO<<<<_____<<<<<<<<\rG247955<<<CHN7309137M170903419204401<<<<<<50";
            //if (message.Length > 50)
            //{
            //    if (message.IndexOf("_") >= 0)
            //    {
            //        FrmMessage.ShowMessage(message.Substring(1, message.Length - 1).Replace("\r", "\r\n"));
            //    }
            //    else
            //    {
            //        ReadPoo(message.Replace("\r", ""));
            //    }
            //}
            //else
            //{
            //    FrmMessage.ShowMessage(message);
            //}
        }

        private void axRt67001_OnMessage(object sender, EventArgs e)
        {
            if (axRt67001.MsgLength > 0)
            {
                string message = GetResults(axRt67001.MsgText, axRt67001.MsgDevice);
                if (message.Length > 50)
                {
                    if (message.IndexOf("_") >= 0)
                    {
                        MessageBox.Show(message.Substring(1, message.Length - 1).Replace("\r", "\r\n"));
                    }
                    else
                    {
                        ReadPoo(message.Replace("\r", "").Replace("<", " "));
                    }
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
        }


        private string GetResults(string aValue, short aDevice)
        {
            string lText;
            //string lTempText;
            //string lCommandStr;
            //long lPos;
            //long lPrevPos;

            //lPrevPos = 1;
            if (aDevice == 79)
            {
                lText = aValue.Substring(1, aValue.Length - 1);
            }
            else
            {
                lText = aValue;
            }

            string message = lText.Replace("\r", "");
            if (String.IsNullOrEmpty(message))
            {
                return "未取得数据。";
            }
            else
            {
                return lText;
            }
        }

        private void axRt67001_OnTimeout(object sender, EventArgs e)
        {
            MessageBox.Show("读取超时错误。");
        }

        #endregion

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

            MyCommand printCommand = new MyCommand(printSingle)
            {
                Caption = "打印",
                LargeGlyph = null,
                SmallGlyph = null
            };

            MyCommand printAllCommand = new MyCommand(printAll)
            {
                Caption = "打印全部",
                LargeGlyph = null,
                SmallGlyph = null
            };


            //MyCommand copyCommand = new MyCommand(null) { Caption = "Copy", LargeGlyph = GlyphHelper.GetGlyph("/Images/Icons/Copy_32x32.png"), SmallGlyph = GlyphHelper.GetGlyph("/Images/Icons/Copy_16x16.png") };

            clipboardBar.Commands.Add(prevCommand);
            clipboardBar.Commands.Add(nextCommand);
            clipboardBar.Commands.Add(addCommand);
            clipboardBar.Commands.Add(delCommand);
            clipboardBar.Commands.Add(saveCommand);
            clipboardBar.Commands.Add(printCommand);
            clipboardBar.Commands.Add(printAllCommand);

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
            //跳到预览
            //if (!string.IsNullOrEmpty(model.FAutoID))
            //{
            //    (App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(this.txtAutoID.Text));
            //}
            //else
            //{
            //    MessageBox.Show("请输入选中一条数据，或者保存");
            //}

            string keyString = this.model.FAutoID;
            var model = visaORM.Customer.FirstOrDefault(c => c.FAutoID == keyString);
            if (model == null)
            {
                MessageBox.Show("找不到对应的签证,可能你还未保存");
                return;
            }

            Report.ReportTable1 report = new Report.ReportTable1();
            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);

            DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
            pt.Print();

        }

        private void printAll()
        {
            //(App.Current.Windows[1] as MainWindow).MainFrame.Navigate(new Report1(printID));

            string keyString = "";

            foreach (var obj in customerList)
            {
                keyString += obj.FAutoID + ",";
            }

            Report.ReportTable1 report = new Report.ReportTable1();
            report.sp_SelectPrintSendInfoTableAdapter.Fill(report.sp_SelectTable1ForPrint1._sp_SelectTable1ForPrint, keyString);

            DevExpress.XtraReports.UI.ReportPrintTool pt = new DevExpress.XtraReports.UI.ReportPrintTool(report);
            pt.Print();

        }


        private void CustomerDel()
        {
            try
            {

                Customer delObj = visaORM.Customer.FirstOrDefault(c => c.FID == model.FID);
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
                        customerList.Remove(delObj);
                        this.CustomerAdd();
                        InitData();
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }


        private void CustomerAdd()
        {
            try
            {

                int selectPurpose = 0;
                string tempVisitCompany = "";
                string tempVisitnamName = "";
                string tempVisitnamAddress = "";
                int selectTimes = 0;
                int selectDuration = 0;
                DateTime? tempDurationBegin = DateTime.Now;
                DateTime? tempDurationEnd = DateTime.Now;
                int selectSpeedType = 0;

                string tempWorkTel = ""; //工作电话
                string tempWorkCompany = ""; //工作单位

                if (model != null)
                {
                    tempWorkTel = model.FProfessionTele;
                    tempWorkCompany = model.FProfessionCompany;
                }
                if (chbLock.IsChecked == true)
                {
                    selectPurpose = cbPurpose.SelectedIndex;
                    tempVisitCompany = this.model.FVisitCompany;
                    tempVisitnamName = this.model.FVisitnamName;
                    tempVisitnamAddress = this.model.FVisitnamAddress;
                    selectTimes = cbTimes.SelectedIndex;
                    selectDuration = cbDuration.SelectedIndex;
                    tempDurationBegin = this.model.FDurationBegin;
                    tempDurationEnd = this.model.FDurationEnd;
                    selectSpeedType = cbSpeedType.SelectedIndex;

                }
                this.model = new Customer();
                InitSelectValue();
                MainTab.DataContext = this.model;

                model.FProfessionCompany = tempWorkCompany;
                model.FProfessionTele = tempWorkTel;

                if (chbLock.IsChecked == true)
                {
                    cbPurpose.SelectedIndex = selectPurpose;
                    model.FVisitCompany = tempVisitCompany;
                    model.FVisitnamName = tempVisitnamName;
                    model.FVisitnamAddress = tempVisitnamAddress;
                    cbTimes.SelectedIndex = selectTimes;
                    cbDuration.SelectedIndex = selectDuration;
                    model.FDurationBegin = tempDurationBegin;
                    model.FDurationEnd = tempDurationEnd;
                    cbSpeedType.SelectedIndex = selectSpeedType;
                }

                model.FPassportType = "P";
                model.FSpeedType = 2;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

            }

        }


        private void AddCustomerLocked(Customer model)
        {

        }


        private void CustomerSave()
        {
            try
            {

                // if (string.IsNullOrEmpty(txtPassportNo.Text))
                // {
                //     MessageBox.Show("请填写护照号码");
                //     return;
                //}
                this.MainBar.Focus();

                if (!ValidateControl(txtName, "中文姓名")) return;
                if (!ValidateControl(txtNameEn, "英文姓名")) return;
                if (!ValidateControl(dateBirth, "出生日期")) return;
                if (!ValidateControl(cbSex, "性别")) return;
                if (!ValidateControl(cbBirthPlace, "出生地点")) return;
                if (!ValidateControl(txtBirhthPlaceEn, "出生地点(英文)")) return;
                if (!ValidateControl(CountryCN1, "原国籍")) return;
                if (!ValidateControl(CountryEN1, "原国籍(英文)")) return;
                if (!ValidateControl(CountryCN2, "现国籍")) return;
                if (!ValidateControl(CountryEN2, "现国籍(英文)")) return;
                if (!ValidateControl(txtPassportNo, "护照号码")) return;
                if (!ValidateControl(cbPassportType, "护照类型")) return;
                if (!ValidateControl(dateMakeDate, "发照日期")) return;
                if (!ValidateControl(dateValiDate, "护照有效期")) return;
                if (!ValidateControl(cbPurpose, "出境目的")) return;
                if (!ValidateControl(cbDuration, "国外停留时间")) return;
                if (!ValidateControl(dateDurationBegin, "开始日期")) return;
                if (!ValidateControl(dateDurationEnd, "结束日期")) return;
                if (!ValidateControl(cbTimes, "出入境次数")) return;
                if (!ValidateControl(cbSpeedType, "办理速度")) return;

                if (model.FBirthDay >= DateTime.Today)
                {
                    MessageBox.Show("生日不能大于今天");
                    return;
                }
                if (model.FBirthDay >= model.FPassportMakeDate)
                {
                    MessageBox.Show("生日不能大于发证日期");
                    return;
                }
                if (model.FPassportMakeDate >= model.FPassportValidDate)
                {
                    MessageBox.Show("发照日期不能大于有效日期");
                    return;
                }
                if (model.FDurationBegin >= model.FDurationEnd)
                {
                    MessageBox.Show("出境开始日期不能大于入境日期");
                    return;
                }
                if (model.FPassportValidDate < model.FDurationEnd)
                {
                    MessageBox.Show("护照有效期早于出境结束日期");
                    return;
                }

                if (model.FPassportMakeDate > DateTime.Now)
                {
                    MessageBox.Show("发证日期不允许大于今天");
                    return;
                }

                if (model.FDurationBegin < DateTime.Today)
                {
                    MessageBox.Show("“预计出入境开始日期”不能小于今天。");
                    return;
                }

                if (visaORM.Customer.FirstOrDefault(c => c.FPassportNo == model.FPassportNo && c.FSysSend == false && c.FID != model.FID && c.FsysZF != true && c.FStopSend != true) != null)
                {
                    MessageBox.Show("该护照已接受申请，正在办理中。");
                    return;
                }

                if (visaORM.CustomerT.FirstOrDefault(c => c.FPassportNo == model.FPassportNo) != null)
                {
                    MessageBox.Show("该护照在特殊人物数据中，不能保存。");
                    return;
                }


                if ((DateTime.Now).Subtract((DateTime)model.FBirthDay).Days >= (80 * 365))
                {
                    if (MessageBox.Show("此人已超过80岁，是否确认保存?", "保存确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                if (visaORM.Customer.FirstOrDefault(c => c.FPassportNo == model.FPassportNo && c.FID != model.FID && c.FsysZF != true && c.FStopSend != true && c.FDurationEnd > DateTime.Now) != null)
                {
                    if (MessageBox.Show("该客户上一次签证尚未过期，是否确认保存?", "保存确认", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return;
                    }

                }

                //if (model.FDurationDay == 1 && model.FTimes != 1)
                //{
                //    MessageBox.Show("停留一个月只能出入境一次。");
                //    return;
                //}

                if (model.FPurpose == "旅游" && model.FDurationDay > 2)
                {
                    MessageBox.Show("旅游最多只能停留3个月。");
                    return;
                }
                if (model.FPurpose == "企业" && model.FDurationDay > 5)
                {
                    MessageBox.Show("企业活动最多只能停留2年。");
                    return;
                }
                if (model.FPurpose == "劳务" && model.FDurationDay > 6)
                {
                    MessageBox.Show("劳务活动最多只能停留3年。");
                    return;
                }



                //SetDurationEnd(this.cbDuration.Text);


                model.FModifyDate = DateTime.Now;
                model.FModifyUser = MainContext.UserID;
                model.FPassportNo = model.FPassportNo.ToUpper();
                if (string.IsNullOrEmpty(txtPassportMake.Text)) model.FPassportMake = "公安部出入境管理局";


                if (model.FID == 0 || model.FID.ToString() == "")
                {

                    visaORM.Customer.AddObject(model);
                    model.FCreateDate = DateTime.Now;
                    model.FCreateUser = MainContext.UserID;
                    model.FCreateCompany = MainContext.UserCompanyName;
                    model.FSysPut = false;
                    model.FSysSend = false;
                    model.FSysChk = false;
                    model.FSysPrint = false;
                    model.FSysSure = false;
                    model.FStopSend = false;
                    model.FsysZF = false;
                    model.FAutoID = GetAutoID(DateTime.Now.ToString("yyMMdd"), model.FDurationDay.ToString(), model.FTimes.ToString());
                    //"SQ-" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "-266-01-1";

                    visaORM.SaveChanges();
                    customerList.Add(model);

                    MessageBox.Show("保存成功");
                    //LeftGrid.SelectedItem = model;
                    this.CustomerAdd();
                    InitData();
                }
                else
                {
                    visaORM.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                    visaORM.SaveChanges();
                    MessageBox.Show("保存成功");
                }
                //if (model.FID == null) model.FID = 0;

                //MessageBox.Show(model.FName);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);

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

            if (e.Value == null || e.Value.ToString() == "")
            {
                e.IsValid = false;
                e.ErrorContent = "必须填写该项内容";
            }

        }

        private void ReadPoo(string name)
        {

            string name1;
            string passportType;
            string nowCount;
            string nowCountEn;
            string passportNo;
            string brithCountry;
            string brithCountryEn;
            DateTime brithday;
            DateTime validday;
            string sex;
            string city;

            ReadPassport(name, out name1, out passportType, out nowCount, out nowCountEn, out passportNo, out brithCountry, out brithCountryEn, out brithday, out validday, out sex, out city);


            int selectPurpose = 0;
            string tempVisitCompany = "";
            string tempVisitnamName = "";
            string tempVisitnamAddress = "";
            int selectTimes = 0;
            int selectDuration = 0;
            DateTime? tempDurationBegin = DateTime.Now;
            DateTime? tempDurationEnd = DateTime.Now;
            int selectSpeedType = 0;




            if (chbLock.IsChecked == true)
            {
                selectPurpose = cbPurpose.SelectedIndex;
                tempVisitCompany = this.model.FVisitCompany;
                tempVisitnamName = this.model.FVisitnamName;
                tempVisitnamAddress = this.model.FVisitnamAddress;
                selectTimes = cbTimes.SelectedIndex;
                selectDuration = cbDuration.SelectedIndex;
                tempDurationBegin = this.model.FDurationBegin;
                tempDurationEnd = this.model.FDurationEnd;
                selectSpeedType = cbSpeedType.SelectedIndex;

            }
            model = new Customer();
            model.FPassportType = "P";
            model.FSpeedType = 2;
            InitSelectValue();

            MainTab.DataContext = model;

            if (chbLock.IsChecked == true)
            {
                cbPurpose.SelectedIndex = selectPurpose;
                model.FVisitCompany = tempVisitCompany;
                model.FVisitnamName = tempVisitnamName;
                model.FVisitnamAddress = tempVisitnamAddress;
                cbTimes.SelectedIndex = selectTimes;
                cbDuration.SelectedIndex = selectDuration;
                model.FDurationBegin = tempDurationBegin;
                model.FDurationEnd = tempDurationEnd;
                cbSpeedType.SelectedIndex = selectSpeedType;
            }


            if (!(String.IsNullOrEmpty(name1))) model.FName = name1;
            if (!(String.IsNullOrEmpty(name1))) model.FNameEn = name1;

            if (!(String.IsNullOrEmpty(passportType))) model.FPassportType = passportType;
            if (!(String.IsNullOrEmpty(passportNo))) model.FPassportNo = passportNo;
            if (!(String.IsNullOrEmpty(brithCountry))) model.FBirthNationlity = brithCountry;
            if (!(String.IsNullOrEmpty(brithCountryEn))) model.FBirthNationlityEn = brithCountryEn;
            if (!(String.IsNullOrEmpty(nowCount))) model.FBirthNationlityPresent = nowCount;
            if (!(String.IsNullOrEmpty(nowCountEn))) model.FBirthNationlityPresentEn = brithCountryEn;
            if (brithday != DateTime.MinValue) model.FBirthDay = brithday;
            if (validday != DateTime.MinValue)
            {
                model.FPassportValidDate = validday;
                if (validday >= new DateTime(2012, 1, 1))
                {
                    model.FPassportMakeDate = validday.AddYears(-10).AddDays(1);
                }
                else
                {
                    model.FPassportMakeDate = validday.AddYears(-5).AddDays(1);
                }
            }
            if (sex != "") model.FSex = sex;
            if (!(String.IsNullOrEmpty(city)))
            {
                model.FBirthPlace = city;
                model.FBirthPlaceEn = ChineseToPinYin.Convert(city == null ? "" : city);
            }

        }



        public void ReadPassport(string code
    , out string name
    , out string passportType
    , out string nowCount
    , out string nowCountEn
    , out string passportNo
    , out string brithCountry
    , out string brithCountryEn
    , out DateTime brithday
    , out DateTime validday
    , out string sex
    , out string city)
        {
            try
            {
                if (code.Length < 80)
                {
                    MessageBox.Show("扫描证件出错，没有读取到完整的信息。");
                    name = "";
                    passportType = "";
                    nowCount = "";
                    nowCountEn = "";
                    passportNo = "";
                    brithCountry = "";
                    brithCountryEn = "";
                    brithday = DateTime.MinValue;
                    validday = DateTime.MinValue;
                    sex = "";
                    city = "";
                    return;
                }
                name = code.Substring(5, 39).Replace("<<", " ").Replace("<", " ").Trim();
                passportType = code.Substring(0, 1);
                string nowShort = code.Substring(2, 3);

                var nowCountryModel = visaORM.TCountry.FirstOrDefault(c => c.Fcountryshort == nowShort);
                if (nowCountryModel != null)
                {
                    nowCount = nowCountryModel.FCountry;
                    nowCountEn = nowCountryModel.FCountryEn;
                }
                else
                {
                    nowCount = "";
                    nowCountEn = "";
                }
                passportNo = code.Substring(44, 9);
                string bShort = code.Substring(54, 3);

                var birthCountryModel = visaORM.TCountry.FirstOrDefault(c => c.Fcountryshort == bShort);
                if (nowCountryModel != null)
                {
                    brithCountry = birthCountryModel.FCountry;
                    brithCountryEn = birthCountryModel.FCountryEn;
                }
                else
                {
                    brithCountry = "";
                    brithCountryEn = "";
                }


                string bday = code.Substring(57, 2) + "." + code.Substring(59, 2) + "." + code.Substring(61, 2);
                string vday = code.Substring(65, 2) + "." + code.Substring(67, 2) + "." + code.Substring(69, 2);
                DateTime.TryParse(bday, out brithday);
                DateTime.TryParse(vday, out validday);
                if (code.Substring(64, 1) == "M")
                {
                    sex = "男";
                }
                else
                {
                    sex = "女";
                }
                string cityNo = code.Substring(76, 4) + "00";
                city = "";
                var cityModel = visaORM.IDC.FirstOrDefault(c => c.FBM == cityNo);
                if (cityModel != null)
                    city = cityModel.FDQ;
            }
            catch (Exception err)
            {
                MessageBox.Show("护照读取错误。");
                name = "";
                passportType = "";
                nowCount = "";
                nowCountEn = "";
                passportNo = "";
                brithCountry = "";
                brithCountryEn = "";
                brithday = DateTime.MinValue;
                validday = DateTime.MinValue;
                sex = "";
                city = "";
            }
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                this.model.FNameEn = ChineseToPinYin.Convert(txtName.Text == null ? "" : txtName.Text);
                txtName.Text = txtName.Text.ToUpper();
            }

        }

        private void cbBirthPlace_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cbBirthPlace.SelectedIndex != -1 && !cbBirthPlace.EditValue.Equals("其他"))
            {
                model.FBirthPlaceEn = ChineseToPinYin.Convert(cbBirthPlace.SelectedItem == null ? "" : (cbBirthPlace.SelectedItem as PlaceClass).FPlaceName);
            }
        }

        private void CountryCN1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryCN1.SelectedIndex != -1)
            {
                //CountryEN1.SelectedItem = dictCountry[(CountryCN1.SelectedItem as TCountry).FCountryEn];
                model.FBirthNationlityEn = (CountryCN1.SelectedItem as TCountry).FCountryEn;
            }
        }

        private void CountryEN1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryEN1.SelectedIndex != -1)
                model.FBirthNationlity = (CountryEN1.SelectedItem as TCountry).FCountry;
        }


        private void CountryCN2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryCN2.SelectedIndex != -1)
                model.FBirthNationlityPresentEn = (CountryCN2.SelectedItem as TCountry).FCountryEn;
        }

        private void CountryEN2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryEN2.SelectedIndex != -1)
                model.FBirthNationlityPresent = (CountryEN2.SelectedItem as TCountry).FCountry;
        }


        private void dateDurationBegin_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (!isCanChangeTime)
            {
                isCanChangeTime = true;
                return;
            }
            if (!string.IsNullOrEmpty(dateDurationBegin.Text) && cbDuration.SelectedItem != null)
            {

                SetDurationEnd(cbDuration.Text);
                return;
            }
            if (string.IsNullOrEmpty(dateDurationBegin.Text))
            {
                dateDurationEnd.Text = "";
            }
        }

        /// <summary>
        /// 设置出境结束时间
        /// </summary>
        /// <param name="DurationValue"></param>
        private void SetDurationEnd(string DurationValue)
        {
            int months;
            switch (DurationValue)
            {
                case "1个月":
                    months = 1;
                    break;
                case "3个月":
                    months = 3;
                    break;
                case "6个月":
                    months = 6;
                    break;
                case "1年":
                    months = 12;
                    break;
                case "2年":
                    months = 24;
                    break;
                case "3年":
                    months = 36;
                    break;
                case "4年":
                    months = 48;
                    break;
                case "5年":
                    months = 60;
                    break;
                default:
                    months = 0;
                    break;

            }
            this.model.FDurationEnd = System.Convert.ToDateTime(dateDurationBegin.EditValue).AddMonths(months);
            dateDurationEnd.EditValue = System.Convert.ToDateTime(dateDurationBegin.EditValue).AddMonths(months);

        }


        //private void cbPurpose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count > 0)
        //    {
        //        if (e.AddedItems[0].Equals("旅游"))
        //            cbDuration.SelectedIndex = 0;
        //    }

        //}

        private void cbDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //if ((e.AddedItems[0] as ItemSourceClass).FName == "1个月")
                //    this.model.FTimes=1;
                //if (dateDurationBegin.EditValue != null && dateDurationEnd.EditValue != null)
                if (model.FDurationBegin != null && model.FDurationEnd != null && isCanChangeTime == true)
                    SetDurationEnd((cbDuration.SelectedItem as ItemSourceClass).FName);
            }
        }

        private void dateValiDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                //新代码，暂时不使用
                //dateMakeDate.EditValue = GetRelateDate(dateValiDate,e.NewValue,2);
                //System.Convert.ToDateTime(e.NewValue as DateTime?).AddYears(diffYears).AddDays(1);
            }



        }


        private void dateMakeDate_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                if (this.model.FPassportMakeDate == null)
                {
                    this.model.FPassportMakeDate = dateMakeDate.DateTime;
                }
                if ((DateTime)(e.NewValue) < new DateTime(2007, 1, 1))
                {
                    this.model.FPassportValidDate = ((DateTime)(e.NewValue)).AddYears(5).AddDays(-1);
                }
                else
                {
                    this.model.FPassportValidDate = ((DateTime)(e.NewValue)).AddYears(10).AddDays(-1);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="de"></param>
        /// <param name="valueStrig"></param>
        /// <param name="deSource">是发照日还是有效日， 1 ： 发照日 2： 有效日</param>
        /// <returns></returns>
        private DateTime GetRelateDate(DateEdit de, object valueStrig, int deSource)
        {
            int diffYears = deSource == 1 ? 10 : -10;

            if (de.EditValue != null)
            {
                if ((DateTime.Now).Subtract((DateTime)model.FBirthDay).Days <= (16 * 365)) //小于16周岁
                {
                    diffYears = deSource == 1 ? 5 : -5;
                }
            }
            if (model.FPassportNo != null)
            {
                if (model.FPassportNo.Substring(0, 1) == "S")  //公务护照
                {
                    diffYears = deSource == 1 ? 5 : -5;
                }
            }

            if (deSource == 1)
            {
                return System.Convert.ToDateTime(valueStrig as DateTime?).AddYears(diffYears).AddDays(-1);
            }
            else
            {
                return System.Convert.ToDateTime(valueStrig as DateTime?).AddYears(diffYears).AddDays(1);
            }

        }

        private void MainTab_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    System.Windows.Forms.SendKeys.Send("{Tab}");    
            // }     
        }


        private void LeftGrid_KeyDown(object sender, KeyEventArgs e)
        {
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // MoveFocus takes a TraveralReqest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }


        private void txtNameEn_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtNameEn.Text != null)
                this.txtNameEn.Text = this.txtNameEn.Text.ToUpper();
        }

        private void txtPassportNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtPassportNo.Text != null)
                this.txtPassportNo.Text = this.txtPassportNo.Text.ToUpper();
        }

        private void cbBirthPlace_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Up || e.Key == Key.Down)
                    return;


                if (!string.IsNullOrEmpty(cbBirthPlace.AutoSearchText))
                {
                    //if (cbBirthPlace.Text.Length == 1 && char.IsLetter((char)(cbBirthPlace.Text.FirstOrDefault())))
                    //{
                    //    var placeList = this.dictPlace.Where(d => d.Key.Contains(cbBirthPlace.Text.ToUpper()));
                    //    if (placeList.Count() > 0)
                    //        cbBirthPlace.ItemsSource = placeList;
                    //}
                    if (cbBirthPlace.AutoSearchText.Length == 2 && char.IsLetter((char)(cbBirthPlace.AutoSearchText.FirstOrDefault())))
                    {
                        List<PlaceClass> filterPlaceList = this.placeList.Where(p => p.FPinYin.Contains(cbBirthPlace.AutoSearchText.ToUpper())).ToList();
                        if (filterPlaceList.Count() == 1)
                        {
                            cbBirthPlace.SelectedItem = filterPlaceList.FirstOrDefault();
                        }
                        else if (filterPlaceList.Count() > 1)
                        {
                            cbBirthPlace.ItemsSource = filterPlaceList;
                            (sender as ComboBoxEdit).IsPopupOpen = true;
                        }
                        else
                            cbBirthPlace.ItemsSource = placeList;
                    }
                    else
                    {
                        cbBirthPlace.ItemsSource = placeList;
                    }

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.WriteLog.WriteErorrLog(ex);
            }

        }

        private void LeftGrid_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (e.OldItem == null || e.NewItem == null)
                return;

            Customer custModel = e.NewItem as Customer;


            var tempModel = visaORM.Customer.FirstOrDefault(c => c.FID == custModel.FID);
            if (tempModel == null)
                return;
            else
            {
                this.model = tempModel;
                isCanChangeTime = false;
                MainTab.DataContext = this.model;

            }

        }


        private void LeftGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Customer custModel = LeftGrid.CurrentItem as Customer;

            var tempModel = visaORM.Customer.FirstOrDefault(c => c.FID == custModel.FID);
            if (tempModel == null)
                return;
            else
            {
                this.model = tempModel;
                isCanChangeTime = false;
                MainTab.DataContext = this.model;

            }

        }

        private void txtBirhthPlaceEn_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txtBirhthPlaceEn.Text != null)
                this.txtBirhthPlaceEn.Text = this.txtBirhthPlaceEn.Text.ToUpper();
        }


    }




    public class ItemSourceClass
    {
        public string FName { get; set; }
        public int FID { get; set; }


        public List<ItemSourceClass> ListDuration()
        {
            List<ItemSourceClass> l = new List<ItemSourceClass>();
            l.Add(new ItemSourceClass { FName = "1个月", FID = 1 });
            l.Add(new ItemSourceClass { FName = "3个月", FID = 2 });
            l.Add(new ItemSourceClass { FName = "6个月", FID = 3 });
            l.Add(new ItemSourceClass { FName = "1年", FID = 4 });
            l.Add(new ItemSourceClass { FName = "2年", FID = 5 });
            l.Add(new ItemSourceClass { FName = "3年", FID = 6 });
            l.Add(new ItemSourceClass { FName = "4年", FID = 7 });
            l.Add(new ItemSourceClass { FName = "5年", FID = 8 });


            return l;
        }

        public List<ItemSourceClass> ListTimes()
        {
            List<ItemSourceClass> l = new List<ItemSourceClass>();
            l.Add(new ItemSourceClass { FName = "1次", FID = 1 });
            l.Add(new ItemSourceClass { FName = "多次", FID = 2 });

            return l;
        }

        public List<ItemSourceClass> ListSpeed()
        {
            List<ItemSourceClass> l = new List<ItemSourceClass>();
            l.Add(new ItemSourceClass { FName = "N-", FID = 3 });
            l.Add(new ItemSourceClass { FName = "N+", FID = 1 });
            l.Add(new ItemSourceClass { FName = "O", FID = 2 });

            return l;
        }



    }


    public class PlaceClass
    {
        public string FPinYin { get; set; }
        public string FPlaceName { get; set; }

    }
}
