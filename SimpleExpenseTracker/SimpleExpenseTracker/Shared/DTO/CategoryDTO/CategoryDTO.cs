using SimpleExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoryDTO
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

        public CategoryDTO(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Budget = category.Budget;
            this.BudgetType = (BudgetTypeDTO)category.BudgetType;
            this.AccountId = category.AccountId;
            this.CategoryIcon = category.CategoryIcon;
            this.CategoryType = (CategoryTypeDTO)category.CategoryType;
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
