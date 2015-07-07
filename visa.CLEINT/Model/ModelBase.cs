using DevExpress.Xpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visa.CLEINT.Model
{
    public class ModelBase : BindableBase 
    {

        string nameCore = "";

        public string Name
        {
            get { return nameCore; }
            set { SetProperty(ref nameCore, value, "Name"); }
        }
    }
}
