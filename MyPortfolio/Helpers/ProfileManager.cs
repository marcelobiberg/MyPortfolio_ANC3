using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyPortfolio.Models;
using System.Collections.Generic;

namespace MyPortfolio.Helpers
{
    public class ProfileManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApplicationUser _currentUser;

        public ProfileManager(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Usuário logado no sistema
        /// </summary>
        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;
                }
                return _currentUser;
            }
        }

        public List<SelectListItem> GetRoles()
        {
            var lis = new List<SelectListItem>()
            {
                new SelectListItem { Value = "DESENVOLVIMENTO WEB", Text = "DESENVOLVIMENTO WEB" },
                new SelectListItem { Value = "DESIGN GRÁFICO", Text = "DESIGN GRÁFICO" }
            };

            return lis;
        }
    }
}
