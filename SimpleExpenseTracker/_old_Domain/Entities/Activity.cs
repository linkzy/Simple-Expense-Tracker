using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
