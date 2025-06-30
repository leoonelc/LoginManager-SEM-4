using System;
using System.Windows.Forms;
using LoginManager.Controllers;
using LoginManager.Models;

namespace LoginManager.Views
{
    public partial class UsuarioForm : Form
    {
        private readonly Cotro controlador = new Cotro();

        public UsuarioForm()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = controlador.ObtenerUsuarios();
                dgvUsuarios.DataSource = usuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando usuarios: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (!int.TryParse(txtAnioNacimiento.Text, out int anioNacimiento))
            {
                MessageBox.Show("Ingrese un año válido.");
                return;
            }

            var usuario = new Usuario
            {
                Nombres = txtNombres.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                AnioNacimiento = anioNacimiento,
                Telefono = txtTelefono.Text.Trim(),
                Password = txtPassword.Text.Trim()
                // RolesId, PaisId, ProvinciaId pueden agregarse aquí si usas combos
            };

            try
            {
                bool exito = controlador.AgregarUsuario(usuario);
                if (exito)
                {
                    MessageBox.Show("Usuario agregado correctamente.");
                    CargarUsuarios();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al agregar usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para editar.");
                return;
            }

            if (!ValidarCampos()) return;

            if (!int.TryParse(txtAnioNacimiento.Text, out int anioNacimiento))
            {
                MessageBox.Show("Ingrese un año válido.");
                return;
            }

            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;
            usuario.Nombres = txtNombres.Text.Trim();
            usuario.Apellido = txtApellido.Text.Trim();
            usuario.Correo = txtCorreo.Text.Trim();
            usuario.AnioNacimiento = anioNacimiento;
            usuario.Telefono = txtTelefono.Text.Trim();
            usuario.Password = txtPassword.Text.Trim();

            try
            {
                bool exito = controlador.EditarUsuario(usuario);
                if (exito)
                {
                    MessageBox.Show("Usuario actualizado correctamente.");
                    CargarUsuarios();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            try
            {
                var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

                txtNombres.Text = usuario.Nombres;
                txtApellido.Text = usuario.Apellido;
                txtCorreo.Text = usuario.Correo;
                txtAnioNacimiento.Text = usuario.AnioNacimiento.ToString();
                txtTelefono.Text = usuario.Telefono;
                txtPassword.Text = usuario.Password;
            }
            catch
            {
                // Por si el DataSource cambia o hay problemas, evitar excepción
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtAnioNacimiento.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return false;
            }
            return true;
        }

        private void LimpiarCampos()
        {
            txtNombres.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtAnioNacimiento.Clear();
            txtTelefono.Clear();
            txtPassword.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
