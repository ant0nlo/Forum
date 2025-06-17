using ForumSentiment.Data;
using ForumSentiment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ForumSentiment.ViewModels;
 
namespace ForumSentiment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            // Забраняваме деактивиране на други админи
            if (isAdmin)
            {
                TempData["Error"] = "You cannot deactivate another Admin.";
                return RedirectToAction("Index");
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserAdminViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserAdminViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = roles,
                    IsActive = user.IsActive
                });
            }

            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _userManager.IsInRoleAsync(user, role) == false)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            return RedirectToAction("Index");
        }

    }
}
