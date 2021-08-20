using System;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        public string Name { get; }

        public IScooterService Service;
        public Accounting Account;

        public RentalCompany(string name, IScooterService service, Accounting account)
        {
            Account = account;
            Service = service;
            Name = name;
        }

        public void StartRent(string id)
        {
            Scooter scooter = Service.GetScooterById(id);
            if (scooter.IsRented)
                throw new Exception("Scooter is rented.");
            else
                scooter.IsRented = true;
        }
        
        public decimal EndRent(string id)
        {
            Scooter scooter = Service.GetScooterById(id);
            scooter.IsRented = false;
            decimal pay = Account.GetPay(id);
            return pay;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal income = Account.CalculatingIncome(year, includeNotCompletedRentals);
            return income;
        }
    }
}
