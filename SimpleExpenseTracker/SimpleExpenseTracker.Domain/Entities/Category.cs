using SimpleExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Domain
{
    public class Category : Entity
    {
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }
        public BudgetType BudgetType { get; set; }

        public List<Activity>? Activities { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public string CategoryIcon { get; set; } = string.Empty;
        public CategoryType CategoryType { get; set; }

        private int _day = DateTime.Now.Day;

        public decimal GetActivitiesSum(int month, int year)
        {
            if (this.Activities == null || this.Activities.Count() < 1)
                return 0;

            return this.Activities.Where(x => x.ActivityDate.Month == month && x.ActivityDate.Year == year).Sum(x => x.Value);
        }

        public int GetBudgetPercentLeft(int month, int year)
        {
            var spent = GetActivitiesSum(month, year);
            if (spent == 0)
                return 100;
            else
                return 100 - Convert.ToInt32(spent * 100 / this.Budget);
        }

        public decimal GetIdealDailySpending(int month, int year)
        {
            int days = DateTime.DaysInMonth(year, month);
            return this.Budget / days;
        }

        public decimal GetDailySpending(int month, int year)
        {
            return GetActivitiesSum(month, year) / _day; // DateTime.Now.Day;
        }

        public decimal SimulateDailySpending(int day, int month, int year)
        {
            return GetActivitiesSum(month, year) / day;
        }

        public bool IsSpendingMoreThanBudget(int month, int year)
        {
            return GetDailySpending(month, year) > GetIdealDailySpending(month, year);
        }

        public decimal GetProportionalDailySpendingLeft(int month, int year)
        {
            decimal idealSpending = GetIdealDailySpending(month, year);
            decimal acumulatedBudget = idealSpending * _day;
            decimal left = acumulatedBudget - GetActivitiesSum(month, year);
            return left < 0 ? 0 : left;
        }

        public DateTime WaitUntilDateToSpendAgain(int month, int year)
        {
            int days = DateTime.DaysInMonth(year, month);
            int day = _day;

            decimal totalSpendingSimulation = SimulateDailySpending(day, month, year);
            decimal idealSepnding = GetIdealDailySpending(month, year);

            while (totalSpendingSimulation > idealSepnding)
            {
                if (day > days)
                    return new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1);

                day++;
                totalSpendingSimulation = SimulateDailySpending(day, month, year);
            }
            return new DateTime(year, month, day);
        }
    }

    public enum CategoryType
    {
        Expense = 1,
        Income = 2
    }

    public enum BudgetType
    {
        NoBudget = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
}
