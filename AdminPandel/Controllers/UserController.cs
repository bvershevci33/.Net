using AdminPandel.Models;
using AdminPandel.Utilities;
using AdminPandel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Controllers
{
    public class UserController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        // GET: UserController
        public IActionResult Index()
        {
            var result = UserManager.Users.OrderBy(m => m.FristName).ToList();
            return View(result);
        }

         [HttpGet]
        public IActionResult Create()
        {
          
            var result = RoleManager.Roles.OrderBy(r => r.Name).ToList();
            var model = new RegisterVm()
            {
                Roles = new SelectList(result, "Name", "Name")
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterVm model)
        {
     
            if (ModelState.IsValid)
            {

                var identityUser = new ApplicationUser()
                {
                    FristName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    BirthDay = model.BirthDay,
                    Email= model.Email,
                    UserName = model.Email,

                };

                IdentityResult result = await UserManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(identityUser, model.RoleName);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }

            var roles = RoleManager.Roles.OrderBy(r => r.Name).ToList();
            model.Roles = new SelectList(roles, "Name", "Name");           
            return View(model);
        }



        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var role = await UserManager.GetRolesAsync(user);
            ViewBag.RoleName = role.Count != 0 ? role[0]: "Pa Rol";

            return View(user); 
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (!id.IsNullOrDefault())
            {
                var user = await UserManager.FindByIdAsync(id);
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");

                }
                var errors = result.Errors;
                foreach (var err in errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

            }
            
            return RedirectToAction("Index");
        }

      
    }
}
