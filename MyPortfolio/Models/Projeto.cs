using System;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Projeto
    {
        [Key]
        public string ID { get; set; }
        [MaxLength(100)]
        public string Titulo { get; set; }
        [MaxLength(500)]
        public string Descricao { get; set; }
        [MaxLength(100)]
        public string Tipo { get; set; }
        [MaxLength(100)]
        public string FrontEnd { get; set; }
        [MaxLength(100)]
        public string BackEnd { get; set; }
        [MaxLength(100)]
        public string BancoDados { get; set; }
        [MaxLength(300)]
        public string ImgBackground { get; set; }
        [MaxLength(300)]
        public string Img01 { get; set; }
        [MaxLength(300)]
        public string Img02 { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
