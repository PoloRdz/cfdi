using cfdi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Receptor
    {
        public Usuario usuario { get; set; }
        public InformacionFiscal informacionFiscal { get; set; }
    }
}
