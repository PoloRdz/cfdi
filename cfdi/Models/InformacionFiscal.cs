using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class InformacionFiscal
    {
        public int id { get; set; }
        public string rfc { get; set; }
        public string razonSocial { get; set; }
        public string direccionFiscal { get; set; }
        public string codigoPostal { get; set; }
        public string telefono { get; set; }
        public RegimenFiscal regimenFiscal { get; set; }
    }
}
