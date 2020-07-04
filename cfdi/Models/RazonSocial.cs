using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class RazonSocial
    {
        public int idRazonSocial { get; set; }
        public string razonSocial { get; set; }
        public string nombreCorto { get; set; }
        public string identificador { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string caller { get; set; }
        public string numeroExterior { get; set; }
        public string numeroInterior { get; set; }
        public string colonia { get; set; }
        public string codigoPostal { get; set; }
        public string municipio { get; set; }
        public RegimenFiscal regimenFiscal { get; set; }
    }
}
