using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Domain.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public DateTime ChangeDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
