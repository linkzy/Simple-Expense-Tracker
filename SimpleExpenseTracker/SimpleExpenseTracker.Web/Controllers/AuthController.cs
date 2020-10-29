using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Relationships;
using Infra.Data.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleExpenseTracker.Web.Controllers
{
    public class AuthController : Controller
    {
        private SETContext _context;

        public AuthController(SETContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection form)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == form["Email"].ToString());
            if(user != null)
            {
                byte[] password = Encoding.ASCII.GetBytes(form["Password"].ToString());
                var passowrdHash = GenerateSaltedHash(password, user.Salt);
                if(CompareByteArrays(user.PasswordHash, passowrdHash))
                {
                    user.UserAccounts = _context.UsersAccounts.Where(ua => ua.UserId == user.Id).ToList();
                    foreach(var ua in user.UserAccounts)
                    {
                        ua.Account = _context.Accounts.FirstOrDefault(a => a.AccountOwnerId == user.Id);
                        ua.Account.Categories = _context.Categories.Where(c => c.AccountId == ua.AccountId).ToList();
                    }

                    await SignInUser(user);

                    var activeAccount = user.UserAccounts.FirstOrDefault(ua => ua.AccountId == user.ActiveAccountId).Account;
                    if (activeAccount.Categories != null && activeAccount.Categories.Count() > 0)
                        return RedirectToAction("Index", "Expenses");
                    else
                        return RedirectToAction("SetUp", "Categories");
                }
            }
            ModelState.AddModelError(String.Empty, "Invalid e-mail or password.");
            return View();

        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form)
        {
            foreach(var f in form)
            {
                if (String.IsNullOrEmpty(f.Value))
                {
                    ModelState.AddModelError(String.Empty, "You need to fill in all the fields to register.");
                    return View();
                }
            }

            if(form["Password"].ToString() != form["RepeatPassword"].ToString())
            {
                ModelState.AddModelError(String.Empty, "Password does not match.");
                return View();
            }

            if (_context.Users.FirstOrDefault(u => u.Email == form["Email"].ToString()) == null)
            {
                byte[] salt = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
                byte[] password = Encoding.ASCII.GetBytes(form["Password"].ToString());

                User user = new User()
                {
                    Name = form["Name"].ToString() + form["LastName"].ToString(),
                    Email = form["Email"].ToString(),
                    PasswordHash = GenerateSaltedHash(password, salt),
                    Salt = salt
                };
                _context.Users.Add(user);

                Account userAccount = new Account()
                {
                    Categories = new List<Category>(),
                    AccountOwner = user
                };
                _context.Accounts.Add(userAccount);
                await _context.SaveChangesAsync();

                user.UserAccounts = new List<UsersAccounts>();
                user.UserAccounts.Add(new UsersAccounts()
                {
                    Account = userAccount,
                    AccountId = userAccount.Id,
                    User = user,
                    UserId = user.Id
                });
                user.ActiveAccountId = userAccount.Id;
                await _context.SaveChangesAsync();

                await SignInUser(user);
                return RedirectToAction("SetUp", "Categories");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Email already in use. Please choose another one.");
            }
            return View();
        }
        
        [Route("logout")]
        [HttpPost] //=> To avoid Chorme "pre-fething" this page.
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);            
        }

        private byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}