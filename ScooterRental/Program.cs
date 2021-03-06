using System;
using System.Collections.Generic;
using ScooterRental.Interfaces;
using ScooterRental.Objects;
using ScooterRental.Processing;

namespace ScooterRental
{
    class Program
    {
        public static ReadingAndSavingScootersAndPayments ReadAndSaveFiles = new ReadingAndSavingScootersAndPayments();
        public static List<Scooter> Scooters = ReadAndSaveFiles.ReadScootersFromFile();
        public static IScooterService Service = new ScooterService("City scooters", Scooters);
        public static List<Payment> Payments = ReadAndSaveFiles.ReadPaymentsFromFile();
        public static Accounting Account = new Accounting(Payments);
        public static IRentalCompany Rental = new RentalCompany("City scooters", Service, Account);
        

        public static void Main(string[] args)
        {
            MakeChoice();
            ReadAndSaveFiles.SaveScooters();
            ReadAndSaveFiles.SavePayments();
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
                }
            } while (choice != "0");
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
            Scooter scooter = Service.GetScooterById(id);
            decimal pricePerMinute = scooter.PricePerMinute;
            Account.StartRenting(id, time, pricePerMinute);
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
    }
}
