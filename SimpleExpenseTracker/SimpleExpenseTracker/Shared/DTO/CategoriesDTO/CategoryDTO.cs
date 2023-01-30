using SimpleExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoriesDTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }
        public BudgetTypeDTO BudgetType { get; set; }

        public int AccountId { get; set; }

        public string CategoryIcon { get; set; } = string.Empty;
        public CategoryTypeDTO CategoryType { get; set; }

        public CategoryDTO()
        {

        }

        public CategoryDTO(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Budget = category.Budget;
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
        NoBudget = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
}
