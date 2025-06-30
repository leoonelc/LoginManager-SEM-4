using System;
using System.Windows.Forms;
using LoginManager.Controllers; 
namespace LoginManager.Views
{
    public partial class LoginForm : Form
    {
        private readonly LoginManager.Controllers.Cotro loginManager;

        public LoginForm()
        {
            InitializeComponent();
            loginManager = new LoginManager.Controllers.Cotro();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string password = txtPassword.Text;

            if (loginManager.ValidarUsuario(correo, password))
            {
                MessageBox.Show("Login exitoso");
                var menu = new MenuForm();
                menu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void lblCorreo_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }

        private void lblCorreo_Click_1(object sender, EventArgs e)
        {

        }
    }
}
