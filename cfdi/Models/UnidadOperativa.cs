using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class UnidadOperativa
    {
        public int idUnidadOperativa { get; set; }
        public string unidadOperativa { get; set; }
        public string descripcion { get; set; }
        public string identificador { get; set; }
        public string telefono { get; set; }
        public string calle { get; set; }
        public string numeroExterior { get; set; }
        public string numeroInterior { get; set; }
        public string colonia { get; set; }
        public string poblacion { get; set; }
        public string codigoPostal { get; set; }
        public string municipio { get; set; }
        public double impAplicables { get; set; }
        public Zona zona { get; set; }
        public RazonSocial razonSocial { get; set; }
        public Servidor servidor { get; set; }
        public bool eliminado { get; set; }
    }
}
