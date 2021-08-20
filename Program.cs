using System;
using System.Collections.Generic;

namespace ScooterRental
{
    class Program
    {
        public static IScooterService Service = new ScooterService("City scooters");
        public static List<Payment> Payments;
        public static Accounting Account = new Accounting(Payments);
        public static IRentalCompany Rental = new RentalCompany("City scooters", Service, Account);

        static void Main(string[] args)
        {
            string choise;
            do
            {
                Console.WriteLine("\n Choose what to do:" +
                                  "\n Add scooter - 1" +
                                  "\n Remove scooter - 2" +
                                  "\n Rent scooter - 3" +
                                  "\n Return scooter - 4" +
                                  "\n Calculate income - 5" +
                                  "\n End work - 0");
                choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        AddingScooter();
                        break;
                    case "2":
                        RemovingScooter();
                        break;
                    case "3":
                        RentingScooter();
                        break;
                    case "4":
                        ReturningScooter();
                        break;
                    case "5":
                        CalculatingIncome();
                        break;
                    case "0":
                        return;
                }
            } while (choise != "0");

        }

        public static void AddingScooter()
        {
            Console.WriteLine("Enter ID");
            string id = Console.ReadLine();
            Console.WriteLine("Enter price per minute");
            decimal pricePerMinute = Convert.ToDecimal(Console.ReadLine());
            Service.AddScooter(id, pricePerMinute);
        }

        public static void RemovingScooter()
        {
            Console.WriteLine("Enter scooter ID");
            string id = Console.ReadLine();
            Service.RemoveScooter(id);
        }

        public static void RentingScooter()
        {
            Console.WriteLine("Enter scooter ID");
            string id = Console.ReadLine();
            Console.WriteLine("Enter time (yyyy-mm-dd hh:mm:ss)");
            DateTime time = DateTime.Parse(Console.ReadLine());
            Account.StartRenting(id, time);
            Rental.StartRent(id);
        }

        public static void ReturningScooter()
        {
            Console.WriteLine("Enter scooter ID");
            string id = Console.ReadLine();
            Console.WriteLine("Enter time (yyyy-mm-dd hh:mm:ss)");
            DateTime time = DateTime.Parse(Console.ReadLine());
            decimal pay = Account.EndRenting(id, time);
            Console.WriteLine($"\n Calculated payment: {pay}"); //{Rental.EndRent(id)}");
        }

        public static void CalculatingIncome()
        {
            Console.WriteLine("Enter year");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Include not completed rentals? (y/n)");
            bool notCompletedRentals = (Console.ReadLine() == "y") ? true : false;
            Console.WriteLine($"\n Calculated income: {Rental.CalculateIncome(year, notCompletedRentals)}");
        }
    }
}
