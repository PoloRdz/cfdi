using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvoiceDateMismatchException : Exception
    {
        public InvoiceDateMismatchException() : base() { }
        public InvoiceDateMismatchException(string message) : base(message) { }
        public InvoiceDateMismatchException(string message, Exception inner) : base(message, inner) { }
    }
}
