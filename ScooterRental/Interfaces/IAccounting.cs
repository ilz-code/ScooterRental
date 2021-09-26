using System;

namespace ScooterRental.Interfaces
{
    public interface IAccounting
    {
        void StartRenting(string id, DateTime time, decimal pricePerMinute);
        void EndRenting(string id, DateTime time);
        decimal GetPay(string id);
        decimal CalculatingIncome(int? year, bool includeNotCompletedRentals);
    }
}
