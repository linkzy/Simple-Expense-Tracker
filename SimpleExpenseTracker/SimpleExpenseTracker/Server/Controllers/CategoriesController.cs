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

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _context.Categories
                                        .Where(x => x.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
                                        .ToListAsync();
            if (categories == null)
            {
                return NotFound();
            }
            return categories.Select(x => new CategoryDTO(x)).ToList();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
            {
                return NotFound();
            }
            return new CategoryDTO(category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO category)
        {
            var account = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
            if (account == null)
                return Problem("Account not found");

            _context.Categories.Add(new Category()
            {
                Id = category.Id,
                Name = category.Name,
                Budget = category.Budget,
                BudgetType = (BudgetType)category.BudgetType,
                CategoryIcon = category.CategoryIcon,
                CategoryType = (CategoryType)category.CategoryType,
                Activities = new List<Activity>(),
                AccountId = account.Id,
                Account = account
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
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

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
