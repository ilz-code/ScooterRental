using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Objects;

namespace ScooterRental.Processing
{
    public class Accounting : IAccounting
    {
        public List<Payment> Payments;

        public Accounting(List<Payment> payments)
        {
            Payments = payments;
        }

        public void StartRenting(string id, DateTime time, decimal pricePerMinute)
        {
            Payment payment = new Payment(id, time, pricePerMinute);
            Console.WriteLine($"Price per minute: {pricePerMinute}");
            Payments.Add(payment);
        }

        public void EndRenting(string id, DateTime time)
        {
            Payment payment = null;
            foreach (Payment p in Payments)
                if (p.Id == id)
                    payment = p;
            payment.EndTime = time;
            if (time < payment.StartTime)
                throw new IncorrectEndTimeException();
            decimal pay = CalculatePay(payment.StartTime, payment.EndTime, payment.PricePerMinute);
            payment.SumPay = pay;
        }

        public decimal GetPay(string id)
        {
            Payment payment = null;
            foreach (Payment p in Payments)
                if (p.Id == id)
                    payment = p;
            if (payment == null)
                throw new PaymentNotFoundException();

            return payment.SumPay;
        }

        public decimal CalculatingIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal income = 0;

            if (includeNotCompletedRentals == false)
            {
                if (year == null)
                {
                    income = Payments.Sum(payment => payment.SumPay);
                }

                else
                {
                    foreach (Payment payment in Payments)
                    {
                        if (payment.EndTime.Year == year)
                            income += payment.SumPay;
                    }
                }
            }

            else
            {
                if (year == null)
                {
                    income = (decimal)Payments.Sum(payment => payment.SumPay);
                    foreach (Payment payment in Payments)
                    {
                        if (payment.EndTime.Year == 1)
                        {
                            DateTime endTime = DateTime.Parse($"{payment.StartTime.Year + 1}-01-01 00:00:00");
                            income += CalculatePay(payment.StartTime, endTime, payment.PricePerMinute);
                        }
                    }
                }
                else
                {
                    foreach (Payment payment in Payments)
                    {
                        if (payment.EndTime.Year == year)
                        {
                            income += payment.SumPay;
                        }
                        else
                        {
                            if (payment.StartTime.Year == year)
                            {
                                payment.EndTime = DateTime.Parse($"{payment.StartTime.Year + 1}-01-01 00:00:00");
                                income += CalculatePay(payment.StartTime, payment.EndTime, payment.PricePerMinute);
                            }
                        }
                    }
                }
            }

            return income;
        }

        private decimal CalculatePay(DateTime startTime, DateTime endTime, decimal pricePerMinute)
        {
            decimal minutes = (decimal)(endTime - startTime).TotalMinutes;
            decimal pay = Math.Round(minutes * pricePerMinute, 2);
            if (pay > 20)
            {
                decimal firstDay = (decimal)(startTime.AddDays(1).Date - startTime)
                    .TotalMinutes * pricePerMinute;
                if (firstDay > 20)
                    firstDay = 20;
                decimal lastDay = (decimal)(endTime - endTime.Date)
                    .TotalMinutes * pricePerMinute;
                if (lastDay > 20)
                    lastDay = 20;
                decimal fullDays = (decimal)((endTime.Date - startTime.Date)
                    .TotalDays - 1) * 20;
                pay = Math.Round(firstDay + fullDays + lastDay, 2);
            }

            return pay;
        }
    }
}

