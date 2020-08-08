using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Serie
    {
        public int id { get; set; }
        public string tipoVenta { get; set; }
        public string serie { get; set; }
        public Emisor emisor { get; set; }
        public bool eliminado { get; set; }
    }
}
