using System.ComponentModel.DataAnnotations;

namespace SimpleExpenseTracker.Shared.DTO.CategoriesDTO
{
    public class AddCategoryDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [CategoryTypeMustBeSet]
        public CategoryTypeDTO CategoryType { get; set; }
  
        [RequiredIfCategoryTypeIsExpense]
        public BudgetTypeDTO? BudgetType { get; set; }

        [RequiredIfBudgetTypeIsSet]
        public decimal? Budget { get; set; }

        [Required]
        public string CategoryIcon { get; set; } = string.Empty;
        
    }
}
