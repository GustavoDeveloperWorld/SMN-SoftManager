using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftManager.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "O celular é obrigatório.")]
        [Phone(ErrorMessage = "O número de celular não é válido.")]
        [Display(Name = "Celular")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [Display(Name = "Endereço")]
        public string? Address { get; set; }

        [NotMapped]
        public IFormFile? ProfilePictureFile { get; set; }

        [Display(Name = "URL da Foto de Perfil")]
        public string? ProfilePicturePath { get; set; }

        [Display(Name = "É Gerente?")]
        public bool IsManager { get; set; }

        [Display(Name = "Gerente")]
        public string? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public ApplicationUser? Manager { get; set; }

        public ICollection<ApplicationUser>? Subordinates { get; set; } = new List<ApplicationUser>();

        public ICollection<UserTask>? Tasks { get; set; } = new List<UserTask>();
    }
}
