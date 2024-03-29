﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoriesDTO
{
    public class RequiredIfCategoryTypeIsExpense : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            AddCategoryDTO category = validationContext.ObjectInstance as AddCategoryDTO;
            if (category?.CategoryType == CategoryTypeDTO.Expense)
            {
                if (category.BudgetType == null)
                {
                    return new ValidationResult("A budget type is required for Expense categories", new List<string> { "BudgetType" });
                }
            }
            return ValidationResult.Success;
        }
    }

    public class RequiredIfBudgetTypeIsSet : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            AddCategoryDTO category = validationContext.ObjectInstance as AddCategoryDTO;
            if (category?.BudgetType != BudgetTypeDTO.NoBudget && Convert.ToDecimal(category.Budget) < 1)
            {
                return new ValidationResult("A budget must be chosen for Expense categories", new List<string> { "Budget" });
            }
            return ValidationResult.Success;
        }
    }

    public class CategoryTypeMustBeSet : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            AddCategoryDTO category = validationContext.ObjectInstance as AddCategoryDTO;
            if (category.CategoryType == CategoryTypeDTO.Income || category.CategoryType == CategoryTypeDTO.Expense)
                return ValidationResult.Success;
            else
                return new ValidationResult("A category type must be chosen", new List<string> { "CategoryType" });
        }
    }
}
