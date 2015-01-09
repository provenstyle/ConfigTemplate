using System;
using System.Collections.Generic;
using System.IO;
using DotLiquid;
using DotLiquid.NamingConventions;
using Newtonsoft.Json;

namespace ConfigTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("templatePath jsonDataPath outputPath");
                return;
            }

            var templatePath = args[0];
            var dataPath = args[1];
            var outputPath = args[2];

            if (FileDoesNotExist(new[] {templatePath, dataPath})) return;

            Template.NamingConvention = new CSharpNamingConvention();
            Console.WriteLine("Convention: {0}", Template.NamingConvention.GetType().Name);
            
            var template = Template.Parse(File.ReadAllText(templatePath));

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(dataPath));
            
            var directory = Path.GetDirectoryName(outputPath);
            if (directory == null)
            {
                Console.WriteLine("Dirctory cannot be null: {0}", outputPath);
                return;
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = File.CreateText(outputPath))
            {
                var renderParameters = new RenderParameters
                {
                    LocalVariables = Hash.FromDictionary(dictionary)
                };
                template.Render(writer, renderParameters);
            }

            Console.WriteLine("Wrote template to: {0}", outputPath);
        }

        static bool FileDoesNotExist(string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist: {0}", filePath);
                    return true;
                }
            }
            return false;
        }
    }
}
