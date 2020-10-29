using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public List<Activity> Activities { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public string CategoryIcon { get; set; }
        public CategoryType CategoryType { get; set; }

        public decimal GetActivitiesSum(int month, int year)
        {
            if (this.Activities == null || this.Activities.Count() < 1)
                return 0;

            return this.Activities.Where(x => x.Date.Month == month && x.Date.Year == year).Sum(x => x.Value);
        }

        public int GetBudgetPercentLeft(int month, int year)
        {
            var spent = GetActivitiesSum(month, year);
            if (spent == 0)
                return 100;
            else
                return 100-Convert.ToInt32(spent * 100  / this.Budget);
        }

        public decimal GetIdealDailySpending(int month, int year)
        {
            int days = DateTime.DaysInMonth(year, month);
            return this.Budget / days;
        }

        public decimal GetDailySpending(int month, int year)
        {
            return GetActivitiesSum(month, year) / DateTime.Now.Day;
        }

        public bool IsSpendingMoreThanBudget(int month, int year)
        {
            return GetIdealDailySpending(month, year) < GetDailySpending(month, year);   
        }

        public decimal GetProportionalDailySpendingLeft(int month, int year)
        {
            int daysLeftInTheMonth = DateTime.DaysInMonth(year, month) - DateTime.Now.Day;
            int budgetLeft = Convert.ToInt32(this.Budget - GetActivitiesSum(month, year));
            return budgetLeft / daysLeftInTheMonth;
        }
    }

    public enum CategoryType
    {
        Expense = 1,
        Income = 2
    }
}
