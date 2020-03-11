using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Helpers;
using MyPortfolio.Models;
using MyPortfolio.ViewModels;

namespace MyPortfolio.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public ProjetoController(ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var projetos = from p in _context.Projetos
                           select p;

            //... Passa a query proveniente dos filtros e paginação
            var pagination = await Paginacao<Projeto>.Pagination(projetos.AsNoTracking(), 1, 15);

            var vm = new ProjetoIndexViewModel
            {
                ProjetoFiltro = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Título", Value = "1" },
                    new SelectListItem { Text = "Categoria", Value = "2" }

                },
                PagedProjetos = pagination
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string sortOrder,
           string currentFilter,
           string searchString,
           ProjetoIndexViewModel model,
           int? pageNumber,
           int pageSize = 15)
        {
            var projetos = from p in _context.Projetos
                           select p;

            //... Se foi enviado texto no campo de busca
            if (!string.IsNullOrEmpty(searchString))
            {
                //... Set up da página para o iníceio
                pageNumber = 1;

                //... Busca pelo usuário como padrão se não encontrar busca pelo e - mail
                switch (model.FiltroId)
                {
                    case "1":
                        projetos = projetos.Where(s => s.Titulo.ToUpper().Contains(searchString.ToUpper()));
                        break;

                    case "2":
                        projetos = projetos.Where(s => s.Categoria.ToUpper().Contains(searchString.ToUpper()));
                        break;
                }
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            //... Passa a query proveniente dos filtros e paginação
            var pagination = await Paginacao<Projeto>.Pagination(projetos.AsNoTracking(), pageNumber ?? 1, pageSize);

            var vm = new ProjetoIndexViewModel
            {
                PagedProjetos = pagination,
                ProjetoFiltro = model.ProjetoFiltro
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        //Criar o post de Projeto/Create .  .

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return RedirectToAction("Index");
        }

    }
}