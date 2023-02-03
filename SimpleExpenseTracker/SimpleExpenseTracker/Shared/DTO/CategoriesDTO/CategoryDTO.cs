using SimpleExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoriesDTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [CategoryTypeMustBeSet]
        public CategoryTypeDTO CategoryType { get; set; }
        
        [RequiredIfCategoryTypeIsExpense]
        public BudgetTypeDTO BudgetType { get; set; }
        
        [RequiredIfBudgetTypeIsSet]
        public decimal Budget { get; set; }

        [Required]
        public string CategoryIcon { get; set; } = string.Empty;

        public int AccountId { get; set; }
        
        public CategoryDTO()
        {

        }

        public CategoryDTO(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Budget = Convert.ToDecimal(category.Budget);
            BudgetType = (BudgetTypeDTO)category.BudgetType;
            AccountId = category.AccountId;
            CategoryIcon = category.CategoryIcon;
            CategoryType = (CategoryTypeDTO)category.CategoryType;
        }

    }

    public enum CategoryTypeDTO
    {
        Expense = 1,
        Income = 2
    }

    public enum BudgetTypeDTO
    {
        NoBudget = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4
    }
}
