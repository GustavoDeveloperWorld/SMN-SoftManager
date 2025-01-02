using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using System.Security.Claims;

namespace SoftManager.Presentation.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApplicationUserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var manager = await _userManager.GetUserAsync(User);
            if (manager == null || !manager.IsManager)
            {
                TempData["ErrorMessage"] = "Você não tem permissão para acessar esta funcionalidade.";
                return RedirectToAction("Login", "Account");
            }

            var users = await _userManager.Users
                .Where(u => u.ManagerId == manager.Id)
                .ToListAsync();

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user, string password)
        {
            if (ModelState.IsValid)
            {
                var manager = await _userManager.GetUserAsync(User);
                if (manager == null || !manager.IsManager)
                {
                    return Forbid();
                }
                user.UserName = user.Email;
                user.ManagerId = manager.Id;

                if (user.ProfilePictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{user.ProfilePictureFile.FileName}";
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile_pictures");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.ProfilePictureFile.CopyToAsync(stream);
                    }

                    user.ProfilePicturePath = $"/uploads/profile_pictures/{fileName}";
                }

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Funcionario");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("Password"))
                    {
                        ViewData["PasswordError"] = error.Description;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.FullName = user.FullName;
                existingUser.BirthDate = user.BirthDate;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Mobile = user.Mobile;
                existingUser.Address = user.Address;
                existingUser.Email = user.Email;

                if (user.ProfilePictureFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{user.ProfilePictureFile.FileName}";
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile_pictures");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.ProfilePictureFile.CopyToAsync(stream);
                    }

                    existingUser.ProfilePicturePath = $"/uploads/profile_pictures/{fileName}";
                }

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(user);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }
}

