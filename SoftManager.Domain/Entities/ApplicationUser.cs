using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nome Completo")]
        public string? FullName { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Celular")]
        public string? Mobile { get; set; }

        [Display(Name = "Endereço")]
        public string? Address { get; set; }

        [NotMapped] // Evita que o EF mapeie essa propriedade para o banco de dados
        public IFormFile? ProfilePictureFile { get; set; }

        [Display(Name = "URL da Foto de Perfil")]
        public string? ProfilePicturePath { get; set; }

        [Display(Name = "É Gerente?")]
        public bool IsManager { get; set; }

        public ICollection<UserTask>? Tasks { get; set; } = new List<UserTask>();
    }
}
