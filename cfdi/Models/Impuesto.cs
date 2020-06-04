using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Impuesto
    {
        public int idImpuesto { get; set; }
        public string tipo { get; set; }
        public double precioBase { get; set; }
        public string impuesto { get; set; }
        public string tipoFactor { get; set; }
        public double tasaOCuota { get; set; }
        public double importe { get; set; }
    }
}
