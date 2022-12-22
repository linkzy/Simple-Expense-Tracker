using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Shared.DTO.CategoryDTO;

namespace SimpleExpenseTracker.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SETContext _context;

        public CategoriesController(SETContext context)
        {
            _context = context;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (_context.Categories == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            if (category.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
                return Unauthorized();

            return new CategoryDTO(category);
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _context.Categories
                                        .Where(x => x.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
                                        .ToListAsync();
            if (categories == null)
                return NotFound();

            return categories.Select(x => new CategoryDTO(x)).ToList();
        }        

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreateCategoryDTO category)
        {
            var account = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
            if (account == null)
                return Problem("Account not found");

            var newCategory = new Category()
            {
                Name = category.Name,
                Budget = category.Budget,
                BudgetType = (BudgetType)category.BudgetType,
                CategoryIcon = category.CategoryIcon,
                CategoryType = (CategoryType)category.CategoryType,
                Activities = new List<Activity>(),
                AccountId = account.Id,
                Account = account
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
        }

        // PUT: api/Categories/5
        [HttpPut]
        public async Task<IActionResult> PutCategory(UpdateCategoryDTO categoryToUpdate)
        {
            if (categoryToUpdate == null || categoryToUpdate.Id < 1)
                return BadRequest();

            var category = await _context.Categories.FindAsync(categoryToUpdate.Id);
            if (category == null )
                return NotFound();

            if (category.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
                return Unauthorized();

            category.Name = categoryToUpdate.Name;
            category.Budget = categoryToUpdate.Budget;
            category.BudgetType = (BudgetType)categoryToUpdate.BudgetType;
            category.CategoryIcon = categoryToUpdate.CategoryIcon;
            category.CategoryType = (CategoryType)categoryToUpdate.CategoryType;
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }        

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            
            var userAccount = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
            if (userAccount == null)
                return Problem("Account not found");

            if (category.AccountId != userAccount.Id)
                return Unauthorized();

            category.IsDeleted = true;
            category.ChangeDate = DateTime.Now;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
