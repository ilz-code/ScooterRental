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

        public static void Main(string[] args)
        {
            Service.AddScooter("From file", 1);
            Payments = Account.GetPayments();
            MakeChoice();
        }

        public static void MakeChoice()
        {
            string choice;
            do
            {
                Console.WriteLine("\n Choose what to do:" +
                                  "\n Add scooter - 1" +
                                  "\n Remove scooter - 2" +
                                  "\n Rent scooter - 3" +
                                  "\n Return scooter - 4" +
                                  "\n Calculate income - 5" +
                                  "\n End work - 0");
                choice = Console.ReadLine();

                switch (choice)
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
                        EndWork();
                        return;
                }
            } while (choice != "0");
        }

        public static void AddingScooter()
        {
            Console.WriteLine("Enter ID");
            string id = Console.ReadLine();
            Console.WriteLine("Enter price per minute");
            decimal pricePerMinute = 0;
            pricePerMinute = Convert.ToDecimal(Console.ReadLine());
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
            Account.EndRenting(id, time);
            decimal pay = Rental.EndRent(id);
            Console.WriteLine($"\n Calculated payment: {pay}");
        }

        public static void CalculatingIncome()
        {
            Console.WriteLine("Enter year");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Include not completed rentals? (y/n)");
            bool notCompletedRentals = (Console.ReadLine() == "y") ? true : false;
            Console.WriteLine($"\n Calculated income: {Rental.CalculateIncome(year, notCompletedRentals)}");
        }

        public static void EndWork()
        {
            Account.SavePayments();
            Service.AddScooter("Save to file", 1);
            Console.WriteLine("Goodbye!");
        }
    }
}
