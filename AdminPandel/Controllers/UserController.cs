using AdminPandel.Models;
using AdminPandel.Utilities;
using AdminPandel.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public IMapper Mapper { get; }

        public UserController(UserManager<ApplicationUser> userManager, 
                              RoleManager<IdentityRole> roleManager,
                              IMapper mapper)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            Mapper = mapper;
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
                ApplicationUser identityUser = Mapper.Map<ApplicationUser>(model);

                //var identityUser = new ApplicationUser()
                //{
                //    FristName = model.FirstName,
                //    LastName = model.LastName,
                //    Address = model.Address,
                //    BirthDay = model.BirthDay,
                //    Email= model.Email,
                //    UserName = model.Email,

                //};

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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {
                var result = await UserManager.FindByIdAsync(id);
                return View(result);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var result = await UserManager.FindByIdAsync(model.Id);

            //var user= Mapper.Map<ApplicationUser>(model);

            result.FristName = model.FristName;
            result.LastName = model.LastName;
            result.Address = model.Address;
            result.Email = model.Email;
            result.BirthDay = model.BirthDay;
            result.PhoneNumber = model.PhoneNumber;


            await UserManager.UpdateAsync(result);
            return RedirectToAction("Index");
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

        public async Task<IActionResult> GetRolesByUser(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            ViewBag.UserName = user.UserName;
            var roles = await UserManager.GetRolesAsync(user);
            List<UserRolesVm> usersRoleVm = new();
            foreach (var role in roles)
            {
                usersRoleVm.Add(new UserRolesVm()
                {
                    
                 
                    UserId = user.Id,
                    UserName = user.UserName,
                    RoleName = role
                });


            }
            return View(usersRoleVm);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRolesToUser(string userName)
        {
            if (!userName.IsNullOrDefault())
            {
                var user = await UserManager.FindByNameAsync(userName);
                var roles = RoleManager.Roles.ToList();
                List<AssignRoleToUser> roleUsers = new();
                foreach (var role in roles)
                {
                    var roleToUser = new AssignRoleToUser()
                    {
                        UserName = user.UserName,
                        RoleName = role.Name,
                    };
                    if (await UserManager.IsInRoleAsync(user, role.Name))
                        roleToUser.IsInUser = true;
                    roleUsers.Add(roleToUser);


                }
                return View(roleUsers);
            }
            return RedirectToAction("GetRolesByUser");

        }

        public async Task<IActionResult> AssignRolesToUser(List<AssignRoleToUser> assignRoleToUsers)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(assignRoleToUsers[0].UserName);
                if (user.IsNullOrDefault())
                {
                    ModelState.AddModelError(string.Empty, $"Useri {user.UserName} nuk ekziston.");
                    return View(assignRoleToUsers);
                }
                var i = 0;
                foreach (var role in assignRoleToUsers)
                {
                    var userIdentity = await UserManager.FindByNameAsync(role.UserName);
                    IdentityResult result = null;
                    if (role.IsInUser && !(await UserManager.IsInRoleAsync(userIdentity, role.RoleName)))
                        result = await UserManager.AddToRoleAsync(userIdentity, role.RoleName);
                    else if (!role.IsInUser && (await UserManager.IsInRoleAsync(userIdentity, role.RoleName)))
                        result = await UserManager.RemoveFromRoleAsync(userIdentity, role.RoleName);
                    else
                        continue;

                    if (result.Succeeded)
                    {
                        if (i < (assignRoleToUsers.Count - 1))
                            continue;
                        else
                            return RedirectToAction("GetRolesByUser", new { userName = user.UserName });
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);

                        }
                        return View(assignRoleToUsers);
                    }

                    i++;
                }
                return RedirectToAction("GetRolesByUser", new { userName = user.UserName });

            }
            return View(assignRoleToUsers);
        }

    }
}
