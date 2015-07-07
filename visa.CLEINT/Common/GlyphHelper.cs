using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using visa.CLEINT.View;

namespace visa.CLEINT.Common
{
    public static class GlyphHelper
    {

        public static ImageSource GetGlyph(string ItemPath)
        {
            return new BitmapImage(AssemblyHelper.GetResourceUri(Assembly.GetExecutingAssembly(), ItemPath));
        }
    }
}
