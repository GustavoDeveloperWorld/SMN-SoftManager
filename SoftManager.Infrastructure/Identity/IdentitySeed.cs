using Microsoft.AspNetCore.Identity;
using SoftManager.Domain.Entities;
using System.Threading.Tasks;

namespace SoftManager.Infrastructure.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var defaultUserEmail = "admin@softmanager.com";
            var defaultUserPassword = "Admin@123";

            var userExists = await userManager.FindByEmailAsync(defaultUserEmail);
            if (userExists == null)
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = defaultUserEmail,
                    Email = defaultUserEmail,
                    EmailConfirmed = true,
                    IsManager = true,
                    FullName = "Admin SMN", 
                    Address = "Rua Professora Cristina Di Lorenzo Marscicano, 155, João Pessoa - PB.",
                    BirthDate = DateTime.Now,
                    Mobile = "(11) 96414-1597",
                    PhoneNumber = "1234567890", 
                    
                    
                    
                };

                var result = await userManager.CreateAsync(defaultUser, defaultUserPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
                else
                {
                    throw new System.Exception($"Erro ao criar usuário padrão: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
