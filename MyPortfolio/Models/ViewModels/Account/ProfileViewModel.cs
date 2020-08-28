using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Models.ViewModels.Account
{
    public class ProfileViewModel
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail inválido")]
        [EmailAddress(ErrorMessage = "Informar e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Usuário obrigatório")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "A senha {0} deve ter ao menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Senha incorreta!")]
        [Display(Name = "Confirmar senha")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Imagem avatar")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Avatar { get; set; }

        public string AvatarRelativePath { get; set; }

        [Display(Name = "Profissão")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Cargo { get; set; }

        [Display(Name = "Descrição sobre o usuário")]
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string AboutDescription { get; set; }

        public string TempMessage { get; set; }

        public List<SelectListItem> RoleList { get; set; }
    }
}
