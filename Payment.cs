﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ScooterRental
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
