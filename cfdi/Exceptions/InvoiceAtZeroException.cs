using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvoiceAtZeroException : Exception
    {
        public InvoiceAtZeroException() : base() { }
        public InvoiceAtZeroException(string message) : base(message) { }
        public InvoiceAtZeroException(string message, Exception inner) : base(message, inner) { }
    }
}
