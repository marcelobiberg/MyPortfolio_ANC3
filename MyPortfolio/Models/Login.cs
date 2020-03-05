using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Models
{
    public class Login
    {
        [Display(Name ="E-mail")]
        [Required(ErrorMessage ="E-mail obrigatório")]
        [EmailAddress(ErrorMessage ="E-mail inválido")]
        public string Email { get; set; }

        [Display(Name ="Senha")]
        [Required(ErrorMessage = "Senha obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
