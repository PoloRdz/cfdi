using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Servidor
    {
        public int idServidor { get; set; }
        public string servidor { get; set; }
        public string descripcion { get; set; }
        public string identificados { get; set; }
        public bool eliminado { get; set; }
    }
}
