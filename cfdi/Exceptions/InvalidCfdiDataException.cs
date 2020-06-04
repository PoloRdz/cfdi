using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidCfdiDataException : Exception
    {
        public InvalidCfdiDataException() : base() { }
        public InvalidCfdiDataException(string message) : base(message) { }
        public InvalidCfdiDataException(string message, Exception inner) : base(message, inner) { }
    }
}
