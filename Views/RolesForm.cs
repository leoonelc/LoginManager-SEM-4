using System;
using System.Windows.Forms;
using LoginManager.Controllers;
using LoginManager.Models;
using System.Collections.Generic;

namespace LoginManager.Views
{
    public partial class RolesForm : Form
    {
        private readonly Cotro controlador = new Cotro();
        private bool modoEdicion = false; // false = agregar, true = editar

        public RolesForm()
        {
            InitializeComponent();
            ConfigurarFormularioInicial();
            CargarRoles();
        }

        private void ConfigurarFormularioInicial()
        {
            txtDetalle.Enabled = false;
            btnGuardar.Enabled = false;
            btnEditar.Enabled = true;
            btnAgregar.Enabled = true;
            btnEliminar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void CargarRoles()
        {
            try
            {
                List<Rol> roles = controlador.ObtenerRoles();
                dgvRoles.DataSource = roles;
                dgvRoles.ClearSelection();
                LimpiarCampos();
                ConfigurarFormularioInicial();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando roles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtDetalle.Clear();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            modoEdicion = false;          // estamos agregando nuevo rol
            LimpiarCampos();
            txtDetalle.Enabled = true;
            txtDetalle.Focus();

            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = true;

            dgvRoles.ClearSelection();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un rol para editar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            modoEdicion = true;           // modo edición activo
            txtDetalle.Enabled = true;
            txtDetalle.Focus();

            btnGuardar.Enabled = true;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDetalle.Text))
            {
                MessageBox.Show("El detalle del rol no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool exito;

                if (modoEdicion)
                {
                    if (dgvRoles.CurrentRow == null)
                    {
                        MessageBox.Show("No hay rol seleccionado para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var rolEdit = (Rol)dgvRoles.CurrentRow.DataBoundItem;
                    rolEdit.Detalle = txtDetalle.Text.Trim();
                    exito = controlador.EditarRol(rolEdit);
                    if (exito)
                        MessageBox.Show("Rol actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al actualizar rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var nuevoRol = new Rol
                    {
                        Detalle = txtDetalle.Text.Trim()
                    };
                    exito = controlador.AgregarRol(nuevoRol);
                    if (exito)
                        MessageBox.Show("Rol agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Error al agregar rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (exito)
                {
                    CargarRoles();
                    LimpiarCampos();
                    ConfigurarFormularioInicial();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un rol para eliminar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rol = (Rol)dgvRoles.CurrentRow.DataBoundItem;

            var resultado = MessageBox.Show($"¿Está seguro que desea eliminar el rol '{rol.Detalle}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    bool exito = controlador.EliminarRol(rol);
                    if (exito)
                    {
                        MessageBox.Show("Rol eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarRoles();
                        LimpiarCampos();
                        ConfigurarFormularioInicial();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el rol.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ConfigurarFormularioInicial();
            dgvRoles.ClearSelection();
        }

        private void dgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                LimpiarCampos();
                return;
            }

            try
            {
                var rol = (Rol)dgvRoles.CurrentRow.DataBoundItem;
                txtDetalle.Text = rol.Detalle;

                // Deshabilitamos edición hasta que se presione editar o agregar
                txtDetalle.Enabled = false;
                btnGuardar.Enabled = false;
                btnAgregar.Enabled = true;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnCancelar.Enabled = true;

                modoEdicion = false; // Por defecto no estamos editando
            }
            catch
            {
                // Evitar error si cambia DataSource inesperadamente
            }
        }

        private void RolesForm_Load(object sender, EventArgs e)
        {
            // Opcional: puedes dejarlo vacío o iniciar configuraciones
        }
    }
}
