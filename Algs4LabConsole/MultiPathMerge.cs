using System;
using System.IO;
using System.Threading;

namespace Alg4Lab
{
    public class MultipathMergeSort
    {
        private string AttributeNames;
        private string file { get; set; }
        private long segments;
        private int delay;
        private int indexKey = 0;

        public MultipathMergeSort(string file, int indexKey, int delay, string filterAttr, string filterAttrValue)
        {
            AttributeNames = GetAttributeNames();
            this.file = file;
            this.indexKey = indexKey;
            this.delay = delay;

            FilterTable(filterAttr, filterAttrValue);
        }

        public MultipathMergeSort(string file, int indexKey, int delay)
        {
            this.file = file;
            this.indexKey = indexKey;
            this.delay = delay;
        }

        public MultipathMergeSort(string file, int delay)
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
            SplitToFilesInt();
            if (segments == 1)
            {
                Console.WriteLine("Сортировка завершена.");
                return;
            }
            while (true)
            {
                MergeIntPairs("1", "2", "3", "4", "5", "6");
                if (segments == 1) break;
                MergeIntPairs("4", "5", "6", "1", "2", "3");
            }
            Console.WriteLine("Сортировка завершена.");
        }

        public void SortForString()
        {
            SplitToFilesString();
            if (segments == 1)
            {
                Console.WriteLine("Сортировка завершена.");
                return;
            }
            while (true)
            {
                MergeStringPairs("1", "2", "3", "4", "5", "6");
                if (segments == 1) break;
                MergeStringPairs("4", "5", "6", "1", "2", "3");
            }
            Console.WriteLine("Сортировка завершена.");
        }

        private void SplitToFilesInt()
        {
            segments = 1;
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter writer1 = new StreamWriter("1.txt"))
            using (StreamWriter writer2 = new StreamWriter("2.txt"))
            using (StreamWriter writer3 = new StreamWriter("3.txt"))
            {
                string previous = sr.ReadLine();
                writer1.WriteLine(previous);
                Console.WriteLine($"Считываем элемент {previous} с файла \"{file}\" и записываем в файл 1.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(delay);
                    string current = sr.ReadLine();
                    if (Convert.ToInt64(FormatInputNum(previous.Split(";")[indexKey])) > Convert.ToInt64(FormatInputNum(current.Split(";")[indexKey])))
                    {
                        segments++;
                        if (segments % 3 == 0) writer2.WriteLine("\'");
                        else if (segments % 3 == 1) writer3.WriteLine("\'");
                        else writer1.WriteLine("\'");
                    }

                    if (segments % 3 == 1)
                    {
                        writer1.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 1.txt.");
                    }
                    else if (segments % 3 == 2)
                    {
                        writer2.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 2.txt.");
                    }
                    else
                    {
                        writer3.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 3.txt.");
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
            using (StreamWriter writer1 = new StreamWriter("1.txt"))
            using (StreamWriter writer2 = new StreamWriter("2.txt"))
            using (StreamWriter writer3 = new StreamWriter("3.txt"))
            {
                string previous = sr.ReadLine();
                writer1.WriteLine(previous);
                Console.WriteLine($"Считываем элемент {previous} с файла \"{file}\" и записываем в файл 1.txt.");
                while (!sr.EndOfStream)
                {
                    Thread.Sleep(delay);
                    string current = sr.ReadLine();
                    if (needToReOrder(previous, current))
                    {
                        segments++;
                        if (segments % 3 == 0) writer2.WriteLine("\'");
                        else if (segments % 3 == 1) writer3.WriteLine("\'");
                        else writer1.WriteLine("\'");
                    }

                    if (segments % 3 == 1)
                    {
                        writer1.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 1.txt.");
                    }
                    else if (segments % 3 == 2)
                    {
                        writer2.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 2.txt.");
                    }
                    else
                    {
                        writer3.WriteLine(current);
                        Console.WriteLine($"Считываем элемент {current} с файла \"{file}\" и записываем в файл 3.txt.");
                    }
                    previous = current;
                }
            }
            Console.WriteLine();
        }

        private void MergeIntPairs(string a, string b, string c, string d, string e, string f)
        {
            segments = 0;
            string curFile = d;
            using (StreamReader reader1 = new StreamReader($"{a}.txt"))
            using (StreamReader reader2 = new StreamReader($"{b}.txt"))
            using (StreamReader reader3 = new StreamReader($"{c}.txt"))
            using (StreamWriter writer1 = new StreamWriter($"{d}.txt"))
            using (StreamWriter writer2 = new StreamWriter($"{e}.txt"))
            using (StreamWriter writer3 = new StreamWriter($"{f}.txt"))
            {
                string element1 = null, element2 = null, element3 = null;
                bool picked1 = false, picked2 = false, picked3 = false;
                while (!reader1.EndOfStream || !reader2.EndOfStream || !reader2.EndOfStream || picked1 || picked2 || picked3)
                {
                    if (picked1 == false && picked2 == false && picked3 == false)
                    {
                        element1 = "";
                        element2 = "";
                        element3 = "";
                        if (segments > 1)
                        {
                            int count = WriteToFile(writer1, writer2, writer3, "\'");
                            if (count == 1) curFile = d;
                            if (count == 2) curFile = e;
                            else curFile = f;
                        }
                        segments++;
                    }

                    if (!reader1.EndOfStream)
                    {
                        if (element1 != "\'" && !picked1)
                        {
                            Thread.Sleep(delay);
                            element1 = reader1.ReadLine();
                            Console.WriteLine($"Считываем элемент {element1} с файла \"{a}.txt\".");
                            picked1 = true;
                        }
                        if (element1 == "\'") picked1 = false;
                    }

                    if (!reader2.EndOfStream)
                    {
                        if (element2 != "\'" && !picked2)
                        {
                            Thread.Sleep(delay);
                            element2 = reader2.ReadLine();
                            Console.WriteLine($"Считываем элемент {element2} с файла \"{b}.txt\".");
                            picked2 = true;
                        }
                        if (element2 == "\'") picked2 = false;
                    }

                    if (!reader3.EndOfStream)
                    {
                        if (element3 != "\'" && !picked3)
                        {
                            Thread.Sleep(delay);
                            element3 = reader3.ReadLine();
                            Console.WriteLine($"Считываем элемент {element3} с файла \"{c}.txt\".");
                            picked3 = true;
                        }
                        if (element3 == "\'") picked3 = false;
                    }

                    if (picked1)
                    {
                        if (picked2)
                        {
                            if (Convert.ToInt64(FormatInputNum(element1.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(element2.Split(";")[indexKey])))
                            {
                                if (picked3)
                                {
                                    if (Convert.ToInt64(FormatInputNum(element1.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(element3.Split(";")[indexKey])))
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element1);
                                        picked1 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(delay);
                                    Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element1);
                                    picked1 = false;
                                }
                            }
                            else
                            {
                                if (picked3)
                                {
                                    if (Convert.ToInt64(FormatInputNum(element2.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(element3.Split(";")[indexKey])))
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element2);
                                        picked2 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(delay);
                                    Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element2);
                                    picked2 = false;
                                }
                            }
                        }
                        else if (picked3)
                        {
                            if (Convert.ToInt64(FormatInputNum(element1.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(element3.Split(";")[indexKey])))
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element1);
                                picked1 = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element1);
                            picked1 = false;
                        }
                    }
                    else if (picked2)
                    {
                        if (picked3)
                        {
                            if (Convert.ToInt64(FormatInputNum(element2.Split(";")[indexKey])) < Convert.ToInt64(FormatInputNum(element3.Split(";")[indexKey])))
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element2);
                                picked2 = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element2);
                            picked2 = false;
                        }
                    }
                    else if (picked3)
                    {
                        Thread.Sleep(delay);
                        Console.WriteLine($"Добовляем {element3} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                        WriteToFile(writer1, writer2, writer3, element3);
                        picked3 = false;
                    }
                }
                Thread.Sleep(delay);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void MergeStringPairs(string a, string b, string c, string d, string e, string f)
        {
            segments = 0;
            string curFile = d;
            using (StreamReader reader1 = new StreamReader($"{a}.txt"))
            using (StreamReader reader2 = new StreamReader($"{b}.txt"))
            using (StreamReader reader3 = new StreamReader($"{c}.txt"))
            using (StreamWriter writer1 = new StreamWriter($"{d}.txt"))
            using (StreamWriter writer2 = new StreamWriter($"{e}.txt"))
            using (StreamWriter writer3 = new StreamWriter($"{f}.txt"))
            {
                string element1 = null, element2 = null, element3 = null;
                bool picked1 = false, picked2 = false, picked3 = false;
                while (!reader1.EndOfStream || !reader2.EndOfStream || !reader2.EndOfStream || picked1 || picked2 || picked3)
                {
                    if (picked1 == false && picked2 == false && picked3 == false)
                    {
                        element1 = "";
                        element2 = "";
                        element3 = "";
                        if (segments > 1)
                        {
                            int count = WriteToFile(writer1, writer2, writer3, "\'");
                            if (count == 1) curFile = d;
                            if (count == 2) curFile = e;
                            else curFile = f;
                        }
                        segments++;
                    }

                    if (!reader1.EndOfStream)
                    {
                        if (element1 != "\'" && !picked1)
                        {
                            Thread.Sleep(delay);
                            element1 = reader1.ReadLine();
                            Console.WriteLine($"Считываем элемент {element1} с файла \"{a}.txt\".");
                            picked1 = true;
                        }
                        if (element1 == "\'") picked1 = false;
                    }

                    if (!reader2.EndOfStream)
                    {
                        if (element2 != "\'" && !picked2)
                        {
                            Thread.Sleep(delay);
                            element2 = reader2.ReadLine();
                            Console.WriteLine($"Считываем элемент {element2} с файла \"{b}.txt\".");
                            picked2 = true;
                        }
                        if (element2 == "\'") picked2 = false;
                    }

                    if (!reader3.EndOfStream)
                    {
                        if (element3 != "\'" && !picked3)
                        {
                            Thread.Sleep(delay);
                            element3 = reader3.ReadLine();
                            Console.WriteLine($"Считываем элемент {element3} с файла \"{c}.txt\".");
                            picked3 = true;
                        }
                        if (element3 == "\'") picked3 = false;
                    }

                    if (picked1)
                    {
                        if (picked2)
                        {
                            if (needToReOrder(element1, element2) == false)
                            {
                                if (picked3)
                                {
                                    if (needToReOrder(element1, element3) == false)
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element1);
                                        picked1 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(delay);
                                    Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element1);
                                    picked1 = false;
                                }
                            }
                            else
                            {
                                if (picked3)
                                {
                                    if (needToReOrder(element2, element3) == false)
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element2);
                                        picked2 = false;
                                    }
                                    else
                                    {
                                        Thread.Sleep(delay);
                                        Console.WriteLine($"Добовляем {element3} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                        WriteToFile(writer1, writer2, writer3, element3);
                                        picked3 = false;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(delay);
                                    Console.WriteLine($"Добовляем {element2} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                    WriteToFile(writer1, writer2, writer3, element2);
                                    picked2 = false;
                                }
                            }
                        }
                        else if (picked3)
                        {
                            if (needToReOrder(element1, element3) == false)
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element1);
                                picked1 = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {element1} из файла \"{a}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element1);
                            picked1 = false;
                        }
                    }
                    else if (picked2)
                    {
                        if (picked3)
                        {
                            if (needToReOrder(element2, element3) == false)
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element2);
                                picked2 = false;
                            }
                            else
                            {
                                Thread.Sleep(delay);
                                Console.WriteLine($"Добовляем {element3} из файла \"{c}.txt\" в файл \"{curFile}.txt\".");
                                WriteToFile(writer1, writer2, writer3, element3);
                                picked3 = false;
                            }
                        }
                        else
                        {
                            Thread.Sleep(delay);
                            Console.WriteLine($"Добовляем {element2} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                            WriteToFile(writer1, writer2, writer3, element2);
                            picked2 = false;
                        }
                    }
                    else if (picked3)
                    {
                        Thread.Sleep(delay);
                        Console.WriteLine($"Добовляем {element3} из файла \"{b}.txt\" в файл \"{curFile}.txt\".");
                        WriteToFile(writer1, writer2, writer3, element3);
                        picked3 = false;
                    }
                }
                Thread.Sleep(delay);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private int WriteToFile(StreamWriter writer1, StreamWriter writer2, StreamWriter writer3, string element)
        {
            if (segments % 3 == 1)
            {
                writer1.WriteLine(element);
                return 1;
            }
            else if (segments % 3 == 2)
            {
                writer2.WriteLine(element);
                return 2;
            }
            else
            {
                writer3.WriteLine(element);
                return 3;
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
