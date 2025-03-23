using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Identity;

namespace AdminDashBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto Model)
        {
            var User = await _userManager.FindByEmailAsync(Model.Email);
            if (User == null)
            {
                ModelState.AddModelError("Email", "InvalidEmail");
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.CheckPasswordSignInAsync(User, Model.Password, false);
            if (!result.Succeeded || !await _userManager.IsInRoleAsync(User, "Admin"))
            {
                ModelState.AddModelError(string.Empty, "You are Not Authorized");
                return RedirectToAction(nameof(Login));
            }
            else {
                return RedirectToAction("Index","Home");
            }
        }
    }
}
