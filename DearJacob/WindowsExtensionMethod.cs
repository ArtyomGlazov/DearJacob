using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DearJacob
{
    static class WindowsExtensionMethod
    {
        static public void ToCenterScreen (this Window window)
        {
            window.Top = (SystemParameters.FullPrimaryScreenHeight - window.Height) / 2;
            window.Left = (SystemParameters.FullPrimaryScreenWidth - window.Width) / 2;
        }
    }
}
