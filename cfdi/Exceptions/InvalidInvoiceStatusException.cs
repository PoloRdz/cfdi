using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidInvoiceStatusException : Exception
    {
        public InvalidInvoiceStatusException() : base() { }
        public InvalidInvoiceStatusException(string message) : base(message) { }
        public InvalidInvoiceStatusException(string message, Exception inner) : base(message, inner) { }
    }
}
