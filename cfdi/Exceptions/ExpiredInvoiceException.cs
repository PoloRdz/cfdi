using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class ExpiredInvoiceException : Exception
    {
        public ExpiredInvoiceException() : base() { }
        public ExpiredInvoiceException(string message) : base(message) { }
        public ExpiredInvoiceException(string message, Exception inner) : base(message, inner) { }
    }
}
