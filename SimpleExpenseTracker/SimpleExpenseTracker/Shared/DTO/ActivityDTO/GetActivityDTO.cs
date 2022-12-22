using SimpleExpenseTracker.Domain;

namespace SimpleExpenseTracker.Shared.DTO.ActivityDTO
{
    public class GetActivityDTO
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }

        public int CategoryId { get; set; }

        public GetActivityDTO(Activity activity)
        {
            this.Id = activity.Id;
            this.Value = activity.Value;
            this.Description = activity.Description;
            this.ActivityDate = activity.ActivityDate;
            this.CategoryId = activity.CategoryId;
        }
    }
}
