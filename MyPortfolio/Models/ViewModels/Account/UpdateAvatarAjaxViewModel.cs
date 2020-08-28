using Microsoft.AspNetCore.Http;

namespace MyPortfolio.Models.ViewModels.Account
{
    public class UpdateAvatarAjaxViewModel
    {
        public string profileId { get; set; }
        public IFormFile File { get; set; }
    }
}
