using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExpenseTracker.Shared.DTO
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = String.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = String.Empty;
    }
}
