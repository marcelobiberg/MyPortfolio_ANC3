using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Models.ViewModels.Projetos
{
    public class ProjetoCreateEditViewModel
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Preencher o campo Título")]
        [MaxLength(100, ErrorMessage = "No máximo 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Preencher o campo Descrição")]
        [MaxLength(500, ErrorMessage = "No máximo 500 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Selecionar à Categoria")]
        public string Tipo { get; set; }

        public List<SelectListItem> CatList { get; set; }

        [Required(ErrorMessage = "Selecionar a tecnologia front-end")]
        public string FrontEnd { get; set; }

        public List<SelectListItem> FEList { get; set; }

        [Required(ErrorMessage = "Selecionar a tecnologia back-end")]
        public string BackEnd { get; set; }

        public List<SelectListItem> BEList { get; set; }

        [Required(ErrorMessage = "Selecionar o Banco de dados")]
        public string BancoDados { get; set; }

        public List<SelectListItem> DBList { get; set; }

        [Required(ErrorMessage = "Selecionar imagem de background")]
        public IFormFile ImgBackground { get; set; }

        public string BackgroundPath { get; set; }

        [Required(ErrorMessage = "Selecionar imagem 01")]
        public IFormFile Img01 { get; set; }

        public string Img01Path { get; set; }

        [Required(ErrorMessage = "Selecionar imagem 02")]
        public IFormFile Img02 { get; set; }

        public string Img02Path { get; set; }

    }
}
