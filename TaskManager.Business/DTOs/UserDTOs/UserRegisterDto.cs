using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entities.Helper;

namespace TaskManager.Entities.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Email is Required"), EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required"), MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }
    }
}