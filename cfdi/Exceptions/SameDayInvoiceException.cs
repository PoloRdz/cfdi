using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class SameDayInvoiceException : Exception
    {
        public SameDayInvoiceException() : base() { }
        public SameDayInvoiceException(string message) : base(message) { }
        public SameDayInvoiceException(string message, Exception inner) : base(message, inner) { }
    }
}
