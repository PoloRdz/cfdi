using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Pagos
    {
        public DateTime fechaPago { get; set; }
        public string formaDePagoP { get; set; }
        public string monedaP { get; set; }
        public double tipodeCambioP { get; set; }
        public double monto { get; set; }
        public string numOperacion { get; set; }
        public string ctaBeneficiario { get; set; }
        public string ctaOrdenante { get; set; }
        public DoctoRelacionado[] doctoRelacionados { get; set; }

    }
}
