using System;
using System.IO;
using System.Threading;

namespace Alg4Lab
{
    public class TableSorts
    {
        private string AttributeNames;
        private int keyIndex;
        public string Path { get; private set; }
        private string pathA = "tables/aFile.txt";
        private string pathB = "tables/bFile.txt";
        private int delay;

        public TableSorts(string path, int keyIndex, int delay)
        {
            Path = path;
            AttributeNames = GetAttributeNames();
            this.keyIndex = keyIndex;
            this.delay = delay;
        }
        public TableSorts(string path, int keyIndex, int delay, string filterAttr, string filterAttrValue)
        {
            Path = path;
            AttributeNames = GetAttributeNames();
            this.keyIndex = keyIndex;
            this.delay = delay;
            FilterTable(filterAttr, filterAttrValue);
        }

        public void DirectSort()
        {
            long linesCount = GetLinesCount(Path);
            int iterations = (int)Math.Log2(linesCount);

            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine("\nновый шаг: \n");
                int elements = (int)Math.Pow(2, i);
                SplitFile(elements);
                CompareAndWrite(elements);
                Console.WriteLine();
                PrintFile(pathA);
                PrintFile(pathB);
                PrintFile(Path);
            }
            Console.WriteLine("Сортировка завершена.");

        }
        private void PrintFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {

                var line = sr.ReadLine();
                while (line != null)
                {
                    Console.Write($"{path}: \t");
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
                Console.WriteLine();
            }
        }
        private void FilterTable(string attributeName, string attributeValue)
        {
            using (StreamReader sr = new StreamReader(Path))
            {
                string[] attributes = { "country", "continent", "capital", "territory", "popelation" };
                int index = -1;
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributeName == attributes[i])
                        index = i;
                }

                var line = sr.ReadLine();

                using (StreamWriter sw = new StreamWriter("tables/temp.txt", false))
                {
                    line = sr.ReadLine();
                    Console.WriteLine($"Фильтруем таблицу на подтаблицу по атрибуту: {attributeValue}");

                    while (line != null)
                    {
                        attributes = line.Trim().Split(';');
                        if (attributes[index] == attributeValue)
                        {
                            Thread.Sleep(delay);
                            sw.WriteLine(line);
                            Console.WriteLine($"{line}, подходит - записываем в подтаблицу");
                        }
                        line = sr.ReadLine();
                    }
                }
            }
            CopyFileData("tables/temp.txt");
        }
        private void CopyFileData(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            using (StreamWriter sw = new StreamWriter(Path, false))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
        }

        private void SplitFile(int elementsToWrite)
        {
            using (StreamReader sr = new StreamReader(Path))
            {
                var line = sr.ReadLine();
                using (StreamWriter swA = new StreamWriter(pathA, false))
                using (StreamWriter swB = new StreamWriter(pathB, false))
                {
                    int count = 0;

                    long linesCount = GetLinesCount(Path);

                    for (int i = 0; i < linesCount / elementsToWrite * 2; i++)
                    {
                        while (count < elementsToWrite)
                        {
                            if (line == null) return;

                            Thread.Sleep(delay);
                            Console.WriteLine($"Записываем строку: {line} в файл A");
                            swA.WriteLine(line);
                            count++;
                            line = sr.ReadLine();
                        }
                        while (count > 0)
                        {
                            if (line == null) return;

                            Thread.Sleep(delay);
                            Console.WriteLine($"Записываем строку: {line} в файл B");
                            swB.WriteLine(line);
                            count--;
                            line = sr.ReadLine();
                        }
                    }
                }
            }
        }

        private void CompareAndWrite(int iterations)
        {
            using (StreamWriter sw = new StreamWriter(Path, false))
            using (StreamReader srA = new StreamReader(pathA))
            using (StreamReader srB = new StreamReader(pathB))
            {
                var lineA = srA.ReadLine();
                var lineB = srB.ReadLine();

                while (true)
                {
                    for (int i = 0; i < iterations * 2; i++)
                    {
                        if (lineA == null && lineB != null)
                        {
                            while (lineB != null)
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine("Элементы в А закончились, дописываем остатки В");
                                sw.WriteLine(lineB);
                                lineB = srB.ReadLine();
                            }
                            sw.Flush();
                            return;
                        }
                        else if (lineA != null && lineB == null)
                        {
                            while (lineA != null)
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine("Элементы в В закончились, дописываем остатки А");
                                sw.WriteLine(lineA);
                                lineA = srA.ReadLine();
                            }
                            sw.Flush();
                            return;
                        }

                        var attrA = lineA.Trim().Split(';');
                        long x = Int64.Parse(FormatInputNum(attrA[keyIndex]));

                        var attrB = lineB.Trim().Split(';');
                        long y = Int64.Parse(FormatInputNum(attrB[keyIndex]));

                        Thread.Sleep(delay);
                        Console.Write($"\nСравниваем {x} и {y}. ");
                        if (x < y)
                        {
                            Console.WriteLine($"Записываем {x}\n");
                            sw.WriteLine(lineA);
                            lineA = srA.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Записываем {y}\n");
                            sw.WriteLine(lineB);
                            lineB = srB.ReadLine();
                        }
                    }
                }
            };
        }

        private string FormatInputNum(string num)
        {
            return num.Replace(" ", "");
        }

        private string GetAttributeNames()
        {
            using (StreamReader sr = new StreamReader(@"C:\Users\Dell\Desktop\header.txt"))
            {
                return sr.ReadLine();
            }
        }
        private long GetKeyAttribute(string line, int attributeIndex)
        {
            var attributes = line.Trim().Split(';');
            try
            {
                long res;
                res = Int64.Parse(attributes[attributeIndex]);
                return res;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("не получилось привести ключевой атрибут из строки таблицы к числу");
            }

            return -1L;
        }
        private long GetLinesCount(string path)
        {
            var linesCount = 1;
            int nextLine = '\n';
            using (var streamReader = new StreamReader(
                new BufferedStream(
                    File.OpenRead(path), 10 * 1024 * 1024))) // буфер в 10 мегабайт
            {
                while (!streamReader.EndOfStream)
                {
                    if (streamReader.Read() == nextLine) linesCount++;
                }
            }

            return linesCount;
        }
    }
}
