using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyPortfolio.Data;
using MyPortfolio.Helpers;
using MyPortfolio.Models;
using MyPortfolio.Models.ViewModels.Projetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        private readonly FileManager _fileManager;

        public ProjetoController(ApplicationDbContext context,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            FileManager fileManager)
        {
            _context = context;
            _hostingEnvironment = environment;
            _configuration = configuration;
            _fileManager = fileManager;
        }

        #region Index
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
        #endregion

        #region Criar
        public IActionResult Create()
        {
            var vm = new ProjetoCreateEditViewModel
            {
                FEList = GetFE().ToList(),
                BEList = GetBE().ToList(),
                DBList = GetDatabases().ToList(),
                CatList = GetCategories().ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjetoCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.FEList = GetFE().ToList();
                vm.BEList = GetBE().ToList();
                vm.DBList = GetDatabases().ToList();
                vm.CatList = GetCategories().ToList();
                return View(vm);
            }

            //Caminho raiz para os arquivos
            var rootPath = _hostingEnvironment.WebRootPath;
            //Caminho para o diretório padrão
            var pathToDir = rootPath + _configuration["File:DefaultFolder"];
            //Nome único para o arquivo e concat da extensão 
            var imgBg = $@"{Guid.NewGuid()}" + Path.GetExtension(vm.ImgBackground.FileName);
            var img01 = $@"{Guid.NewGuid()}" + Path.GetExtension(vm.Img01.FileName);
            var img02 = $@"{Guid.NewGuid()}" + Path.GetExtension(vm.Img02.FileName);

            //. . . Salva os arquivos fisicamente
            _fileManager.AddFile(Path.Combine(pathToDir, imgBg), vm.ImgBackground);
            _fileManager.AddFile(Path.Combine(pathToDir, img01), vm.ImgBackground);
            _fileManager.AddFile(Path.Combine(pathToDir, img02), vm.ImgBackground);

            try
            {
                var project = new Projeto
                {
                    ID = Guid.NewGuid().ToString(),
                    Titulo = vm.Titulo,
                    Tipo = vm.Tipo,
                    BancoDados = vm.BancoDados,
                    BackEnd = vm.BackEnd,
                    FrontEnd = vm.FrontEnd,
                    Descricao = vm.Descricao,
                    ImgBackground = imgBg,
                    Img01 = img01,
                    Img02 = img02,
                    GitUrl = vm.GitUrl,
                    SiteUrl = vm.SiteUrl,
                    CreatedOn = DateTime.Now
                };

                await _context.Projetos.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message + " / " + ex.InnerException);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Detalhes
        public async Task<IActionResult> Details(string Id)
        {
            var project = await _context.Projetos.FindAsync(Id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        #endregion

        #region Editar
        public async Task<IActionResult> Edit(string Id)
        {
            var projeto = await _context.Projetos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == Id);

            var vm = new ProjetoCreateEditViewModel
            {
                ID = Id,
                Titulo = projeto.Titulo,
                Tipo = projeto.Tipo,
                BancoDados = projeto.BancoDados,
                BackEnd = projeto.BackEnd,
                FrontEnd = projeto.FrontEnd,
                Descricao = projeto.Descricao,
                GitUrl = projeto.GitUrl,
                SiteUrl = projeto.SiteUrl,
                BackgroundPath = $@"{_configuration["File:DefaultFolder"]}" + projeto.ImgBackground,
                Img01Path = $@"{_configuration["File:DefaultFolder"]}" + projeto.Img01,
                Img02Path = $@"{_configuration["File:DefaultFolder"]}" + projeto.Img02,
                FEList = GetFE().ToList(),
                BEList = GetBE().ToList(),
                DBList = GetDatabases().ToList(),
                CatList = GetCategories().ToList()
            };

            if (projeto == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(string id)
        {
            var projeto = await _context.Projetos
                .FirstOrDefaultAsync(p => p.ID == id);

            projeto.UpdatedOn = DateTime.UtcNow;

            if (await TryUpdateModelAsync<Projeto>(projeto, "",
                c => c.Titulo,
                c => c.Tipo,
                c => c.BackEnd,
                c => c.FrontEnd,
                c => c.BancoDados,
                c => c.GitUrl,
                c => c.SiteUrl,
                c => c.Descricao))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Erro ao atualizar projeto: " + ex.InnerException + " : " + ex.Message);
                }
            }

            return View(projeto);
        }

        /// <summary>
        /// Atualiza as imagens do projeto via AJAX
        /// </summary>
        /// <param name="ProjetoId">ID do projeto</param>
        /// <param name="Tipo">Tipos: ImgBackground, Img01 e Img02</param>
        /// <param name="File">Arquivo físico selecionado pelo usuário</param>
        [HttpPost]
        public async Task<IActionResult> UpdateImageAjax(UpdateImageAjaxViewModel vm)
        {
            if (vm.File == null)
            {
                return NotFound();
            }

            //Busca o projeto
            var projeto = await _context.Projetos.FindAsync(vm.ProjetoId);
            //Variáveis do sistema
            var fileName = string.Empty;
            var fullPath = string.Empty;
            var tipoFile = string.Empty;
            //Caminho raiz para os arquivos
            var rootPath = _hostingEnvironment.WebRootPath;
            var defaultPath = _configuration["File:DefaultFolder"];
            var folderPath = rootPath + defaultPath;

            //Tipo do arquivo
            // *imgBackground: Imagem de background
            // *img1: Imagem 01
            // *img2: Imagem 02
            switch (vm.Tipo)
            {
                case "imgBackground":
                    //... Polula a variável do sistema com o nome do novo arquivo ( Imagem )
                    fileName = projeto.ImgBackground;
                    //... Remove o arquivo ( Imagem ) anterior
                    _fileManager.RemoveFile(Path.Combine(folderPath, fileName));
                    //... Resgata a extensão do novo arquivo
                    tipoFile = Path.GetExtension(vm.File.FileName);
                    fileName = $@"{Guid.NewGuid()}" + tipoFile;
                    //... Atualiza o registro
                    projeto.ImgBackground = fileName;
                    break;
                case "img1":
                    //... Polula a variável do sistema com o nome do novo arquivo ( Imagem )
                    fileName = projeto.Img01;
                    //... Remove o arquivo ( Imagem ) anterior
                    _fileManager.RemoveFile(Path.Combine(folderPath, fileName));
                    //... Resgata a extensão do novo arquivo
                    tipoFile = Path.GetExtension(vm.File.FileName);
                    fileName = $@"{Guid.NewGuid()}" + tipoFile;
                    //... Atualiza o registro
                    projeto.Img01 = fileName;
                    break;
                case "img2":
                    //... Polula a variável do sistema com o nome do novo arquivo ( Imagem )
                    fileName = projeto.Img02;
                    //... Remove o arquivo ( Imagem ) anterior
                    _fileManager.RemoveFile(Path.Combine(folderPath, fileName));
                    //... Resgata a extensão do novo arquivo
                    tipoFile = Path.GetExtension(vm.File.FileName);
                    fileName = $@"{Guid.NewGuid()}" + tipoFile;
                    //... Atualiza o registro
                    projeto.Img02 = fileName;
                    break;
            }

            //Salva a nova imagem no diretório padrão
            _fileManager.AddFile(Path.Combine(folderPath, fileName), vm.File);
            //Atualiza o registro
            projeto.UpdatedOn = DateTime.Now;
            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();
            //Retorna o caminho completoo até o arquivo via o ajax
            fullPath = Path.Combine(defaultPath, fileName);
            return Ok(fullPath);
        }
        #endregion

        #region Remove
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var p = await _context.Projetos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == Id);

            return View(p);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(string Id)
        {
            var p = await _context.Projetos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == Id);

            try
            {
                //Remove o projeto do DB
                _context.Entry(p).State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message + " InnerEx" + ex.InnerException);
            }

            //Caminho raiz para os arquivos
            var rootPath = _hostingEnvironment.WebRootPath;
            var defaultPath = _configuration["File:DefaultFolder"];
            //Caminho para o diretório padrão
            var folderPath = Path.Combine(rootPath, defaultPath);

            //Remove as arquivos relacionados com o projeto
            _fileManager.RemoveFile(Path.Combine(folderPath, p.ImgBackground));
            _fileManager.RemoveFile(Path.Combine(folderPath, p.Img01));
            _fileManager.RemoveFile(Path.Combine(folderPath, p.Img02));

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        public static List<SelectListItem> GetDatabases()
        {
            var lis = new List<SelectListItem>()
            {
                new SelectListItem { Value = "SQL SERVER", Text = "SQL SERVER" },
                new SelectListItem { Value = "MY SQL", Text = "MY SQL" },
                new SelectListItem { Value = "POSTGRE", Text = "POSTGRE" }
            };

            return lis;
        }

        public static List<SelectListItem> GetCategories()
        {
            var lis = new List<SelectListItem>()
            {
                new SelectListItem { Value = "DESENVOLVIMENTO WEB", Text = "DESENVOLVIMENTO WEB" },
                new SelectListItem { Value = "DESIGN GRÁFICO", Text = "DESIGN GRÁFICO" }
            };

            return lis;
        }

        public static List<SelectListItem> GetFE()
        {
            var lis = new List<SelectListItem>()
            {
                new SelectListItem { Value = "BOOTSTRAP 4", Text = "BOOTSTRAP 4" },
                new SelectListItem { Value = "BOOTSTRAP 3", Text = "BOOTSTRAP 3" }
            };

            return lis;
        }

        public static List<SelectListItem> GetBE()
        {
            var lis = new List<SelectListItem>()
            {
                new SelectListItem { Value = "ASP.NET", Text = "ASP.NET" },
                new SelectListItem { Value = "ASP.NET CORE 2X", Text = "ASP.NET CORE 2X" },
                new SelectListItem { Value = "ASP.NET CORE 3X", Text = "ASP.NET CORE 3X" },
                new SelectListItem { Value = "Blazor", Text = "Blazor" },
            };

            return lis;
        }
        #endregion
    }
}