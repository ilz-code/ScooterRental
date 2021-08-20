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
            if (id == "scooters")
            {
                var text = File.ReadAllText($"..\\..\\scooters.txt");
                string[] lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] words = lines[i].Split(' ');
                    scooters.Add(new Scooter(words[0], Convert.ToDecimal(words[1])));
                }
            }
            else
            {
                Scooter scooter = new Scooter(id, pricePerMinute);
                scooters.Add(scooter);
            }
        }

        public void RemoveScooter(string id)
        {
            Scooter scooter = GetScooterById(id);
            try
            {
                scooters.Remove(scooter);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                throw;
            }
            //if (scooter.IsRented == false)
            //    scooters.Remove(scooter);
            //else 
            //    Console.WriteLine("This scooter is rented out now");
        }

        public IList<Scooter> GetScooters()
            {
                return scooters;
            }

        public Scooter GetScooterById(string scooterId)
        {
            Scooter scooter = scooters.Find(sc => sc.Id ==scooterId);
            return scooter;
        }
    }
}
