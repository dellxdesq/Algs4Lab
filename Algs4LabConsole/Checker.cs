using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg4Lab
{
    internal class Checker
    {
        public static List<double> ResultTime = new List<double>();
        public static List<double> Time = new List<double>();
        public static void GetAverageTime()
        {
            double count = Double.MaxValue;

            foreach (var item in Time)
            {
                if (count > item)
                {
                    count = item;
                }

            }

            Time.Clear();
            ResultTime.Add(count);

        }
        public static void CountWords(List<string> words)
        {

            IEnumerable<string> enumerable = words as IEnumerable<string>;
            var stringGroups = enumerable.GroupBy(s => s);
            foreach (var stringGroup in stringGroups)
                Console.WriteLine("{0}  {1}", stringGroup.Key, stringGroup.Count());


        }

        public static string WorkingFile(int n)
        {

            string text;


            using (StreamReader sr = new StreamReader($"{n}text.txt"))
            {
                text = sr.ReadLine();
            }

            return text;
        }
        public static string Alphabe = "abcdefghijklmnopqrstuvwxyz";
        //получаем сам список слов
        public static List<string> GetWordsList(int n)
        {
            string line = WorkingFile(n);
            string[] data = line.Split(" ");

            List<string> list = new List<string>();

            foreach (string word in data)
            {
                string result = "";

                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        if (Char.ToLower(word[i]) == Alphabe[j])
                        {
                            result = result + word[i];
                        }
                        continue;

                    }
                }

                list.Add(result);

            }
            return list;

        }
    }
}
