using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleExpenseTracker.Domain;
using SimpleExpenseTracker.Shared.DTO;
using System.Linq;
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
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Activity>> Get(int month, int year, int categoryId)
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
    }
}
