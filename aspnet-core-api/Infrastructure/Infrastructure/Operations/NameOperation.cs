using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Operations
{
    public class NameOperation
    {
        public static string CharacterRegulatory(string name)
        {
            name = Regex.Replace(name, @"[\""!'^+%&/()=?_@€¨~,;:.<>|æß]", "");

            var replacements = new Dictionary<string, string>
            {
                { "Ö", "o" }, { "ö", "o" }, { "Ü", "u" }, { "ü", "u" },
                { "ı", "i" }, { "İ", "i" }, { "ğ", "g" }, { "Ğ", "g" },
                { "â", "a" }, { "î", "i" }, { "ş", "s" }, { "Ş", "s" },
                { "Ç", "c" }, { "ç", "c" }
            };

            foreach (var pair in replacements)
            {
                name = name.Replace(pair.Key, pair.Value);
            }

            name = name.Replace(" ", "-");

            return name;
        }
    }
}
