using DevExpress.Xpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace visa.CLEINT.Model
{
    public class BarModel : ModelBase 
    {


        ObservableCollection<MyCommand> commandsCore = new ObservableCollection<MyCommand>();

        public ObservableCollection<MyCommand> Commands
        {
            get { return commandsCore; }
            set { SetProperty(ref commandsCore, value, "Commands"); }
        }

    }
}
