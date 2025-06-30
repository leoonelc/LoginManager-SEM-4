using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginManager.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string Apellido { get; set; }
        public int AnioNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Password { get; set; }
        public int? RolesId { get; set; }
        public int? PaisId { get; set; }
        public int? ProvinciaId { get; set; }
    }
}
