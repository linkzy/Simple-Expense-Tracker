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

        //Stats
        public int Month { get; set; }
        public int Year { get; set; }

        public decimal ActivitiesSum { get; set; }

        
        public CategoryDTO()
        {

        }

        public CategoryDTO(Category category, int? month, int? year)
        {
            Id = category.Id;
            Name = category.Name;
            Budget = Convert.ToDecimal(category.Budget);
            BudgetType = (BudgetTypeDTO)category.BudgetType;
            AccountId = category.AccountId;
            CategoryIcon = category.CategoryIcon;
            CategoryType = (CategoryTypeDTO)category.CategoryType;

            Month = (int)month;
            Year = (int)year;
            ActivitiesSum = category.GetActivitiesSum(Month, Year);
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
