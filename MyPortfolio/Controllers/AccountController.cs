using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Helpers;
using MyPortfolio.Models;
using MyPortfolio.Models.ViewModels.Account;

namespace MyPortfolio.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ProfileManager _profileManager;
        private readonly FileManager _fileManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ProfileManager profileManager,
            FileManager fileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileManager = profileManager;
            _fileManager = fileManager;
        }

        #region Login
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Dashboard", "Admin");
                }
            }

            ModelState.AddModelError("", "Falha na autenticação, favor tentar novamente");
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Site");
        }
        #endregion

        #region Profile
        /// <summary>
        /// Atualiza os dados do cadastro
        /// </summary>
        public IActionResult EditProfile()
        {
            var user = _profileManager
                .CurrentUser;

            var vm = new ProfileViewModel
            {
                ID = user.Id,
                UserName = user.UserName,
                Nome = user.Nome,
                Avatar = user.Avatar,
                Cargo = user.Cargo,
                AvatarRelativePath = Path.Combine(_fileManager.RelativePath(), user.Avatar ?? _fileManager.DefaultRelativePathAvatar()),
                AboutDescription = user.AboutDescription,
                Email = user.Email,
                RoleList = _profileManager.GetRoles()
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel vm)
        {
            var user = _profileManager
                .CurrentUser;

            user.AboutDescription = vm.AboutDescription;
            user.Cargo = vm.Cargo;
            user.Nome = vm.Nome;
            user.UpdatedOn = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                vm.UserName = user.UserName;
                vm.Email = user.Email;
                vm.TempMessage = "Perfil atualizado com sucesso!";
                vm.RoleList = _profileManager.GetRoles();
                return View(vm);
            }

            return View(vm);
        }

        public async Task<IActionResult> ChangeAuth(ProfileViewModel vm)
        {
            var user = _profileManager
                .CurrentUser;

            if (!string.IsNullOrEmpty(vm.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, vm.Password);
            }

            if (!user.Email.Equals(vm.Email, StringComparison.OrdinalIgnoreCase))
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, vm.Email);
                await _userManager.ChangeEmailAsync(user, vm.Email, token);
            }


            if (!user.UserName.Equals(vm.UserName, StringComparison.OrdinalIgnoreCase))
            {
                user.UserName = vm.UserName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                vm.TempMessage = "Perfil atualizado com sucesso!";
                return View("EditProfile", vm);
            }

            return View(vm);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Atualiza as imagens do projeto via AJAX
        /// </summary>
        /// <param name="ProjetoId">ID do projeto</param>
        /// <param name="Tipo">Tipos: ImgBackground, Img01 e Img02</param>
        /// <param name="File">Arquivo físico selecionado pelo usuário</param>
        [HttpPost]
        public async Task<IActionResult> UpdateAvatarAjax(UpdateAvatarAjaxViewModel vm)
        {
            if (vm.File == null)
            {
                return NotFound();
            }

            //Busca o projeto
            var profile = await _userManager.FindByIdAsync(vm.profileId);
            //Variáveis do sistema
            var relativePath = _fileManager.RelativePath();
            var absolutePath = _fileManager.AbsolutePath();

            if (!_fileManager.IsAvatarDefault(profile.Avatar))
            {
                _fileManager.RemoveFile(absolutePath + profile.Avatar);
            }

            //... Resgata a extensão do novo arquivo
            var tipoFile = Path.GetExtension(vm.File.FileName);
            var fileName = $@"{Guid.NewGuid()}" + tipoFile;

            profile.Avatar = fileName;

            //Salva a nova imagem no diretório padrão
            _fileManager.AddFile(_fileManager.AbsolutePath() + fileName, vm.File);

            profile.UpdatedOn = DateTime.Now;
            var result = await _userManager.UpdateAsync(profile);

            if (result.Succeeded)
            {
                return Ok(relativePath + fileName);
            }

            return null;
        }
        #endregion
    }
}