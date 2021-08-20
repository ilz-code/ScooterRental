using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScooterRental
{
    public class Accounting
    {

        public List<Payment> Payments = new List<Payment>();
        public IScooterService Service = new ScooterService("City scooters");
        public List<Scooter> scooters;

        public Accounting(List<Payment> payments)
        {
            Payments = payments;
        }

        public void StartRenting(string id, DateTime time)
        {
            Scooter scooter = Service.GetScooterById(id);
            decimal pricePerMinute = scooter.PricePerMinute;
            Payment payment = new Payment(id, time, pricePerMinute);
            Payments.Add(payment);
        }

        public decimal EndRenting(string id, DateTime time) //, List<Payment> Payments
        {
            Payment payment = Payments.Find(p => p.Id == id);
            payment.EndTime = time;
            decimal pay = CalculatePay(payment.StartTime, payment.EndTime, payment.PricePerMinute);
            payment.SumPay = pay;
            return pay;
        }

        public decimal GetPay(string id)
        {
            Payment payment = Payments.Find(p => p.Id == id);
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

            else //if (includeNotCompletedRentals == true)
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

        public List<Payment> PaymentsFromFile(string fileName)
        {
            List<Payment> payments = new List<Payment>();
            var text = File.ReadAllText($"..\\..\\{fileName}.txt");
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] words = lines[i].Split(' ');
                DateTime time = DateTime.Parse(words[1] + " " + words[2]);
                decimal price = Convert.ToDecimal(words[3]);
                payments.Add(new Payment(words[0], time, price));
            }

            return payments;
        }
    }
}

