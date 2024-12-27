using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftManager.Application.Services;
using SoftManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok("Usuário registrado com sucesso.");
            }
            else
            {
                return BadRequest("Falha ao registrar usuário.");
            }
        }
    }
}
