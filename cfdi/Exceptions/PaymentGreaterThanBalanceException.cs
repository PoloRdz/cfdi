using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Exceptions
{
    public class PaymentGreaterThanBalanceException : Exception
    {
        public PaymentGreaterThanBalanceException() : base() { }
        public PaymentGreaterThanBalanceException(string message) : base(message) { }
        public PaymentGreaterThanBalanceException(string message, Exception inner) : base(message, inner) { }
    }
}
