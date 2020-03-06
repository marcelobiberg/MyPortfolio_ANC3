using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Controllers
{
    public class ProjetoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}