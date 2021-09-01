using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Objects;
using ScooterRental.Processing;

namespace ScooterRentalTests
{
    [TestClass]
    public class RentalCompanyTests
    {
        public IScooterService Service;
        public IRentalCompany Rental;
        public Accounting Account;
        public List<Payment> Payments;
        public List<Scooter> Scooters;
        public ReadingAndSavingScootersAndPayments ReadAndSaveFiles = new ReadingAndSavingScootersAndPayments();

        public RentalCompanyTests()
        {
            Payments = new List<Payment>();
            Scooters = ReadAndSaveFiles.ReadScootersFromFile();
            Account = new Accounting(Payments);
            Service = new ScooterService("City scooters", Scooters);
            Rental = new RentalCompany("City scooters", Service, Account);
        }

        [TestMethod]
        public void StartRent_IsRented()
        {
            Service.AddScooter("Scooters", 0.01m);
            Rental.StartRent("3");
            bool result = Service.GetScooterById("3").IsRented;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EndRent_EndingRent_IsRentedFalse()
        {
            //Arrange
            Scooters.Clear();
            Service.AddScooter("2", 0.05m);
            Scooter scooter = Service.GetScooterById("2");
            scooter.IsRented = false;
            Rental.StartRent("2");
            DateTime timeStart = DateTime.Parse("2020-08-13 08:00:00");
            Account.StartRenting("2", timeStart, 0.05m);
            DateTime timeEnd = DateTime.Parse("2020-08-14 17:00:00");
            Account.EndRenting("2", timeEnd);
            //Act
            Rental.EndRent("2");
            bool result = Service.GetScooterById("2").IsRented;
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StartRent_IsRented_ScooterIsNotAvailableException()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            Rental.StartRent("5");
            //Assert
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Rental.StartRent("5"));
        }

        [TestMethod]
        public void EndRent_ScooterNotFound_ScooterIsNotAvailableException()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            //Assert
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Rental.EndRent("44"));
        }

        [TestMethod]
        public void EndRent_IsNotRented_ScooterIsNotAvailableException()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            Scooter scooter = Service.GetScooterById("3");
            scooter.IsRented = false;
            //Assert
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Rental.EndRent("3"));
        }
    }
}
