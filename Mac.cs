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
        public static bool UseBuiltin;
        public static RegexOptions Options = RegexOptions.None;

        public static void Process(string filename, string[] lines)
        {
            filename = Path.GetFileNameWithoutExtension(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                if (UseBuiltin)
                {
                    lines[i] = lines[i].Replace("$file", filename);
                }
                foreach (var r in Replacements)
                {
                    lines[i] = Regex.Replace(lines[i], r.Key, r.Value, Options);
                }
            }
        }
    }
}
