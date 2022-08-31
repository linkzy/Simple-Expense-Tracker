using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Domain
{
    public class Activity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
