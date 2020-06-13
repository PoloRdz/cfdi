using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Emisor
    {
        public int idSucursal { get; set; }
        public string sucursal { get; set; }
        public string rfcSucursal { get; set; }
        public string regimenFiscal { get; set; }
        public Certificado certificado { get; set; }
    }
}
