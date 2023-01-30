using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoriesDTO
{
    public class AddCategoryDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Budget { get; set; }
        [Required]
        public BudgetTypeDTO BudgetType { get; set; }
        [Required]
        public string CategoryIcon { get; set; } = string.Empty;
        [Required]
        public CategoryTypeDTO CategoryType { get; set; }
    }
}
