using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvoiceNumberAvailabilityException : Exception
    {
        public InvoiceNumberAvailabilityException() : base() { }
        public InvoiceNumberAvailabilityException(string message) : base(message) { }
        public InvoiceNumberAvailabilityException(string message, Exception inner) : base(message, inner) { }
    }
}
