using System;
using System.Windows.Forms;

namespace LoginManager.Views
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            var usuarioForm = new UsuarioForm();
            usuarioForm.ShowDialog();
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            var rolesForm = new RolesForm();
            rolesForm.ShowDialog();
        }

        private void btnAsignarRol_Click(object sender, EventArgs e)
        {
            var asignarRolForm = new AsignarRolForm();
            asignarRolForm.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }
    }
}
