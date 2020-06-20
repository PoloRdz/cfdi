using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class WebServiceCommunicationException : Exception
    {
        public WebServiceCommunicationException() : base() { }
        public WebServiceCommunicationException(string message) : base(message) { }
        public WebServiceCommunicationException(string message, Exception inner) : base(message, inner) { }
    }
}
