using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ScooterRental
{
    class Program
    {
        public static ScooterService Service = new ScooterService("City scooters");
        public static RentalCompany Rental = new RentalCompany("City scooters");

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
            Console.WriteLine(" Add scooter:" +
                              "\n Manually 1" +
                              "\n From file 2");
            string kind = Console.ReadLine();
            if (kind == "1")
            {
                Console.WriteLine("Enter ID");
                string id = Console.ReadLine();
                Console.WriteLine("Enter price per minute");
                decimal pricePerMinute = Convert.ToDecimal(Console.ReadLine());
                Service.AddScooter(id, pricePerMinute);
            }
            else if (kind == "2")
            {
                Console.WriteLine("Enter file name:");
                string name = Console.ReadLine();
                Service.AddScooterFromFile(name);
            }
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
            string time = Console.ReadLine();
            Scooter scooter = Service.RentOutScooter(id);
            scooter.TimeStart = DateTime.Parse(time);
            Rental.StartRent(scooter);
        }

        public static void ReturningScooter()
        {
            Console.WriteLine("Enter scooter ID");
            string id = Console.ReadLine();
            Console.WriteLine("Enter time (yyyy-mm-dd hh:mm:ss)");
            string time = Console.ReadLine();
            Scooter scooter = Service.ReceiveBack(id);
            scooter.TimeEnd = DateTime.Parse(time);
            Console.WriteLine(Rental.EndRent(scooter));
        }

        public static void CalculatingIncome()
        {
            Console.WriteLine("Enter year");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Include not completed rentals? (y/n)");
            bool notCompletedRentals = (Console.ReadLine() == "y") ? true : false;
            Console.WriteLine(Rental.CalculateIncome(year, notCompletedRentals));
        }
    }
}
