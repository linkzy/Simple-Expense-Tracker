using SimpleExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO
{
    public class ActivityDTO
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }

        public int CategoryId { get; set; }

        public ActivityDTO()
        {

        }

        public ActivityDTO(Activity activity)
        {
            this.Id = activity.Id;
            this.Value = activity.Value;
            this.Description = activity.Description;
            this.ActivityDate = activity.ActivityDate;
            this.CategoryId = activity.CategoryId;
        }
    }
}
