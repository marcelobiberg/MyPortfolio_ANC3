using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyPortfolio.Models;
using System.Security.Claims;

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
    }
}
