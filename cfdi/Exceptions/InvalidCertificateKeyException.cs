using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidCertificateKeyException : CertificateException
    {
        public InvalidCertificateKeyException() { }
        public InvalidCertificateKeyException(string message) : base(message) { }
        public InvalidCertificateKeyException(string message, Exception inner) : base(message, inner) { }
    }
}
