using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visa.CLEINT
{
  public static  class MainContext
    {
        private static string companyName = String.Empty;
        private static int userID = 1 ;
        private static string userName = "";
        private static string userWorkCompanyName = String.Empty;
        private static string userPhoneNo = String.Empty;


        public static string UserCompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }
        public static int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }

        public static string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }


        public static string UserWorkCompanyName
        {
            get
            {
                return userWorkCompanyName;
            }
            set
            {
                userWorkCompanyName = value;
            }
        }
        public static string UserPhoneNo
        {
            get
            {
                return userPhoneNo;
            }
            set
            {
                userPhoneNo = value;
            }
        }
    }
}
