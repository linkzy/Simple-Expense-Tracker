using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Shared.DTO.ActivityDTO;
using System.Security.Claims;

namespace SimpleExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly SETContext _context;

        public ActivitiesController(SETContext context)
        {
            _context=context;
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDTO>> Add(ActivityDTO activityDTO)
        {
            var account = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
            if (account == null)
                return Problem("Account not found");

            var categories = _context.Categories.Where(x => x.AccountId == account.Id).ToList();
            if (!categories.Any(x => x.Id == activityDTO.CategoryId))
                return Problem("Category not found");

            var activity = new Activity()
            {
                CategoryId= activityDTO.CategoryId,
                Value= activityDTO.Value,
                Description= activityDTO.Description,
                Date = DateTime.UtcNow
            };
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            activityDTO.Id = activity.Id;
            return Ok(activityDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDTO>>> Get(int categoryId, int month, int year)
        {
            var account = await _context.Accounts.FindAsync(Convert.ToInt32(User.FindFirstValue("UserAccountId")));
            if (account == null)
                return Problem("Account not found");

            var categories = _context.Categories.Where(x => x.AccountId == account.Id).ToList();
            if (!categories.Any(x => x.Id == categoryId))
                return Problem("Category not found");

            var activities = _context.Activities.Where(
                x => x.CategoryId == categoryId &&
                x.Date.Month == month &&
                x.Date.Year == year
            ).ToList();

            if (!activities.Any())
                return NotFound();

            return Ok(activities);
        }

    }
}
