using AdminDashBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace AdminDashBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.Users.Select(i => new UserViewModel
            {
                Id = i.Id,
                DisplayName = i.DisplayName,
                UserName = i.UserName,
                Email = i.Email,
                Roles = _userManager.GetRolesAsync(i).Result,
            }).ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var viewmodel = new UserRoleViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result

                }).ToList(),
            };
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var userroles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userroles.Any(p=>p == role.Name && !role.IsSelected))
                    await _userManager.RemoveFromRoleAsync(user, role.Name);


                if (!userroles.Any(e => e == role.Name && role.IsSelected))
                    await _userManager.AddToRoleAsync(user, role.Name);
            }
            return RedirectToAction("Index");
        }
    } 
}
