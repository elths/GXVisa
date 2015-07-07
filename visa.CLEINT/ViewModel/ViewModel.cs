using DevExpress.Xpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visa.CLEINT.ViewModel
{
    public class ViewModel : BindableBase 
    {

        ObservableCollection<Model.BarModel> barsCore = new ObservableCollection<Model.BarModel>();

        public ObservableCollection<Model.BarModel> Bars
        {
            get { return barsCore; }
            set { SetProperty(ref barsCore, value, "Bars"); }
        }
    }
}
