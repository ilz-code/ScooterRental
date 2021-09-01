using System;

namespace ScooterRental.Exceptions
{
    public class DoubleScooterIdException : Exception
    {
        public DoubleScooterIdException() : base("Scooter with such ID already exists")
        {}
    }
}
