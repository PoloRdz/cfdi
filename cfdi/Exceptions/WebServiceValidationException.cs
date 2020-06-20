using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class WebServiceValidationException : Exception
    {
        public WebServiceValidationException() : base() { }
        public WebServiceValidationException(string message) : base(message) { }
        public WebServiceValidationException(string message, Exception inner) : base(message, inner) { }
    }
}
