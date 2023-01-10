//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SimpleExpenseTracker.Domain;
//using SimpleExpenseTracker.Shared.DTO.ActivityDTO;
//using SimpleExpenseTracker.Shared.DTO.CategoryDTO;
//using System.Security.Claims;

//namespace SimpleExpenseTracker.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ActivityController : ControllerBase
//    {
//        private readonly SETContext _context;

//        public ActivityController(SETContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Activity
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<GetActivityDTO>>> GetActivities(int month, int year, int categoryId)
//        {
//            var category = await _context.Categories
//                                        .FirstOrDefaultAsync(x =>
//                                                    x.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId"))
//                                                    && x.Id == categoryId);                                       

      
//            if (category == null)
//                return NotFound();

//            var activites = _context.Activities
//                                            .Where(x =>
//                                                        x.CategoryId == categoryId
//                                                        && x.ActivityDate.Year == year
//                                                        && x.ActivityDate.Month == month)
//                                            .ToList();

//            if (activites == null)
//                return NotFound();

//            return activites.Select(x => new GetActivityDTO(x)).ToList();
//        }

//        // POST: api/Activity
//        [HttpPost]
//        public async Task<ActionResult<Category>> PostCategory(CreateActivityDTO activity)
//        {
//            var account = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
//            if (account == null)
//                return Problem("Account not found");

//            var newCategory = new Category()
//            {
//                Name = category.Name,
//                Budget = category.Budget,
//                BudgetType = (BudgetType)category.BudgetType,
//                CategoryIcon = category.CategoryIcon,
//                CategoryType = (CategoryType)category.CategoryType,
//                Activities = new List<Activity>(),
//                AccountId = account.Id,
//                Account = account,
//                CreationDate = DateTime.Now,
//            };

//            _context.Categories.Add(newCategory);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
//        }

//        // PUT: api/Activity/5
//        [HttpPut]
//        public async Task<IActionResult> PutCategory(UpdateCategoryDTO categoryToUpdate)
//        {
//            if (categoryToUpdate == null || categoryToUpdate.Id < 1)
//                return BadRequest();

//            var category = await _context.Categories.FindAsync(categoryToUpdate.Id);
//            if (category == null)
//                return NotFound();

//            if (category.AccountId == Convert.ToInt32(User.FindFirstValue("UserAccountId")))
//                return Unauthorized();

//            category.Name = categoryToUpdate.Name;
//            category.Budget = categoryToUpdate.Budget;
//            category.BudgetType = (BudgetType)categoryToUpdate.BudgetType;
//            category.CategoryIcon = categoryToUpdate.CategoryIcon;
//            category.CategoryType = (CategoryType)categoryToUpdate.CategoryType;
//            category.ChangeDate = DateTime.Now;

//            _context.Entry(category).State = EntityState.Modified;
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                throw;
//            }

//            return NoContent();
//        }

//        // DELETE: api/Activity/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCategory(int id)
//        {
//            var category = await _context.Categories.FindAsync(id);
//            if (category == null)
//                return NotFound();

//            var userAccount = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
//            if (userAccount == null)
//                return Problem("Account not found");

//            if (category.AccountId != userAccount.Id)
//                return Unauthorized();

//            category.IsDeleted = true;
//            category.ChangeDate = DateTime.Now;

//            _context.Categories.Update(category);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
