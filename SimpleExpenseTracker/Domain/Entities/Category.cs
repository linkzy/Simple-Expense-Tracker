using System;
using System.Collections.Generic;
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
    }

    public enum CategoryType
    {
        Expense,
        Income
    }
}
