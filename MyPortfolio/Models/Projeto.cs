using System;

namespace MyPortfolio.Models
{
    public class Projeto
    {
        public string ID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public string ImgBackground { get; set; }
        public string Img01 { get; set; }
        public string Img02 { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
