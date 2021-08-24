using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ScooterRental;

namespace ScooterRentalTests
{
    [TestClass]
    public class ScooterRentalTests
    {
        public IScooterService Service;
        public IRentalCompany Rental;
        public Accounting Account;
        public List<Payment> Payments;

        public ScooterRentalTests()
        {
            Payments = new List<Payment>();
            Account = new Accounting(Payments);
            Service = new ScooterService("City scooters");
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
            Service.AddScooter("2", 0.05m);
            Scooter scooter = Service.GetScooterById("2");
            scooter.IsRented = false;
            Rental.StartRent("2");
            DateTime timeStart = DateTime.Parse("2020-08-13 08:00:00");
            Account.StartRenting("2", timeStart);
            DateTime timeEnd = DateTime.Parse("2020-08-14 17:00:00");
            Account.EndRenting("2", timeEnd);
            //Act
            Rental.EndRent("2");
            bool result = Service.GetScooterById("2").IsRented;
            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StartRent_Count()
        {
            //Arrange
            Service.AddScooter("Scooters", 0.01m);
            Scooter scooter = Service.GetScooterById("1"); 
            DateTime time = DateTime.Parse("2021-08-12 11:00:00");
            //Act
            Account.StartRenting(scooter.Id, time);
            //Assert
            int result = Payments.Count;
            int expect = 1;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void EndRent_scooter_pay()
        {
            //Arrange
            Payments.AddRange(Account.PaymentsFromFile("Payments"));
            DateTime timeEnd = DateTime.Parse("2020-08-14 17:00:00");
            //Act
            decimal result = Account.EndRenting("1", timeEnd);
            //Assert
            decimal expect = 54.40m;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        [DataRow(2020, false, 54.40)]
        [DataRow(2020, true, 114.400)]
        [DataRow(2021, false, 74.400)]
        [DataRow(2021, true, 108.80)]
        [DataRow(null, false, 128.80)]
        [DataRow(null, true, 163.20)]
        
        public void CalculateIncome_ThreeRentings_Income(int? year, bool notCompleted, double expect)
        {
            //Arrange
            decimal expected = Convert.ToDecimal(expect);
            Payments.AddRange(Account.PaymentsFromFile("Payments"));
            Payment payment1 = Payments.Find(p => p.Id == "1");
            DateTime time2 = DateTime.Parse("2020-08-14 17:00:00");
            payment1.EndTime = time2;
            payment1.SumPay = Account.EndRenting(payment1.Id, time2);
            Payment payment2 = Payments.Find(p => p.Id == "2");
            DateTime time4 = DateTime.Parse("2021-01-01 08:00:00");
            payment2.EndTime = time4;
            payment2.SumPay = Account.EndRenting(payment2.Id, time4);
            Payment payment3 = Payments.Find(p => p.Id == "3");
            //Act
            decimal result = Rental.CalculateIncome( year, notCompleted);
            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
