using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Alg4Lab
{
    public static class Output
    {

        public static void Print(string path, int delay)
        {
            var lines = GetLinedText(path);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
                Thread.Sleep(delay);
            }
        }

        private static List<string> GetLinedText(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<string> lines = new List<string>();
            string line = sr.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }

            return lines;
        }
    }
}
