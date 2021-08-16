using System;
using System.Collections.Generic;
using System.IO;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        public string Name;
        public static List<Scooter> scooters = new List<Scooter>();

        public ScooterService(string name)
        {
            Name = name;
        }
        
        public void AddScooter(string id, decimal pricePerMinute)
        {
            Scooter scooter = new Scooter(id, pricePerMinute);
            scooters.Add(scooter);
        }

        public void AddScooterFromFile(string name)
        {
            var text = File.ReadAllText($"..\\..\\{name}.txt");
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] words = lines[i].Split(' ');
                scooters.Add(new Scooter(words[0], Convert.ToDecimal(words[1])));
            }
        }

        public void RemoveScooter(string id)
        {
            for (int i = 0; i < scooters.Count; i++)
            {
                if (scooters[i].Id == id && scooters[i].IsRented == false)
                    scooters.Remove(scooters[i]);
            }
        }

        public IList<Scooter> GetScooters()
            {
                return scooters;
            }

        public Scooter GetScooterById(string scooterId)
        {
            Scooter scooter = null;
            for (int i = 0; i < scooters.Count; i++)
            {
                if (scooters[i].Id == scooterId)
                    scooter = scooters[i];
            }

            return scooter;
        }

        public Scooter RentOutScooter(string id)
        {
            Scooter scooter = GetScooterById(id);
            if (scooter.IsRented == false)
            {
                scooter.IsRented = true;
            }
            else Console.WriteLine("This scooter is not available");

            return scooter;
        }

        public Scooter ReceiveBack(string id)
        {
            Scooter scooter = GetScooterById(id);
            scooter.IsRented = false;
            return scooter;
        }
    }
}
