using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Helpers
{
    public class FileManager
    {
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
    }
}
