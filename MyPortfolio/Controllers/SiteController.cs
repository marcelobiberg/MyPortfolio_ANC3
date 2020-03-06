using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyPortfolio.Models;
using MyPortfolio.ViewModels;

namespace MyPortfolio.Controllers
{
    public class SiteController : Controller
    {
        private readonly ILogger<SiteController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public SiteController(ILogger<SiteController> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var email = _configuration["User:Email"];
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var vm = new SiteViewModel
            {
                Profile = user
            };

            return View(vm);
        }
    }
}
