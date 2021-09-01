using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Objects;
using ScooterRental.Processing;

namespace ScooterRentalTests
{
    [TestClass]
    public class AccountingTests
    {
        public IScooterService Service;
        public IRentalCompany Rental;
        public Accounting Account;
        public List<Payment> Payments;
        public ReadingAndSavingScootersAndPayments ReadAndSaveFiles = new ReadingAndSavingScootersAndPayments();
        public List<Scooter> Scooters;

        public AccountingTests()
        {
            Payments = ReadAndSaveFiles.ReadPaymentsFromFile();
            Account = new Accounting(Payments);
            Scooters = ReadAndSaveFiles.ReadScootersFromFile();
            Service = new ScooterService("City scooters", Scooters);
            Rental = new RentalCompany("City scooters", Service, Account);
        }
        [TestMethod]
        public void StartRent_PaymentsCount()
        {
            //Arrange
            Payments.Clear();
            string id = "1";
            decimal pricePerMinute = 0.03m;
            DateTime time = DateTime.Parse("2021-08-12 11:00:00");
            //Act
            Account.StartRenting(id, time, pricePerMinute);
            //Assert
            int result = Payments.Count;
            int expect = 1;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void EndRent_scooter_pay()
        {
            //Arrange
            ReadAndSaveFiles.ReadPaymentsFromFile();
            DateTime timeStart = DateTime.Parse("2020-08-12 16:00:00");
            Account.StartRenting("1", timeStart, 0.03m);
            DateTime timeEnd = DateTime.Parse("2020-08-14 17:00:00");
            //Act
            Account.EndRenting("1", timeEnd);
            decimal result = Account.GetPay("1");
            //Assert
            decimal expect = 54.40m;
            Assert.AreEqual(expect, result);
        }
        [TestMethod]
        [DataRow(2020, false, 74.40)]
        [DataRow(2020, true, 128.80)]
        [DataRow(2021, false, 68.80)]
        [DataRow(2021, true, 103.20)]
        [DataRow(null, false, 143.20)]
        [DataRow(null, true, 177.60)]

        public void CalculateIncome_ThreeRentings_Income(int? year, bool notCompleted, double expect)
        {
            //Arrange
            decimal expected = Convert.ToDecimal(expect);
            ReadAndSaveFiles.ReadPaymentsFromFile();
            Payments.Clear();
            DateTime time1 = DateTime.Parse("2020-08-11 16:00:00");
            Account.StartRenting("11", time1, 0.03m);
            DateTime time2 = DateTime.Parse("2020-08-14 17:00:00");
            Account.EndRenting("11", time2);
            DateTime time3 = DateTime.Parse("2020-12-29 16:00:00");
            Account.StartRenting("12", time3, 0.03m);
            DateTime time4 = DateTime.Parse("2021-01-01 08:00:00");
            Account.EndRenting("12", time4);
            DateTime time5 = DateTime.Parse("2021-12-30 16:00:00");
            Account.StartRenting("13", time5, 0.03m);
            //Act
            decimal result = Rental.CalculateIncome(year, notCompleted);
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EndRenting_IncorrectEndTime_Exception()
        {
            //Arrange
            Scooters.Clear();
            Service.AddScooter("5", 0.04m);
            DateTime timeStart = DateTime.Parse("2020-09-09 09:00:00");
            Account.StartRenting("5", timeStart, 0.04m);
            DateTime timeEnd = DateTime.Parse("2020-09-08 17:00:00");
            //Assert
            Assert.ThrowsException<IncorrectEndTimeException>(() => Account.EndRenting("5", timeEnd));
        }

        [TestMethod]
        public void GetPay_NonExistingPayment_PaymentNotFoundException()
        {
            //Arrange
            ReadAndSaveFiles.ReadPaymentsFromFile();
            //Assert
            Assert.ThrowsException<PaymentNotFoundException>(() => Account.GetPay("33"));
        }
    }
}
