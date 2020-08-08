using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Certificado
    {
        public int idCertificado { get; set; }
        public string cert { get; set; }
        public string descripcion { get; set; }
        public string identificador { get; set; }
        public string rutaCert { get; set; }
        public string key { get; set; }
        public string contrasena { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public bool eliminado { get; set; }
    }
}
