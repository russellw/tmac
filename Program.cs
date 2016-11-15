using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TMac
{
    class Program
    {
        static void Help()
        {
            Console.WriteLine("Options:");
            Console.WriteLine();
            Console.WriteLine("-help     Show help");
            Console.WriteLine("-version  Show version");
            Console.WriteLine();
            Console.WriteLine("@file     Read args from response file");
        }

        static void Version()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine("{0} {1}", assemblyName.Name, assemblyName.Version.ToString(1));
        }

        static List<string> Files = new List<string>();

        static void Expand(string s)
        {
            // Unix shells have already expanded wild cards 
            if (Path.DirectorySeparatorChar == '/')
            {
                Files.Add(s);
                return;
            }

            // Not a wild card
            if (!s.Contains("*") && !s.Contains("?"))
            {
                Files.Add(s);
                return;
            }

            // Current directory
            if (!s.Contains("\\"))
            {
                Files.AddRange(Directory.GetFiles(".", s));
                return;
            }

            // Other directory
            var path = Path.GetDirectoryName(s);
            var pattern = Path.GetFileName(s);
            Files.AddRange(Directory.GetFiles(path, pattern));
        }

        static void Parse(string[] args)
        {
            var more = true;
            for (var i = 0; i < args.Length; i++)
            {
                var s = args[i];

                // -- means no more options
                if (!more)
                {
                    Expand(s);
                    continue;
                }
                if (s == "--")
                {
                    more = false;
                    continue;
                }

                // Response file
                if (s.StartsWith("@"))
                {
                    Parse(File.ReadAllLines(s.Substring(1)));
                    continue;
                }

                // On Windows, options can start with /
                if (Path.DirectorySeparatorChar == '\\' && s.StartsWith("/"))
                    s = "-" + s.Substring(1);

                // Not an option
                if (!s.StartsWith("-"))
                {
                    Expand(s);
                    continue;
                }

                // Option
                s = s.TrimStart('-');
                switch (s)
                {
                    case "?":
                    case "h":
                    case "help":
                        Help();
                        Environment.Exit(0);
                        break;
                    case "V":
                    case "v":
                    case "version":
                        Version();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("{0}: Unknown option", args[i]);
                        Environment.Exit(1);
                        break;
                }
            }
        }

        static bool IsBinary(string path)
        {
            var bytes = File.ReadAllBytes(path);
            foreach (var b in bytes)
                if (b == 0)
                    return true;
            return false;
        }

        static void Main(string[] args)
        {
            Parse(args);
            foreach (var file in Files)
            {
                if (IsBinary(file))
                {
                    Console.WriteLine("{0}: binary file", file);
                    continue;
                }
                var lines = File.ReadAllLines(file);
                var old = lines.ToArray();
                Mac.Process(file, lines);
                var changed = false;
                for (int i = 0; i < lines.Length; i++)
                    if (lines[i] != old[i])
                    {
                        changed = true;
                        break;
                    }
                if (changed)
                {
                    File.WriteAllLines(file, lines);
                    Console.WriteLine(file);
                }
            }
            if (Debugger.IsAttached)
                Console.ReadKey();
        }
    }
}
