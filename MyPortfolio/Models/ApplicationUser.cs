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
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [MaxLength(100)]
        public string Avatar { get; set; }
        [MaxLength(100)]
        public string Cargo { get; set; }
        [MaxLength(500)]
        [Column(TypeName = "ntext")]
        public string AboutDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
