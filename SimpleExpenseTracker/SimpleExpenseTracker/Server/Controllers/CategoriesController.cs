using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Shared.DTO;

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
        public ActionResult<CategoryDTO> GetCategory(int id)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));                            

            var category = user?.UserAccount?.Categories?.FirstOrDefault(x => x.Id == id);                
            if (category == null)
                return NotFound();

            return Ok(new CategoryDTO(category));
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories()
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            if (user?.UserAccount?.Categories == null)
                return NotFound();

            return Ok(user.UserAccount.Categories.Select(x => new CategoryDTO(x)).ToList());
        }        

        // POST: api/Categories/
        [HttpPost]
        public ActionResult<CategoryDTO> PostCategory(CategoryDTO category)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            if (user?.UserAccount == null)
                return Problem("Account not found");

            var newCategory = new Category()
            {
                Name = category.Name,
                Budget = category.Budget,
                BudgetType = (BudgetType)category.BudgetType,
                CategoryIcon = category.CategoryIcon,
                CategoryType = (CategoryType)category.CategoryType,
                Activities = new List<Activity>(),
                AccountId = user.UserAccount.Id,
                Account = user.UserAccount,
                CreationDate = DateTime.Now,
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
        }

        // PUT: api/Categories/5
        [HttpPut]
        public IActionResult PutCategory(int id, CategoryDTO categoryToUpdate)
        {

            if (categoryToUpdate == null || categoryToUpdate.Id < 1)
                return BadRequest();

            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var category = user?.UserAccount.Categories.FirstOrDefault(x=> x.Id == id);
            if (category == null )
                return BadRequest();

            category.Name = categoryToUpdate.Name;
            category.Budget = categoryToUpdate.Budget;
            category.BudgetType = (BudgetType)categoryToUpdate.BudgetType;
            category.CategoryIcon = categoryToUpdate.CategoryIcon;
            category.CategoryType = (CategoryType)categoryToUpdate.CategoryType;
            category.ChangeDate = DateTime.Now;

            _context.Entry(category).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }        

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var category = user?.UserAccount.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound();  

            category.IsDeleted = true;
            category.ChangeDate = DateTime.Now;

            _context.Categories.Update(category);
            _context.SaveChanges();

            return NoContent();
        }       
    }
}
