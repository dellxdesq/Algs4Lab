using System;
using System.IO;
using System.Threading;

namespace Alg4Lab
{
    public class NaturalSort
    {
        private string AttributeNames;
        private string file { get; set; }
        private long segments;
        private int delay;
        private int indexKey = 0;

        public NaturalSort(string file, int indexKey, int delay, string filterAttr, string filterAttrValue)
        {
            AttributeNames = GetAttributeNames();
            this.file = file;
            this.indexKey = indexKey;
            this.delay = delay;

            FilterTable(filterAttr, filterAttrValue);
        }

        public NaturalSort(string file, int indexKey, int delay)
        {
            this.file = file;
            this.indexKey = indexKey;
            this.delay = delay;
        }

        public NaturalSort(string file, int delay)
        {
            this.file = file;
            this.delay = delay;
        }

        private static bool needToReOrder(string s1, string s2)
        {
            for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
            {
                if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return false;
                if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return true;
            }
            return false;
        }

        public void Sort()
        {
            while (true)
            {
                SplitToFilesInt();
                if (segments == 1) break;
                MergeIntPairs();
            }
            Console.WriteLine("Сортировка завершена.");
        }

        public void SortForString()
        {
            while (true)
            {
                SplitToFilesString();
                if (segments == 1) break;
                MergeStringPairs();
            }
            Console.WriteLine("Сортировка законченна.");
        }

        private void SplitToFilesInt()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writerA = new StreamWriter("a.txt"))
            using (StreamWriter writerB = new StreamWriter("b.txt"))
            {
                bool flag = true;
                string previous = sr.ReadLine();
                writerA.WriteLine(previous);
                Console.WriteLine($"Считываем элемент {previous} с файла \"{file}\" и записываем в файл a.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(delay);
                    string current = sr.ReadLine();
                    if (Convert.ToInt64(FormatInputNum(previous.Split(";")[indexKey])) > Convert.ToInt64(FormatInputNum(current.Split(";")[indexKey])))
                    {
                        flag = !flag;
                        segments++;
                        if (flag == true) writerB.WriteLine("\'");
                        else writerA.WriteLine("\'");
                    }

                    if (flag)
                    {
                        writerA.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл a.txt.");
                    }
                    else
                    {
                        writerB.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл b.txt.");
                    }
                    previous = current;
                }
            }
            Console.WriteLine();
        }

        private void SplitToFilesString()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writerA = new StreamWriter("a.txt"))
            using (StreamWriter writerB = new StreamWriter("b.txt"))
            {
                bool flag = true;
                string prev = sr.ReadLine();
                writerA.WriteLine(prev);
                Console.WriteLine($"Считываем элемент {prev} с файла \"{file}\" и записываем в файл a.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(delay);
                    string cur = sr.ReadLine();
                    if (needToReOrder(prev, cur))
                    {
                        flag = !flag;
                        segments++;
                        if (flag == true) writerB.WriteLine("\'");
                        else writerA.WriteLine("\'");
                    }

                    if (flag)
                    {
                        writerA.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл a.txt.");
                    }
                    else
                    {
                        writerB.WriteLine(cur);
                        Console.WriteLine($"Считываем элемент {cur} с файла \"{file}\" и записываем в файл b.txt.");
                    }
                    prev = cur;
                }
            }
            Console.WriteLine();
        }

        private void MergeIntPairs()
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                string a = null, b = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
                {
                    if (pickedA == false && pickedB == false)
                    {
                        a = "";
                        b = "";
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (a != "\'" && !pickedA)
                        {
                            Thread.Sleep(delay);
                            a = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {a} с файла \"a.txt\".");
                            pickedA = true;
                        }
                        if (a == "\'") pickedA = false;
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (b != "\'" && !pickedB)
                        {
                            Thread.Sleep(delay);
                            b = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {b} с файла \"b.txt\".");
                            pickedB = true;
                        }
                        if (b == "\'") pickedB = false;
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            if (Convert.ToInt64(FormatInputNum(a.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(b.Split(";")[indexKey])))
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {a} из файла \"a.txt\" в файл \"{file}\".");
                                sr.WriteLine(a);
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {b} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(b);
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {a} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(a);
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(delay);
                        Console.WriteLine($"Добовляем {b} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(b);
                        pickedB = false;
                    }
                }
                Thread.Sleep(delay);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void MergeStringPairs()
        {
            using (StreamReader readerA = new StreamReader("a.txt"))
            using (StreamReader readerB = new StreamReader("b.txt"))
            using (StreamWriter sr = new StreamWriter(file))
            {
                string elementA = null, elementB = null;
                bool pickedA = false, pickedB = false;
                while (!readerA.EndOfStream || !readerB.EndOfStream || pickedA || pickedB)
                {
                    if (pickedA == false && pickedB == false)
                    {
                        elementA = "";
                        elementB = "";
                    }

                    if (!readerA.EndOfStream)
                    {
                        if (elementA != "\'" && !pickedA)
                        {
                            Thread.Sleep(delay);
                            elementA = readerA.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementA} с файла \"a.txt\".");
                            pickedA = true;
                        }
                        if (elementA == "\'") pickedA = false;
                    }

                    if (!readerB.EndOfStream)
                    {
                        if (elementB != "\'" && !pickedB)
                        {
                            Thread.Sleep(delay);
                            elementB = readerB.ReadLine();
                            Console.WriteLine($"Считываем элемент {elementB} с файла \"b.txt\".");
                            pickedB = true;
                        }
                        if (elementB == "\'") pickedB = false;
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            if (needToReOrder(elementA, elementB) == false)
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementA);
                                pickedA = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                                sr.WriteLine(elementB);
                                pickedB = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {elementA} из файла \"a.txt\" в файл \"{file}\".");
                            sr.WriteLine(elementA);
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        Thread.Sleep(delay);
                        Console.WriteLine($"Добовляем {elementB} из файла \"b.txt\" в файл \"{file}\".");
                        sr.WriteLine(elementB);
                        pickedB = false;
                    }
                }
                Thread.Sleep(delay);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private string FormatInputNum(string num)
        {
            return num.Replace(" ", "");
        }
        private void FilterTable(string attributeName, string attributeValue)
        {
            using (StreamReader sr = new StreamReader(this.file))
            {
                string[] attributes = { "country", "continent", "capital", "territory", "population" };
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
            using (StreamWriter sw = new StreamWriter(this.file, false))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
        }
        private string GetAttributeNames()
        {
            using (StreamReader sr = new StreamReader("tables/header.txt"))
            {
                return sr.ReadLine();
            }
        }
    }
}
