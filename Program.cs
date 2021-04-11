using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SvgPointsToPath
{
    public static class Program
    {
        private static string NameSpace = "CarbonBlazor";
        private static string Class = "IconPath";
        private static string PathToFolder;
        private static string OutputPath;

        public static List<string> paths = new List<string>();

        private static void Main(string[] args)
        {
            ParseArgs(args);
            paths = Directory.GetFiles(PathToFolder, "*", SearchOption.AllDirectories).ToList();
            Create();

            Console.WriteLine($"[{DateTime.UtcNow}]: Well Done! File svg-storage with name {Class} created by next path: {OutputPath}");
        }

        private static void ParseArgs(string[] args)
        {
            if (args.Count() == 1) 
            {
                ShowHelp(args[0]);
                return;
            }
            if (args.Count() != 4) throw new ArgumentException("Args must be four");

            var _path = args[0];
            if(!Directory.Exists(_path)) throw new DirectoryNotFoundException($"Directory by: \"{_path}\" not exist");

            var output = args[3];
            if(!Directory.Exists(output)) throw new DirectoryNotFoundException($"Directory by: \"{output}\" not exist");

            PathToFolder = _path;
            NameSpace = args[1];
            Class = args[2];
            OutputPath = args[3];
        }

        private static void ShowHelp(string arg)
        {
            if(arg != "-h") throw new ArgumentException("Maybe you need a help? Print '-h'");

            Console.WriteLine("Args must be next: Path NameSpace Class Output");
            Console.WriteLine("'Path' - full path to the folder with svg files");
            Console.WriteLine("'NameSpace' - NameSpace class where store svg");
            Console.WriteLine("'Class' - Class name where store svg");
            Console.WriteLine("'Output' - Path where will be created cs file with svg");

            Environment.Exit(0);
        }

        private static void Create()
        {
            var lines = new List<string>
            {
                "namespace " + NameSpace,
                "{",
                "\tpublic static class " + Class,
                "\t{",
            };

            foreach (var path in paths)
            {
                var points = ParsePoints(path).PointsToPath();

                var _paths = ParsePath(path);
                var _path = string.Join("", _paths).Replace("\"", "\'");

                var title = Path.GetFileNameWithoutExtension(path).ToCamelCase();
                var titleTag = $"<title>{title}</title>";

                var end = points + _path + titleTag;

                lines.Add($"\t\tpublic const string {title} = " + "@\"" + end + "\"" + ";" + Environment.NewLine);
            }

            lines.Add("\t}");
            lines.Add("}");

            OutputPath = Path.Combine(OutputPath, Class + ".cs");
            File.WriteAllLines(OutputPath, lines, Encoding.UTF8);
        }

        private static List<string> ParsePath(string path)
        {
            var document = XDocument.Load(path);
            var root = document.Root;

            if(!Contains(path, "<path")) return new List<string>();

            return root.Descendants("{http://www.w3.org/2000/svg}path")
                .Select(i => i.ToString())
                .ToList();
        }

        private static List<string> ParsePoints(string path)
        {
            if(!Contains(path, "points=")) return new List<string>();

            var document = new XPathDocument(path);
            var navigator = document.CreateNavigator();

            var expression = navigator.Compile("//@points");

            var iterator = navigator.Select(expression);
            
            var paths = new List<string>();
            while (iterator.MoveNext())
            {
                paths.Add(iterator.Current.Value);
            }

            return paths;
        }

        private static string ParseTitle(string path)
        {
            if(!Contains(path, "title")) return "path";

            var document = XDocument.Load(path);
            var root = document.Root;
            return root.Descendants("{http://www.w3.org/2000/svg}title").First().Value;
        }

        public static string PointsToPath(this IEnumerable<string> points)
        {
            if (points.Count() == 0) return "";

            var path = string.Join(" ", points);
            return "<path d=\'M" + path + "z\'></path>";
        }

        private static string ToCamelCase(this string title)
        {
            title = title.Replace("--", "&");
            title = title.Replace("-", "&");
            title = title.Replace("&", " ");

            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(title).Replace(" ", "");
        }

        private static bool Contains(string path, string element)
        {
            return File.ReadAllText(path).Contains(element);
        }
    }
}
