using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Shared.DTO;
using System.Linq;
using System.Security.Claims;

namespace SimpleExpenseTracker.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly SETContext _context;
        public ActivitiesController(SETContext context)
        {
            _context = context;
        }

        [HttpGet("{categoryId}/{month}/{year}")]
        public ActionResult<ActivityDTO> Get(int categoryId, int month, int year)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var category = user.UserAccount.Categories.FirstOrDefault(x => x.Id == categoryId);

            if (category == null)
                return NotFound();

            var activites = _context.Activities
                                            .Where(x =>
                                                        x.CategoryId == categoryId
                                                        && x.ActivityDate.Year == year
                                                        && x.ActivityDate.Month == month)
                                            .ToList();

            if (activites == null)
                return NotFound();

            return Ok(activites.Select(x => new ActivityDTO(x)).ToList());
        }

        //POST: api/Activity
        [HttpPost]
        public ActionResult<ActivityDTO> Post(ActivityDTO activity)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var category = user.UserAccount.Categories.FirstOrDefault(x => x.Id == activity.CategoryId);

            if (category == null)
                return NotFound();

            var newActivity = new Activity()
            {
                ActivityDate = activity.ActivityDate,
                CategoryId = activity.CategoryId,
                Description= activity.Description,
                Value= activity.Value,
                CreationDate = DateTime.UtcNow
            };

            _context.Activities.Add(newActivity);
            _context.SaveChangesAsync();

            return Ok(new ActivityDTO(newActivity));
        }

        //POST: api/Activity
        [HttpPut]
        public ActionResult<ActivityDTO> Put(int id, ActivityDTO activity)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var category = user.UserAccount.Categories.FirstOrDefault(x => x.Id == activity.CategoryId);
            if (category == null)
                return NotFound();

            var existingActivity = _context.Activities.FirstOrDefault(x => x.Id == id && x.CategoryId == category.Id);
            if (existingActivity == null)
                return NotFound();

            existingActivity.ChangeDate = DateTime.UtcNow;
            existingActivity.Value = activity.Value;
            existingActivity.Description = activity.Description;
            existingActivity.ActivityDate = activity.ActivityDate;

            _context.Activities.Update(existingActivity);
            _context.SaveChangesAsync();

            return Ok(new ActivityDTO(existingActivity));
        }

        //DELETE: api/Activity
        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            var user = _context
                            .Users
                            .Include(x => x.UserAccount.Categories)
                            .FirstOrDefault(x => x.UserId == new Guid(User.FindFirstValue("UserId")));

            var existingActivity = _context.Activities.FirstOrDefault(x => x.Id == id);
            if (existingActivity == null)
                return NotFound();

            var category = user.UserAccount.Categories.FirstOrDefault(x => x.Id == existingActivity.CategoryId);
            if (category == null || !user.UserAccount.Categories.Contains(category))
                return NotFound();
           

            existingActivity.ChangeDate = DateTime.UtcNow;
            existingActivity.IsDeleted= true;

            _context.Activities.Update(existingActivity);
            _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
