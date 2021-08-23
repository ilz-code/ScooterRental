using System;

namespace ScooterRental.Exceptions
{
    public class ScooterIsNotAvailableException : Exception
    {
        public ScooterIsNotAvailableException() : base("Scooter is not available")
        {

        }

        public ScooterIsNotAvailableException(string message) : base(message)
        {

        }
    }
}
