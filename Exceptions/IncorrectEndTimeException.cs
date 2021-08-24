using System;

namespace ScooterRental.Exceptions
{
    public class IncorrectEndTimeException : Exception
    {
        public IncorrectEndTimeException() : base("End time must be later than start time")
        {
        }
    }
}
