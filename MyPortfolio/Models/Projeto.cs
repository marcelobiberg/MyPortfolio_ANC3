using System;
using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Projeto
    {
        [Key]
        public string ID { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Título")]
        [MaxLength(100)]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Descrição")]
        [MaxLength(500)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Categoria")]
        [MaxLength(100)]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Front-end")]
        [MaxLength(100)]
        public string FrontEnd { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Back-end")]
        [MaxLength(100)]
        public string BackEnd { get; set; }
        [Required(ErrorMessage = "Favor preencher o campo Banco de dados")]
        [MaxLength(100)]
        public string BancoDados { get; set; }
        [MaxLength(40)]
        public string ImgBackground { get; set; }
        [MaxLength(40)]
        public string Img01 { get; set; }
        [MaxLength(40)]
        public string Img02 { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
