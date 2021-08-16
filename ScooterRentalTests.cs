using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ScooterRental;

namespace ScooterRentalTests
{
    [TestClass]
    public class ScooterRentalTests
    {
        public static ScooterService Service = new ScooterService("City scooters");
        public static RentalCompany Rental = new RentalCompany("City scooters");

        [TestMethod]
        public void AddScooters_Count()
        {
            Service.AddScooter("13", (decimal)0.08);
            Service.AddScooterFromFile("Scooters");
            Service.RemoveScooter("8");
            int result = Service.GetScooters().Count;
            int expect = 9;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void GetScooterById_scooter()
        {
            Service.AddScooterFromFile("Scooters");
            decimal result = Service.GetScooterById("7").PricePerMinute;
            decimal expect = (decimal)0.06;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void RentOutScooter_IsRented()
        {
            Service.AddScooterFromFile("Scooters");
            Service.RentOutScooter("6");
            bool result = Service.GetScooterById("6").IsRented;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReceiveBack_IsRented()
        {
            Service.AddScooterFromFile("Scooters");
            Service.RentOutScooter("6");
            Service.ReceiveBack("6");
            bool result = Service.GetScooterById("6").IsRented;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EndRent_scooter_pay()
        {
            Scooter scooter = new Scooter("15", (decimal)0.03);
            scooter.TimeStart = DateTime.Parse("2021-08-12 16:00:00");
            scooter.TimeEnd = DateTime.Parse("2021-08-14 17:00:00");
            decimal result = Rental.EndRent(scooter);
            decimal expect = (decimal)54.40;
            Assert.AreEqual(expect, result);
        }

        [DataTestMethod]
        [DataRow(2020, false, 54.40)]
        [DataRow(2020, true, 128.80)]
        public void CalculateIncome_scooter_Income(int? year, bool incomplete, double income)
        {
            List<Payment> payments = new List<Payment>();
            DateTime time1 = DateTime.Parse("2020-08-12 16:00:00");
            DateTime time2 = DateTime.Parse("2020-08-14 17:00:00");
            Payment payment1 = new Payment("5", time1, (decimal)0.03);
            payment1.EndTime = time2;
            payment1.SumPay = Rental.CalculatePay(time1, time2, (decimal)0.03);
            payments.Add(payment1);
            DateTime time3 = DateTime.Parse("2020-12-29 16:00:00");
            DateTime time4 = DateTime.Parse("2021-01-01 17:00:00");
            Payment payment2 = new Payment("7", time3, (decimal)0.03);
            payment2.EndTime = time4;
            payment2.SumPay = Rental.CalculatePay(time3, time4, (decimal)0.03);
            payments.Add(payment2);
            double result =(double)Rental.CalculateIncome(year, incomplete);
            Assert.AreEqual(income, result);
        }
    }
}
