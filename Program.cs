using System;
using System.Windows.Forms;
using LoginManager.Views;

namespace LoginManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Iniciar mostrando el formulario LoginForm
            Application.Run(new LoginForm());
        }
    }
}
