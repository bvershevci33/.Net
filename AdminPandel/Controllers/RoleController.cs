using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Controllers
{
    //[Authorize(Roles ="SuperAdmin")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> RoleManager { get; }

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
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
        public IActionResult Details(int? a)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? a)
        {
            return View();
        }


    }
}
