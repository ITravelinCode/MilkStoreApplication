using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Auth
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
