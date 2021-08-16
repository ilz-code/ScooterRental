using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public interface IScooterService
    {
        void AddScooter(string id, decimal pricePerMinute);
        void RemoveScooter(string id);
        IList<Scooter> GetScooters();
        Scooter GetScooterById(string scooterId);
    }
}
