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
            Console.WriteLine("Tiny macro processor");
            Console.WriteLine("tmac [options] <files>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine();
            Console.WriteLine("-help                       Show help");
            Console.WriteLine("-version                    Show version");
            Console.WriteLine();
            Console.WriteLine("-r <pattern> <replacement>  Replace text");
            Console.WriteLine("                            Pattern is a regular expression");
            Console.WriteLine();
            Console.WriteLine("@file                       Read args from response file");
        }

        static void Version()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine("{0} {1}", assemblyName.Name, assemblyName.Version.ToString(1));
        }

        static List<string> Filenames = new List<string>();

        static void Expand(string s)
        {
            // Unix shells have already expanded wild cards 
            if (Path.DirectorySeparatorChar == '/')
            {
                Filenames.Add(s);
                return;
            }

            // Not a wild card
            if (!s.Contains("*") && !s.Contains("?"))
            {
                Filenames.Add(s);
                return;
            }

            // Current directory
            if (!s.Contains("\\"))
            {
                Filenames.AddRange(Directory.GetFiles(".", s));
                return;
            }

            // Other directory
            var path = Path.GetDirectoryName(s);
            var pattern = Path.GetFileName(s);
            Filenames.AddRange(Directory.GetFiles(path, pattern));
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
                    case "r":
                        if (++i >= args.Length)
                            throw new IOException("-r: Expected pattern");
                        var pattern = args[i];
                        if (++i >= args.Length)
                            throw new IOException("-r: Expected replacement");
                        var replacement = args[i];
                        Mac.Replacements.Add(new KeyValuePair<string, string>(pattern, replacement));
                        break;
                    default:
                        throw new IOException(args[i] + ": Unknown option");
                }
            }
        }

        static bool IsBinary(string filename)
        {
            var bytes = File.ReadAllBytes(filename);
            foreach (var b in bytes)
                if (b == 0)
                    return true;
            return false;
        }

        static void Main(string[] args)
        {
            try
            {
                Parse(args);
                foreach (var filename in Filenames)
                {
                    if (IsBinary(filename))
                    {
                        Console.WriteLine("{0}: binary file", filename);
                        continue;
                    }
                    var lines = File.ReadAllLines(filename);
                    var old = lines.ToArray();
                    Mac.Process(filename, lines);
                    var changed = false;
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i] != old[i])
                        {
                            changed = true;
                            break;
                        }
                    if (changed)
                    {
                        File.WriteAllLines(filename, lines);
                        Console.WriteLine(filename);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            if (Debugger.IsAttached)
                Console.ReadKey();
        }
    }
}
