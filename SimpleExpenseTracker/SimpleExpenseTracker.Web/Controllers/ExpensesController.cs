using Domain.Entities;
using Infra.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Web.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private SETContext _context;

        public ExpensesController(SETContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var uEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == uEmail);
            var categories = _context.Categories.Where(x => x.AccountId == user.ActiveAccountId).Include("Activities");

            ViewBag.Account = _context.Accounts.Where(x => x.Id == user.ActiveAccountId).Include("Categories").FirstOrDefault();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddActivity(IFormCollection form)
        {
            Activity a = new Activity()
            {
                Value = Convert.ToDecimal(form["value"]),
                CategoryId = Convert.ToInt32(form["catId"]),
                Date = DateTime.Now,
            };
            _context.Activities.Add(a);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}
