using System;

namespace ScooterRental.Objects
{
    public class Payment
    { 
        public string Id;
        public DateTime StartTime;
        public DateTime EndTime;
        public decimal PricePerMinute;
        public decimal SumPay;

        public Payment(string id, DateTime timeS, decimal pricePerMinute)
        {
            Id = id;
            StartTime = timeS;
            PricePerMinute = pricePerMinute;
        }
    }
}
