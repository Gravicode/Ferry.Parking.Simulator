using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace sample
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
            FormStart dlg = new FormStart();
            dlg.ShowDialog();
            if (dlg.Video)
                 Application.Run(new FormVideo());
            else
                Application.Run(new FormImages());
        }
    }
}
