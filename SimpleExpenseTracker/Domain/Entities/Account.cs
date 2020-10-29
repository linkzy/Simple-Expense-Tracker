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

    }
}
