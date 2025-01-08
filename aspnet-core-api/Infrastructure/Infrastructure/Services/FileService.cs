using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class FileService
    {


 

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


    }
}
