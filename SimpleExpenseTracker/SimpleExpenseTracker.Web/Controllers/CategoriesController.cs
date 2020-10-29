using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Infra.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleExpenseTracker.Web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private SETContext _context;

        public CategoriesController(SETContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SetUp()
        {
            var uEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == uEmail);
            var categories = _context.Categories.Where(x => x.AccountId == user.ActiveAccountId);
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategoryToUserAccount(IFormCollection form)
        {
            string categoryName = form["catName"];
            int budget = Convert.ToInt32(form["catBudget"]);
            int categoryType = Convert.ToInt32(form["catType"]);
            string categoryIco = form["catIco"];

            var uEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == uEmail);
            Category c = new Category()
            {
                Name = categoryName,
                Budget = budget,
                CategoryIcon = categoryIco,
                CategoryType = (CategoryType)categoryType,
                AccountId = user.ActiveAccountId,
            };
            _context.Categories.Add(c);
            _context.SaveChanges();

            return RedirectToAction("SetUp");
        }

        [HttpPost]
        public IActionResult RemoveCategoryFromUserAccount(int categoryId)
        {
            var c = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
            _context.Categories.Remove(c);
            _context.SaveChanges();

            return Json(c.Id);
        }
    }
}