using System.Collections.Generic;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Objects;

namespace ScooterRental.Processing
{
    public class ScooterService : IScooterService
    {
        public string Name;
        public static List<Scooter> Scooters;

        public ScooterService(string name, List<Scooter> scooters)
        {
            Name = name;
            Scooters = scooters;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            foreach (var sc in Scooters)
            {
                if (sc.Id == id)
                    throw new DoubleScooterIdException();
            }

            if (pricePerMinute <= 0)
                throw new NegativePriceException();

            Scooter scooter = new Scooter(id, pricePerMinute);
            Scooters.Add(scooter);
        }

        public void RemoveScooter(string id)
        {
            Scooter scooter = GetScooterById(id);

            if (scooter.IsRented)
                throw new ScooterIsNotAvailableException();
            else
                Scooters.Remove(scooter);
        }

        public IList<Scooter> GetScooters()
        {
            return Scooters;
        }

        public Scooter GetScooterById(string scooterId)
        {
            Scooter scooter = Scooters.Find(sc => sc.Id == scooterId);
            if (scooter == null)
                throw new ScooterIsNotAvailableException();

            return scooter;
        }
    }
}
