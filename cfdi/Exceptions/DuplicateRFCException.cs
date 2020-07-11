using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class DuplicateRFCException : Exception
    {
        public DuplicateRFCException() : base() { }
        public DuplicateRFCException(string message) : base(message) { }
        public DuplicateRFCException(string message, Exception inner) : base(message, inner) { }
    }
}
