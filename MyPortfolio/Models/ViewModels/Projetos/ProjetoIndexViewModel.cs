using Microsoft.AspNetCore.Mvc.Rendering;
using MyPortfolio.Helpers;
using MyPortfolio.Models;
using System.Collections.Generic;

namespace MyPortfolio.Models.ViewModels.Projetos
{
    public class ProjetoIndexViewModel
    {
        public Paginacao<Projeto> PagedProjetos { set; get; }
        public List<SelectListItem> ProjetoFiltro { get; set; }
        public string FiltroId { get; set; }
    }
}
