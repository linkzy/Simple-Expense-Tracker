using SimpleExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Domain
{
    public class Activity : Entity
    {
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
