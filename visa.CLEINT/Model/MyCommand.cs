using DevExpress.Xpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace visa.CLEINT.Model
{
   public  class MyCommand : BindableBase, ICommand 
    {


        Action action;
        string captionCore = "";
        ImageSource largeGlyphCore = null;
        ImageSource smallGlyphCore = null;

        public MyCommand() { }
        public MyCommand(Action action)
        {
            this.action = action;
        }

        public string Caption
        {
            get { return captionCore; }
            set { SetProperty(ref captionCore, value, "Caption"); }
        }
        public ImageSource LargeGlyph
        {
            get { return largeGlyphCore; }
            set { SetProperty(ref largeGlyphCore, value, "LargeGlyph"); }
        }
        public ImageSource SmallGlyph
        {
            get { return smallGlyphCore; }
            set { SetProperty(ref smallGlyphCore, value, "SmallGlyph"); }
        }
        void ShowMSGBX()
        {
            MessageBox.Show(String.Format("Command \"{0}\" executed", this.Caption));
        }

        public bool CanExecute(object parameter) { return true; }
        public event EventHandler CanExecuteChanged { add { } remove { } }

        public virtual void Execute(object parameter)
        {
            if (action != null)
                action();
            //else
            //    ShowMSGBX();
        }
    }
}
