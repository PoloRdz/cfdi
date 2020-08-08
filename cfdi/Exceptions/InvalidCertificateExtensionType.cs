using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class InvalidCertificateExtensionType : Exception
    {
        public InvalidCertificateExtensionType() : base() { }
        public InvalidCertificateExtensionType(string message) : base(message) { }
        public InvalidCertificateExtensionType(string message, Exception inner) : base (message, inner) { }
    }
}
