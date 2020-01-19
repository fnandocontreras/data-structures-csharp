using System;
using System.Collections.Generic;
using System.IO;
using Libs.Compression;

namespace Algo
{
    class Program
    {
        static string FolderPath = @"C:\Users\fcontreras\OneDrive\Algo";
        static void Main(string[] args)
        {
            Console.WriteLine("started");

            var output = new FileStream(Path.Combine(FolderPath, "lorem-ipsum-compressed.txt"), FileMode.Create);
            using BinaryWriter writer = new BinaryWriter(output);


            var compressor = new HauffmanCompressor();
            var compressed = compressor.FitEncode(ReadInputFile());
            foreach (var line in compressed)
            {
                writer.Write(line);
            }

        }

        static IEnumerable<string> ReadInputFile()
        {
            using StreamReader reader = new StreamReader(Path.Combine(FolderPath, "lorem-ipsum.txt"));
            while (!reader.EndOfStream)
                yield return reader.ReadLine();
        }
    }
}
