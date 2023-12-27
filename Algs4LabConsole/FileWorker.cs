using System;
using System.IO;
using System.Text;

namespace Alg4Lab
{
    public static class DataWorker
    {
        public static string GetAttributesFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                var line = sr.ReadLine();
                if (line != null)
                    return line;
            }
            return String.Empty;
        }
    }
}
