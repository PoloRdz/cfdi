using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class RegimenFiscal
    {
        public int idRegimenFiscal { get; set; }
        public string descripcion { get; set; }
        public bool personaFisica { get; set; }
        public bool personaMoral { get; set; }
    }
}
