﻿using SimpleExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Domain
{
    public class Account : Entity
    {
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

        public decimal GetTotalBudget()
        {
            return this.Categories.Where(x => x.CategoryType == CategoryType.Expense).Sum(c => Convert.ToDecimal(c.Budget));
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
                if (day > days)
                    return new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1);

                day++;
                totalSpendingSimulation = this.Categories.Sum(x => x.SimulateDailySpending(day, month, year));
            }
            if (day > days)
                return new DateTime(year, month, days);
            return new DateTime(year, month, day);
        }
    }
}
