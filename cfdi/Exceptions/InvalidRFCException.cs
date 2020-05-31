using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidRFCException : Exception
    {
        public InvalidRFCException() { }
        public InvalidRFCException(string message) : base(message) { }
        public InvalidRFCException(string message, Exception inner) : base(message, inner) { }
    }
}
