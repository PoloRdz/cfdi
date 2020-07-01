using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Folio
    {
        public int id { get; set; }
        public int folios { get; set; }
        public Emisor emisor { get; set; }
    }
}
