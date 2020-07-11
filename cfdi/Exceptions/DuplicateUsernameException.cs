using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException() : base(){ }
        public DuplicateUsernameException(string message) : base(message) { }
        public DuplicateUsernameException(string message, Exception inner) : base(message, inner) { }
    }
}
