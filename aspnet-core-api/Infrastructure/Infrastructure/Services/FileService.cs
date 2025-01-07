using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly private IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string FileRename(string path, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string regulatedNameWithoutExtension = NameOperation.CharacterRegulatory(Path.GetFileNameWithoutExtension(fileName));
            string regulatedNameWithExtension = $"{regulatedNameWithoutExtension}{extension}";

            int counter = 1;
            string newFileName = regulatedNameWithExtension;

            while (File.Exists(Path.Combine(path, newFileName)))
            {
                newFileName = $"{regulatedNameWithoutExtension}-{counter}{extension}";
                counter++;
            }

            return newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();

            foreach (IFormFile file in files)
            {
                string fileNewName = FileRename(uploadPath, file.FileName);

                bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
                datas.Add((fileNewName, Path.Combine(path, fileNewName)));
                results.Add(result);
            }

            if(results.TrueForAll(r=> r.Equals(true)))
                return datas;

            return null;
            //todo: yukarıdaki if geçerli değilse hata olduğuna dair bi ex fırlatılabilir.
        }
    }
}
