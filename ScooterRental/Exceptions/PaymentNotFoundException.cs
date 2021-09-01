using System;

namespace ScooterRental.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException() : base("Payment not found")
        {
        }
    }
}
