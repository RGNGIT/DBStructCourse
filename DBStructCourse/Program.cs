using System;
using System.Windows.Forms;

namespace DBStructCourse
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Auth());
        }

        public static string Server;
        public static string Security;
        public static string Database;

    }
}
