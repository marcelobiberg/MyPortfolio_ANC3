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
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;

        public ProjetoController(ApplicationDbContext context,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = environment;
            _configuration = configuration;
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
        #endregion

        #region Detalhes
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
        #endregion

        #region Editar
        public async Task<IActionResult> Edit(string Id)
        {
            var projeto = await _context.Projetos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == Id);

            if (projeto == null)
            {
                return NotFound();
            }

            //Caminho para o diretório
            ViewBag.DirPath = _configuration["File:DefaultFolder"];

            return View(projeto);
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
            //Caminho para o diretório
            ViewBag.DirPath = _configuration["File:DefaultFolder"];
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
            var tipoFile = string.Empty;
            //Caminho raiz para os arquivos
            var rootPath = _hostingEnvironment.WebRootPath;
            var defaultPath = _configuration["File:DefaultFolder"];
            //Caminho para o diretório padrão
            var folderPath = Path.Combine(rootPath, defaultPath);

            //Inicia instância do objeto FileManager
            FileManager fm = new FileManager();
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
                    fm.RemoveFile(Path.Combine(folderPath, fileName));
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
                    fm.RemoveFile(Path.Combine(folderPath, fileName));
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
                    fm.RemoveFile(Path.Combine(folderPath, fileName));
                    //... Resgata a extensão do novo arquivo
                    tipoFile = Path.GetExtension(vm.File.FileName);
                    fileName = $@"{Guid.NewGuid()}" + tipoFile;
                    //... Atualiza o registro
                    projeto.Img02 = fileName;
                    break;
            }

            //Salva a nova imagem no diretório padrão
            fm.AddFile(Path.Combine(folderPath, fileName), vm.File);
            //Atualiza o registro
            projeto.UpdatedOn = DateTime.Now;
            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();
            //Retorna o nome do novo arquivo para atualizar o ajax
            return Ok(fileName);
        }
        #endregion
    }
}