using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CropToFace
{
    class Program
    {
        static IConfiguration configuration;
        static string inPath;
        static string outPath;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            if (args.Length >= 2)
            {
                inPath = args[0];
                outPath = args[1];
            }
            else
            {
                Console.WriteLine("Enter path to input file.");
                inPath = Console.ReadLine();
                Console.WriteLine("Enter path to output file.");
                outPath = Console.ReadLine();
            }

            IFaceCropper c = new AzureCongServFaceAPI_Cropper
            {
                Config = configuration,
                InputFile = inPath,
                OutputFile = outPath
            };
            c.Crop();
        }
    }
}
