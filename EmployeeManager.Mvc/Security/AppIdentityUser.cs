using Microsoft.AspNetCore.Identity;
using System;

namespace EmployeeManager.Mvc.Security
{
    public class AppIdentityUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
