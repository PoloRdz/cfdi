using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Emisor
    {
        public UnidadOperativa unidadOperativa { get; set; }
        public string serie { get; set; }
        public Certificado certificado { get; set; }
    }
}
