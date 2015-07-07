using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using visa.MODEL;

namespace visa.CLEINT.ViewModel
{
   public  class UserAdminVM
    {
       visaEntities visaORM = new visaEntities();

       public ObservableCollection<TB_Menu> MenuList  = new ObservableCollection<TB_Menu>();

       public UserAdminVM()
       {
           foreach (TB_Menu menuObj in visaORM.TB_Menu.Where(m =>m.FType==1 && m.FStatus == true && m.FStatus==true))
           {
               MenuList.Add(menuObj);
           }

       }
    }
}
