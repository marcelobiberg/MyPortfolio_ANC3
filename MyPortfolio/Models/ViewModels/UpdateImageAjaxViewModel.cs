using Microsoft.AspNetCore.Http;

namespace MyPortfolio.ViewModels
{
    public class UpdateImageAjaxViewModel
    {
        public string ProjetoId { get; set; }
        public string Tipo { get; set; }
        public IFormFile File { get; set; }
    }
}
