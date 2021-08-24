using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ScooterRental.Exceptions;


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
            else if (id == "From file")
            {
                var sc = File.ReadAllText(@"..\..\ScootersList.txt");
                scooters = JsonConvert.DeserializeObject<List<Scooter>>(sc);
            }
            else if (id == "Save to file")
            {
                var scootersList = JsonConvert.SerializeObject(scooters);
                File.WriteAllText(@"..\..\ScootersList.txt", scootersList);
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

            if (scooter.IsRented)
                throw new ScooterIsNotAvailableException();
            else
                scooters.Remove(scooter);
        }

        public IList<Scooter> GetScooters()
        {
            return scooters;
        }

        public Scooter GetScooterById(string scooterId)
        {
            Scooter scooter = scooters.Find(sc => sc.Id == scooterId);
            if (scooter == null)
                throw new ScooterIsNotAvailableException();
            
            return scooter;
        }
    }
}
