using System.Collections.Generic;

namespace ScooterRental
{
    public interface IRentalCompany
    {
        string Name { get; }
        List<Payment> StartRent(Scooter scooter);
        decimal EndRent(Scooter scooter);
        decimal CalculateIncome(List<Payment> payments, int? year, bool includeNotCompletedRentals);
    }
}
