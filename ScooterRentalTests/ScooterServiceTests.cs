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
    public class ScooterServiceTests
    {
        public IScooterService Service;
        public IRentalCompany Rental;
        public Accounting Account;
        public List<Payment> Payments;
        public List<Scooter> Scooters;
        public ReadingAndSavingScootersAndPayments ReadAndSaveFiles = new ReadingAndSavingScootersAndPayments();

        public ScooterServiceTests()
        {
            Payments = ReadAndSaveFiles.ReadPaymentsFromFile();
            Account = new Accounting(Payments);
            Scooters = ReadAndSaveFiles.ReadScootersFromFile();
            Service = new ScooterService("City scooters", Scooters);
            Rental = new RentalCompany("City scooters", Service, Account);
        }
        [TestMethod]
        public void AddScooters_Count()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            var count1 = Service.GetScooters().Count;
            //Act
            Service.AddScooter("13", 0.08m);
            //Assert
            int result = Service.GetScooters().Count;
            int expect = ++count1;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void RemoveScooters_Count()
        {
            //Arrange
            Service.AddScooter("scooters", 0.08m);
            var count1 = Service.GetScooters().Count;
            //Act
            Service.RemoveScooter("7");
            //Assert
            int result = Service.GetScooters().Count;
            int expect = --count1;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void GetScooterById_scooter()
        {
            Service.AddScooter("Scooters", 0.05m);
            decimal result = Service.GetScooterById("3").PricePerMinute;
            decimal expect = 0.05m;
            Assert.AreEqual(expect, result);
        }
        [TestMethod]
        public void RemoveScooter_IsRented_ScooterIsNotAvailableException()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            Rental.StartRent("2");
            //Assert
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Service.RemoveScooter("2"));
        }

        [TestMethod]
        public void GetScooterById_NonExistingId_ScooterIsNotAvailableException()
        {
            Service.AddScooter("scooters", 1);
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Service.GetScooterById("22"));
        }

        [TestMethod]
        public void AddScooter_ExistingId_DoubleScooterIdException()
        {
            Scooters = ReadAndSaveFiles.ReadScootersFromFile();
            Assert.ThrowsException<DoubleScooterIdException>
                (() => Service.AddScooter("1", 0.03m));
        }

        [TestMethod]
        public void AddScooter_NegativePrice_NegativePriceException()
        {
            Scooters = ReadAndSaveFiles.ReadScootersFromFile();
            Assert.ThrowsException<NegativePriceException>
                (() => Service.AddScooter("12", -0.03m));
        }
    }
}
