using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class CertificateException : Exception
    {
        public CertificateException() : base() { }
        public CertificateException(string message) : base(message) { }
        public CertificateException(string message, Exception inner) : base(message, inner) { }
    }
}
