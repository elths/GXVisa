using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using visa.CLEINT.Common;
using visa.MODEL;

namespace visa.CLEINT.ViewModel
{
   public  class VisaEditVM
    {
       visaEntities visaORM = new visaEntities();

       public ICommand MoveNextRow { get; private set; }

       public VisaEditVM()
       {
           this.MoveNextRow = new SimpleCommand(param =>
           {
               MessageBox.Show("aaaaaaaaaaaaa");
           });

       }
    }
}
