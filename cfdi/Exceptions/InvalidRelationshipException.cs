using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidRelationshipException : Exception
    {
        public InvalidRelationshipException() : base() { }
        public InvalidRelationshipException(string message) : base(message) { }
        public InvalidRelationshipException(string message, Exception inner) : base(message, inner) { }
    }
}
