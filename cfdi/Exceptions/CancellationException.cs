using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class CancellationException : Exception
    {
        public CancellationException() { }
        public CancellationException(string message) : base(message) { }
        public CancellationException(string message, Exception inner) : base(message, inner) { } 
    }
}
