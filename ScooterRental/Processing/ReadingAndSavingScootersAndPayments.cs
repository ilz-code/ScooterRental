using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ScooterRental.Interfaces;
using ScooterRental.Objects;

namespace ScooterRental.Processing
{
    public class ReadingAndSavingScootersAndPayments
    {
        public IScooterService Service;
        public List<Scooter> Scooters;
        public List<Payment> Payments;

        public ReadingAndSavingScootersAndPayments()
        {
            Scooters = new List<Scooter>();
            Payments = new List<Payment>();
        }

        public List<Scooter> ReadScootersFromFile()
        {
            if (File.Exists(@"..\..\ScootersList.txt"))
            {
                var sc = File.ReadAllText(@"..\..\ScootersList.txt");
                Scooters = JsonConvert.DeserializeObject<List<Scooter>>(sc);
            }
            else 
            {
                var text = File.ReadAllText("..\\..\\scooters.txt");
                string[] lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] words = lines[i].Split(' ');
                    string Id = words[0];
                    decimal pricePerMinute = Convert.ToDecimal(words[1]);
                    Scooters.Add(new Scooter(Id, pricePerMinute));
                    //Service.AddScooter(Id, pricePerMinute);
                }
                //Scooters = Service.GetScooters();
            }

            return Scooters;
        }

        public void SaveScooters()
        {
            var scootersList = JsonConvert.SerializeObject(Scooters);
            File.WriteAllText(@"..\..\ScootersList.txt", scootersList);
        }

        public List<Payment> ReadPaymentsFromFile()
        {
            List<Payment> payments = new List<Payment>(); 

            if (File.Exists(@"..\..\PaymentsList.txt"))
            {
                var pm = File.ReadAllText(@"..\..\PaymentsList.txt");
                Payments = JsonConvert.DeserializeObject<List<Payment>>(pm);
            }
            else
            {
                var text = File.ReadAllText($"..\\..\\Payments.txt");
                string[] lines = text.Split('\n');
                for (int i = 0; i < 3; i++) //lines.Length
                {
                    string[] words = lines[i].Split(' ');
                    DateTime time = DateTime.Parse(words[1] + " " + words[2]);
                    decimal price = Convert.ToDecimal(words[3]);
                    Payments.Add(new Payment(words[0], time, price));
                }
            }
            return Payments;
        }

        public void SavePayments()
        {
            var paymentsList = JsonConvert.SerializeObject(Payments);
            File.WriteAllText(@"..\..\PaymentsList.txt", paymentsList);
        }
    }
}
