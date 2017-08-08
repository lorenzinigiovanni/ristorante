using System;
using System.Windows.Forms;

namespace Ristorante
{
    internal static class Program
    {
        /// <summary>
        ///     Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new SplashScreen().Show();
            Application.Run();
        }
    }
}