using AdminPandel.Models;
using AdminPandel.Utilities;
using AdminPandel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<ApplicationUser> UserManager { get; }

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var result = RoleManager.Roles.OrderBy(m=> m.Name).ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

            }
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {
                var result = await RoleManager.FindByIdAsync(id);
                return View(result);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            var result = await RoleManager.FindByIdAsync(model.Id);

            result.Name = model.Name;

            await RoleManager.UpdateAsync(result);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!id.IsNullOrDefault())
            {
                var identityRole = await RoleManager.FindByIdAsync(id);
                return View(identityRole);

            }
            return View();
          
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!id.IsNullOrDefault())
            {

                var identityRole = await RoleManager.FindByIdAsync(id);
                if (!identityRole.IsNullOrDefault())
                {
                    var result = await RoleManager.DeleteAsync(identityRole);                   
                    return RedirectToAction("Index");
                }              

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByRoleName(string roleName) 
        {
            if (!roleName.IsNullOrDefault())
            {
                var role = await RoleManager.FindByNameAsync(roleName);
                ViewBag.RoleName = roleName;
                var users = await UserManager.GetUsersInRoleAsync(roleName);
                List<UserRolesVm> usersRoleVm = new();
                foreach (var user in users)
                {
                    usersRoleVm.Add(new UserRolesVm() { 
                        RoleId= role.Id,
                        RoleName = role.Name,
                        UserId= user.Id,
                        UserName = user.UserName
                    });
                }

                return View(usersRoleVm);

            }
            return View();
           
        }

        [HttpGet]
        public async Task<IActionResult> AssignUsersToRole(string roleName)
        {
            if (!roleName.IsNullOrDefault())
            {
                var role = await RoleManager.FindByNameAsync(roleName);
                ViewBag.RoleName = roleName;
                var users = UserManager.Users.ToList();
                List<AssignUserToRole> userRoles = new();

                foreach (var user in users)
                {
                    var userToRole = new AssignUserToRole()
                    {
                        UserName = user.UserName,
                        RoleName = role.Name
                    };

                    if(await UserManager.IsInRoleAsync(user,role.Name))
                        userToRole.IsInRole = true;
                    userRoles.Add(userToRole);
                }
                return View(userRoles);
            }


            return RedirectToAction("GetUsersByRoleName");
        }

        [HttpPost]
        public async Task<IActionResult> AssignUsersToRole(List<AssignUserToRole> assignUserToRoles)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByNameAsync(assignUserToRoles[0].RoleName);
                if (role.IsNullOrDefault())
                {
                    ModelState.AddModelError(string.Empty, $"Roli {role.Name} nuk ekziston.");
                    return View(assignUserToRoles);
                }

                var i = 0;
                foreach (var user in assignUserToRoles)
                {
                    var userIdentity = await UserManager.FindByNameAsync(user.UserName);
                    IdentityResult result = null;
                    if (user.IsInRole && !(await UserManager.IsInRoleAsync(userIdentity, role.Name)))
                        result = await UserManager.AddToRoleAsync(userIdentity, role.Name);
                    else if (!user.IsInRole && (await UserManager.IsInRoleAsync(userIdentity, role.Name)))
                        result = await UserManager.RemoveFromRoleAsync(userIdentity, role.Name);
                    else
                        continue;

                    if (result.Succeeded)
                    {
                        if (i < (assignUserToRoles.Count - 1))
                            continue;
                        else
                            return RedirectToAction("GetUsersByRoleName", new { roleName = role.Name });
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);

                        }
                        return View(assignUserToRoles);
                    }

                    i++;

                }

                return RedirectToAction("GetUsersByRoleName", new { roleName = role.Name });

            }

            return View(assignUserToRoles);
        }


        }
}
