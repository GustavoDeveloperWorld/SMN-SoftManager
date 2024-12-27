﻿// Licenciado à .NET Foundation sob um ou mais acordos.
// A .NET Foundation licencia este arquivo para você sob a licença MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SoftManager.Domain.Entities;

namespace SoftManager.Presentation.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
        ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
            ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
            ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     Esta API suporta a infraestrutura padrão do ASP.NET Core Identity e não é destinada a ser usada
            ///     diretamente no seu código. Esta API pode mudar ou ser removida em versões futuras.
            /// </summary>
            [Display(Name = "Lembre-se de mim?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Limpa o cookie externo existente para garantir um processo de login limpo
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Isso não conta falhas de login para o bloqueio da conta
                // Para habilitar falhas de senha para ativar o bloqueio da conta, defina lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário entrou no sistema.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Conta do usuário bloqueada.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                    return Page();
                }
            }

            return Page();
        }
    }
}