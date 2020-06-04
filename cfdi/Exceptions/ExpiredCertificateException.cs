using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class ExpiredCertificateException : CertificateException
    {
        public ExpiredCertificateException() { }
        public ExpiredCertificateException(string message) : base(message) { }
        public ExpiredCertificateException(string message, Exception inner) : base(message, inner) { }
    }
}
