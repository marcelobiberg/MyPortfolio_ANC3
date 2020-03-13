using MyPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.ViewModels
{
    public class SiteViewModel
    {
        public ApplicationUser Profile { get; set; }
        public List<Projeto> Projetos { get; set; }
    }
}
