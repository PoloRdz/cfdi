using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class UsoCFDi
    {
        public string usoCFDi { get; set; }
        public string descripcion { get; set; }
        public string explicacion { get; set; }
        public bool fisica { get; set; }
        public bool moral { get; set; }
        public bool eliminado { get; set; }
    }
}
