using System;
using System.Windows.Forms;

using Guryflix.Forms;

namespace Guryflix
{
    static class Program
    {
        
        
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Guryflix.Dados.DatabaseContext.InitializeDatabase();
            Application.Run(new IntroPage());
        }
    }
}
