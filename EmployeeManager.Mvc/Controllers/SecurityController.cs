using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Mvc.Models;
using EmployeeManager.Mvc.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Mvc.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly RoleManager<AppIdentityRole> roleManager;
        private readonly SignInManager<AppIdentityUser> signinManager;

        public SecurityController(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager,
            SignInManager<AppIdentityUser> signinManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signinManager = signinManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register obj)
        {
            //JOS
            //pwd bv Super45Secret!
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync("Manager").Result)
                {
                    AppIdentityRole role = new AppIdentityRole();
                    role.Name = "Manager";
                    role.Description = "Can perform CRUD operations.";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }

                AppIdentityUser user = new AppIdentityUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                user.FullName = obj.FullName;
                user.BirthDate = obj.BirthDate;

                IdentityResult result = userManager.CreateAsync
                (user, obj.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                    return RedirectToAction("SignIn", "Security");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details: " + result.Errors.ToString());
                }
            }
            return View(obj);
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                var result = signinManager.PasswordSignInAsync
                (obj.UserName, obj.Password,
                    obj.RememberMe, false).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("List", "EmployeeManager");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                }
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult SignOut()
        {
            signinManager.SignOutAsync().Wait();
            return RedirectToAction("SignIn", "Security");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}