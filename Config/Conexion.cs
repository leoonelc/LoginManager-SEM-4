using System.Data;
using MySql.Data.MySqlClient;

namespace LoginManager.Config
{
    public class Conexion
    {
        private readonly string cadenaConexionMySql =
            "server=localhost;database=asignacion_y_gestion;uid=root;pwd=root;";

        private MySqlConnection conexionMySql;

        public IDbConnection AbrirConexion()
        {
            conexionMySql = new MySqlConnection(cadenaConexionMySql);
            conexionMySql.Open();
            return conexionMySql;
        }

        public void CerrarConexion()
        {
            if (conexionMySql != null && conexionMySql.State == ConnectionState.Open)
            {
                conexionMySql.Close();
            }
        }
    }
}
