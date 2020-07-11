using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException() : base() { }
        public DuplicateEmailException(string message) : base(message) { }
        public DuplicateEmailException(string message, Exception inner) : base(message, inner) { }
    }
}
