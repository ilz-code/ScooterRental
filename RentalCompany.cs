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

       public void StartRent(Scooter scooter)
       {
           payments.Add(new Payment(scooter.Id, scooter.TimeStart, scooter.PricePerMinute));
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

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal income = 0;
            foreach (Payment payment in payments)
            {
                int? year1 = payment.EndTime.Year;
                if (year1==year)
                {
                    income += payment.SumPay;
                }
            }

            if (includeNotCompletedRentals == true)
            {
                foreach (Payment payment in payments)
                {
                    if (payment.EndTime.Year != year)
                    {
                        income += CalculatePay(payment.StartTime, DateTime.Now, payment.PricePerMinute);
                    }
                }
            }

            if (year == null)
            {
                foreach (Payment payment in payments)
                {
                    income += payment.SumPay;
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
