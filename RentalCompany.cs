using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        public string Name { get; }
        public List<Payment> payments = new List<Payment>();

        public RentalCompany(string name)
        {
            Name = name;
        }

        public List<Payment> StartRent(Scooter scooter)
        {
            payments.Add(new Payment(scooter.Id, scooter.TimeStart, scooter.PricePerMinute));
            return payments;
        }

        public decimal EndRent(Scooter scooter)
        {
            decimal pay = CalculatePay(scooter.TimeStart, scooter.TimeEnd, scooter.PricePerMinute);
            foreach (Payment payment in payments)
            {
                if (payment.Id == scooter.Id)
                {
                    payment.EndTime = scooter.TimeEnd;
                    payment.SumPay = pay;
                }
            }

            return pay;
        }

        public decimal CalculateIncome(List<Payment> payments, int? year, bool includeNotCompletedRentals)
        {
            decimal income = 0;

            if (includeNotCompletedRentals == false)
            {
                if (year == null)
                {
                    income = (decimal)payments.Sum(payment => payment.SumPay);
                }

                else
                {
                    foreach (Payment payment in payments)
                    {
                        if (payment.EndTime.Year == year)
                            income += payment.SumPay;
                    }
                }
            }

            if (includeNotCompletedRentals == true)
            {
                if (year == null)
                {
                    income = (decimal)payments.Sum(payment => payment.SumPay);
                    foreach (Payment payment in payments)
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
                    foreach (Payment payment in payments)
                    {
                        if (payment.StartTime.Year == year)
                        {
                            if (payment.EndTime.Year == year)
                            {
                                income += payment.SumPay;
                            }
                            else
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

        public decimal CalculatePay(DateTime startTime, DateTime endTime, decimal pricePerMinute)
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
