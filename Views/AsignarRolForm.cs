using System;
using System.Windows.Forms;
using LoginManager.Controllers;

namespace LoginManager.Views
{
    public partial class AsignarRolForm : Form
    {
        // Cambié el nombre para que sea más claro el controlador
        private readonly Cotro controlador = new Cotro();

        public AsignarRolForm()
        {
            InitializeComponent();
        }

        private void AsignarRolForm_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarRoles();
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = controlador.ObtenerUsuarios();
                if (usuarios.Count == 0)
                {
                    MessageBox.Show("No hay usuarios disponibles para asignar rol.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbUsuarios.DataSource = usuarios;
                cmbUsuarios.DisplayMember = "Correo";
                cmbUsuarios.ValueMember = "UsuarioId";
                cmbUsuarios.SelectedIndex = -1; // para que no seleccione nada por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarRoles()
        {
            try
            {
                var roles = controlador.ObtenerRoles();
                if (roles.Count == 0)
                {
                    MessageBox.Show("No hay roles disponibles para asignar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbRoles.DataSource = roles;
                cmbRoles.DisplayMember = "Detalle";
                cmbRoles.ValueMember = "RolesId";
                cmbRoles.SelectedIndex = -1; // para que no seleccione nada por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando roles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar que haya una selección válida en ambos combos
            if (cmbUsuarios.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, selecciona un usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRoles.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, selecciona un rol.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = (int)cmbUsuarios.SelectedValue;
            int rolId = (int)cmbRoles.SelectedValue;

            try
            {
                bool exito = controlador.AsignarRolUsuario(usuarioId, rolId);
                if (exito)
                {
                    MessageBox.Show("Rol asignado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Opcional: refrescar combos o cerrar form
                }
                else
                {
                    MessageBox.Show("No se pudo asignar el rol. Intenta nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
