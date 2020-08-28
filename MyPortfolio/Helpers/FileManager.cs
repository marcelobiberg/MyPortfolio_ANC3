using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Helpers
{
    public class FileManager
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileManager(IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Remove arquivo do sistema
        /// </summary>
        /// <param name="path">Caminho completo para o arquivo</param>
        public void RemoveFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message + " InnerEx" + ex.InnerException);
            }
        }

        /// <summary>
        /// Adiciona arquivo no caminho informado via param
        /// </summary>
        /// <param name="path">Caminho completo para o diretório do arquivo</param>
        /// <param name="file">Arquivo físico</param>
        public void AddFile(string path, IFormFile file)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message + " InnerEx" + ex.InnerException);
            }
        }

        /// <summary>
        /// Retorna o caminho relativo para pasta padrão ( DefaultFolder )
        /// </summary>
        public string RelativePath()
        {
            return _configuration["File:DefaultFolder"];
        }

        /// <summary>
        /// Retorna o caminho absoluto para a pasta padrão
        /// </summary>
        public string AbsolutePath()
        {
            return _hostingEnvironment.WebRootPath + RelativePath();
        }

        /// <summary>
        /// Retorna o caminho relativo para o avatar padrão
        /// </summary>
        public string DefaultRelativePathAvatar()
        {
            return _configuration["File:DefaultRelativePathToAvatar"];
        }

        /// <summary>
        /// Valida avatar padrão
        /// </summary>
        /// <param name="avatar">Nome do avatar</param>
        public bool IsAvatarDefault(string avatar)
        {
            var defaultAvatar = _configuration["File:DefaultAvatar"];

            if (avatar.Equals(defaultAvatar))
            {
                return true;
            }

            return false;
        }
    }
}
