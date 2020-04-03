using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
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
                        projetos = projetos.Where(s => s.Tipo.ToUpper().Contains(searchString.ToUpper()));
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

        [HttpPost]
        public async Task<IActionResult> Create(string Titulo,
            string Categoria,
            string desc,
            string BackEnd,
            string FrontEnd,
            string BD,
            List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                var vm = new Projeto
                {
                    Tipo = Categoria,
                    Descricao = desc,
                    BackEnd = BackEnd,
                    FrontEnd = FrontEnd,
                    BancoDados = BD
                };

                return View(vm);
            }

            var project = new Projeto();

            //Caminho raiz para os arquivos
            var rootPath = _hostingEnvironment.WebRootPath;
            //Caminho para o diretório padrão
            var pathToDir = Path.Combine(rootPath, "img/Uploads");

            try
            {
                int count = 0;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        count++;
                        //Resgata a extensão do arquivo
                        var tipoFile = Path.GetExtension(file.FileName);
                        //Nome único para o arquivo
                        var fileName = $@"{Guid.NewGuid()}" + tipoFile;

                        var filePath = Path.Combine(pathToDir, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        switch (count)
                        {
                            case 1:
                                project.ImgBackground = fileName;
                                break;
                            case 2:
                                project.Img01 = fileName;
                                break;
                            case 3:
                                project.Img02 = fileName;
                                break;
                        }
                    }
                }

                project.ID = Guid.NewGuid().ToString();
                project.Titulo = Titulo;
                project.Tipo = Categoria;
                project.BancoDados = BD;
                project.BackEnd = BackEnd;
                project.FrontEnd = FrontEnd;
                project.Descricao = desc;
                project.CreatedOn = DateTime.UtcNow;

                await _context.Projetos.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message + " / " + ex.InnerException);
            }

            return Ok();
        }

        public async Task<IActionResult> Details(string Id)
        {
            var project = await _context.Projetos.FindAsync(Id);
            if (project == null)
            {
                return NotFound();
            }

            if (project.ImgBackground == null)
            {
                project.ImgBackground = "No-Img-Default.jpg";
            }
            if (project.Img01 == null)
            {
                project.Img01 = "No-Img-Default.jpg";
            }
            if (project.Img02 == null)
            {
                project.Img02 = "No-Img-Default.jpg";
            }

            return View(project);
        }

    }
}