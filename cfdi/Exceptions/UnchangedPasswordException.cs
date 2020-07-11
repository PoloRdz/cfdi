using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class UnchangedPasswordException : Exception
    {
        public UnchangedPasswordException() : base() { }
        public UnchangedPasswordException(string message) : base(message) { }
        public UnchangedPasswordException(string message, Exception inner) : base(message, inner) { }
    }
}
