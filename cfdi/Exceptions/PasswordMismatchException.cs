using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException() : base() { }
        public PasswordMismatchException(string message) : base(message) { }
        public PasswordMismatchException(string message, Exception inner) : base(message, inner) { }
    }
}
