using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using TilesetGenerator.Views;

namespace TilesetGenerator
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Telerik.WinControls.Themes.VisualStudio2012DarkTheme theme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView());
        }
    }
}
