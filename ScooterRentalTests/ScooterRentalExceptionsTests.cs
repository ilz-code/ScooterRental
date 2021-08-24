using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRental;
using ScooterRental.Exceptions;

namespace ScooterRentalTests
{
    [TestClass]
    public class ScooterRentalExceptionsTests
    {
        public IScooterService Service;
        public IRentalCompany Rental;
        public Accounting Account;
        public List<Payment> Payments;

        public ScooterRentalExceptionsTests()
        {
            Payments = new List<Payment>();
            Account = new Accounting(Payments);
            Service = new ScooterService("City scooters");
            Rental = new RentalCompany("City scooters", Service, Account);
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
        public void StartRent_IsRented_ScooterIsNotAvailableException()
        {
            //Arrange
            Service.AddScooter("scooters", 1);
            Rental.StartRent("5");
            //Assert
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Rental.StartRent("5"));
        }

        [TestMethod]
        public void GetScooterById_NonExistingId_ScooterIsNotAvailableException()
        {
            Service.AddScooter("scooters", 1);
            Assert.ThrowsException<ScooterIsNotAvailableException>(() => Service.GetScooterById("22"));
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

        [TestMethod]
        public void EndRenting_IncorrectEndTime_Exception()
        {
            //Arrange
            Service.AddScooter("5", 0.04m);
            DateTime timeStart = DateTime.Parse("2020-09-09 09:00:00");
            Account.StartRenting("5", timeStart);
            DateTime timeEnd = DateTime.Parse("2020-09-08 17:00:00");
            //Assert
            Assert.ThrowsException<IncorrectEndTimeException>(() => Account.EndRenting("5", timeEnd));
        }
    }
}
