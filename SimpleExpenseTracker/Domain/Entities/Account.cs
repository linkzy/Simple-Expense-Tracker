using Domain.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public List<Category> Categories { get; set; }
        public User AccountOwner { get; set; }
        public int AccountOwnerId { get; set; }

        private int _day = DateTime.Now.Day;
        public decimal GetTotalIdealDailySpending(int month, int year)
        {
            return this.Categories.Where(x => x.CategoryType == CategoryType.Expense).Sum(c => c.GetIdealDailySpending(month, year));
        }

        public decimal GetTotalDailySpending(int month, int year)
        {
            return this.Categories.Where(x => x.CategoryType == CategoryType.Expense).Sum(c => c.GetDailySpending(month, year));
        }

        public bool IsSpendingMoreThanBudget(int month, int year)
        {
            return GetTotalDailySpending(month, year) > GetTotalIdealDailySpending(month, year);
        }       

        public decimal GetTotalBudget(int month, int year)
        {
            return this.Categories.Where(x => x.CategoryType == CategoryType.Expense).Sum(c => c.Budget);
        }

        public decimal GetTotalSpending(int month, int year)
        {
            return this.Categories.Where(x => x.CategoryType == CategoryType.Expense).Sum(c => c.GetActivitiesSum(month, year));
        }

        public DateTime WaitUntilDateToSpendAgain(int month, int year)
        {
            int days = DateTime.DaysInMonth(year, month);
            int day = _day;// DateTime.Now.Day;
            
            decimal totalSpendingSimulation = this.Categories.Sum(x => x.SimulateDailySpending(day, month, year));
            decimal idealSepnding = GetTotalIdealDailySpending(month, year);
            
            while (totalSpendingSimulation > idealSepnding)
            {
                if(day > days)
                    return new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1);

                day++;
                totalSpendingSimulation = this.Categories.Sum(x => x.SimulateDailySpending(day, month, year));
            }
            return new DateTime(year, month, day);
        }
    }
}
