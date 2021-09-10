using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModels
{
    public class UserViewModel: IdentityUser
    {
        public string FullName { get; set; }
        public string NIN { get; set; }//National Identification Number
        public string Password { get; set; }
    }

    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
