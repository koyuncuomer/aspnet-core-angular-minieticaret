using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Operations;

namespace Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected string FileRename(string pathOrContainerName, string fileName, HasFile hasFileMethod)
        {
            string extension = Path.GetExtension(fileName);
            string regulatedNameWithoutExtension = NameOperation.CharacterRegulatory(Path.GetFileNameWithoutExtension(fileName));
            string regulatedNameWithExtension = $"{regulatedNameWithoutExtension}{extension}";

            int counter = 1;
            string newFileName = regulatedNameWithExtension;

            //while (File.Exists(Path.Combine(path, newFileName)))
            while (hasFileMethod(pathOrContainerName, newFileName))
            {
                newFileName = $"{regulatedNameWithoutExtension}-{counter}{extension}";
                counter++;
            }

            return newFileName;
        }

    }
}
