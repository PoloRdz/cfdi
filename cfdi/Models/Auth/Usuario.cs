using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models.Auth
{
    public class Usuario
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellidoM { get; set; }
        public string apellidoP { get; set; }
        public string correo { get; set; }

    }
}
