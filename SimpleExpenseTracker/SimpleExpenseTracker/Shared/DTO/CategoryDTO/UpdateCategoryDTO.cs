using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO.CategoryDTO
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }
        public BudgetTypeDTO BudgetType { get; set; }

        public string CategoryIcon { get; set; } = string.Empty;
        public CategoryTypeDTO CategoryType { get; set; }
    }
}
