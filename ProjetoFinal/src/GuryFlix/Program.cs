using System;
using System.Windows.Forms;

using Guryflix.Forms;

namespace Guryflix
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Guryflix.Data.DatabaseContext.InitializeDatabase();
            Application.Run(new IntroPage());
        }
    }
}
