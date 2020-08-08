using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class ExcepcionInterna : Exception 
    {
        public ExcepcionInterna() : base() { }
        public ExcepcionInterna(string mensaje) : base(mensaje) { }
        public ExcepcionInterna(string mensaje, Exception excepcionInterna) : base(mensaje, excepcionInterna) { }
    }
}
