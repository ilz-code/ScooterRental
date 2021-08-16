using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class Scooter
    {
        public string Id { get; }
        public decimal PricePerMinute { get; }
        public bool IsRented { get; set; }
        public DateTime TimeStart;
        public DateTime TimeEnd;
        
        public Scooter(string id, decimal pricePerMinute)
        {
            Id = id;
            PricePerMinute = pricePerMinute;
            IsRented = false;
        }
    }
}

