using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TMac
{
    static class Mac
    {
        public static List<KeyValuePair<string, string>> Replacements = new List<KeyValuePair<string, string>>();
        public static bool UsePredefined = true;

        public static void Process(string file, string[] lines)
        {
            file = Path.GetFileNameWithoutExtension(file);
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (var r in Replacements)
                {
                    lines[i] = Regex.Replace(lines[i], r.Key, r.Value);
                }
                if (UsePredefined)
                    lines[i] = lines[i].Replace("$file", file);
            }
        }
    }
}
