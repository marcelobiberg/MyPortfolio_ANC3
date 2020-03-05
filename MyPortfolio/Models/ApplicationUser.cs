using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "bit")]
        public bool Ativo { get; set; }
        [Required(ErrorMessage = "Nome obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nome { get; set; }
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Avatar { get; set; }
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Cargo { get; set; }
        [MaxLength(800, ErrorMessage = "Máximo 800 caracteres")]
        [Column(TypeName = "ntext")]
        public string AboutDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
