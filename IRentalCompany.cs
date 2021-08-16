namespace ScooterRental
{
    public interface IRentalCompany
    {
        string Name { get; }
        void StartRent(Scooter scooter);
        decimal EndRent(Scooter scooter);
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
    }
}
