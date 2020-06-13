using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidInvoiceNumberException : Exception
    {
        public InvalidInvoiceNumberException() : base() { }
        public InvalidInvoiceNumberException(string message) : base(message) { }
        public InvalidInvoiceNumberException(string message, Exception inner) : base(message, inner) { }
    }
}
