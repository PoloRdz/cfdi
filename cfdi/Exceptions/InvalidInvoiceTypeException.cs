using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidInvoiceTypeException : Exception
    {
        public InvalidInvoiceTypeException() : base() { }
        public InvalidInvoiceTypeException(string message) : base(message) { }
        public InvalidInvoiceTypeException(string message, Exception inner) : base(message, inner) { }

    }
}
