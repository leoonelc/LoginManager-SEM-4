using LoginManager.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace LoginManager.Controllers
{
    public class Cotro
    {
        private readonly string connectionString;

        public Cotro()
        {
            connectionString = "server=localhost;port=3306;database=asignacion_y_gestion;uid=root;pwd=;";
        }

        public bool ValidarUsuario(string correo, string password)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Password FROM usuarios WHERE Correo = @correo LIMIT 1";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);
                    var resultado = cmd.ExecuteScalar();
                    return resultado != null && resultado.ToString() == password;
                }
            }
        }

        public bool AgregarUsuario(Usuario usuario)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO usuarios 
                    (Nombres, Apellido, Correo, AnioNacimiento, Telefono, Password, RolesId, PaisId, ProvinciaId) 
                    VALUES (@nombres, @apellido, @correo, @anio, @telefono, @password, @rol, @pais, @provincia)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@anio", usuario.AnioNacimiento);
                    cmd.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);
                    cmd.Parameters.AddWithValue("@rol", usuario.RolesId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pais", usuario.PaisId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@provincia", usuario.ProvinciaId ?? (object)DBNull.Value);
                    try { return cmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }

        public bool EditarUsuario(Usuario usuario)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE usuarios SET
                    Nombres=@nombres, Apellido=@apellido, Correo=@correo, AnioNacimiento=@anio, Telefono=@telefono, Password=@password,
                    RolesId=@rol, PaisId=@pais, ProvinciaId=@provincia
                    WHERE UsuarioId=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                    cmd.Parameters.AddWithValue("@anio", usuario.AnioNacimiento);
                    cmd.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);
                    cmd.Parameters.AddWithValue("@rol", usuario.RolesId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pais", usuario.PaisId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@provincia", usuario.ProvinciaId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", usuario.UsuarioId);
                    try { return cmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            var lista = new List<Usuario>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT UsuarioId, Nombres, Apellido, Correo, AnioNacimiento, Telefono, Password, RolesId, PaisId, ProvinciaId 
                    FROM usuarios";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            UsuarioId = reader.GetInt32("UsuarioId"),
                            Nombres = reader.GetString("Nombres"),
                            Apellido = reader.GetString("Apellido"),
                            Correo = reader.GetString("Correo"),
                            AnioNacimiento = reader.GetInt32("AnioNacimiento"),
                            Telefono = reader.GetString("Telefono"),
                            Password = reader.GetString("Password"),
                            RolesId = reader.IsDBNull(reader.GetOrdinal("RolesId")) ? (int?)null : reader.GetInt32("RolesId"),
                            PaisId = reader.IsDBNull(reader.GetOrdinal("PaisId")) ? (int?)null : reader.GetInt32("PaisId"),
                            ProvinciaId = reader.IsDBNull(reader.GetOrdinal("ProvinciaId")) ? (int?)null : reader.GetInt32("ProvinciaId")
                        });
                    }
                }
            }
            return lista;
        }

        public bool AgregarRol(Rol rol)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO roles (Detalle) VALUES (@detalle)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@detalle", rol.Detalle);
                    try { return cmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }

        public bool EditarRol(Rol rol)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE roles SET Detalle = @detalle WHERE RolesId = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@detalle", rol.Detalle);
                    cmd.Parameters.AddWithValue("@id", rol.RolesId);
                    try { return cmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }

        public List<Rol> ObtenerRoles()
        {
            var lista = new List<Rol>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT RolesId, Detalle FROM roles";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Rol
                        {
                            RolesId = reader.GetInt32("RolesId"),
                            Detalle = reader.GetString("Detalle")
                        });
                    }
                }
            }
            return lista;
        }

        public bool EliminarRol(Rol rol)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Verificar si el rol está en uso
                string verificarSql = "SELECT COUNT(*) FROM usuarios WHERE RolesId = @id";
                using (var verificarCmd = new MySqlCommand(verificarSql, conn))
                {
                    verificarCmd.Parameters.AddWithValue("@id", rol.RolesId);
                    int usados = Convert.ToInt32(verificarCmd.ExecuteScalar());

                    if (usados > 0)
                        return false; // Está en uso, no se puede eliminar
                }

                // Eliminar si no está en uso
                string eliminarSql = "DELETE FROM roles WHERE RolesId = @id";
                using (var eliminarCmd = new MySqlCommand(eliminarSql, conn))
                {
                    eliminarCmd.Parameters.AddWithValue("@id", rol.RolesId);
                    try { return eliminarCmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }

        public bool AsignarRolUsuario(int usuarioId, int rolId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE usuarios SET RolesId = @rolId WHERE UsuarioId = @usuarioId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@rolId", rolId);
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    try { return cmd.ExecuteNonQuery() > 0; } catch { return false; }
                }
            }
        }
    }
}
