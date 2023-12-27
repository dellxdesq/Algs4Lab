using Alg4Lab;

using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace alglab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите номер задания:");
            string start = Console.ReadLine();
            switch (start)
            {
                case "1":
                    Run2Task();
                    break;
                case "2":
                    Run3Task();
                    break;
            }


        }
        
        static void Run2Task()
        {
            Console.WriteLine("Выберите желаемый метод сортировки (1 - прямой, 2 - естественный, 3 - многопутевой.)");
            string sortOption = Console.ReadLine();

            Console.WriteLine($"Возможные атрибуты: {DataWorker.GetAttributesFromFile("tables/header.txt")} ");
            Console.WriteLine("Введите индекс ключевого атрибута (он должен быть числовым): ");
            int keyIndex = Int32.Parse(Console.ReadLine().Trim());
            Console.WriteLine("Введите название атрибута, по которому отфильтровать таблицу: ");
            string attributeName = Console.ReadLine().Trim();
            Console.WriteLine("Введите значение фильтрующего атрибута: ");
            string attributeValue = Console.ReadLine().Trim();
            Console.WriteLine("Введите время задержки вывода (в милисекундах): ");
            int delay = Int32.Parse(Console.ReadLine());

            string text = "Mongolia;asia;Ulaanbaatar;1 564 116;2 736 800\nJapan;asia;Tokyo;377 97;126 435 321\nKazakhstan;asia;Nur-Sultan;2 724 900;16 674 960\nSaudi Arabia;asia;Riyadh;2 218 000;28 686 630\nIndonesia;asia;Jakarta;1 919 440;237 641 330\nIran;asia;Tehran;1 648 000;74 000 000\nPakistan;asia;Islamabad;803 940;190 291 130\nGermany;europe;Berlin;357 168;82 800 000\nFrance;europe;Paris;547 030; 67 348 000\nGreat Britain;europe;London;244 820;66 040 229\nItaly;europe;Rome;301 338;60 589 445\nUkraine;europe;Kiev;603 628;42 418 235\nPoland;europe;Warsaw;312 685;38 422 346\nNetherlands;europe;Amsterdam;41 543;17 271 990\nCzech Republic;europe;Prague;78 866;10 610 947\nArgentina;south america;Buenos Aires;2 766 890;46 226 168\nBolivia;south america;Sucre;1 098 580;12 069 813\nColombia;south america;Bogotá;1 138 910;52 698 313\nPeru;south america;Lima;1 285 220;34 333 082\nChile;south america;Santiago;1 285 220;19 724 059\nVenezuela;south america;Caracas;912 050;27 297 542\nUruguay;south america;Montevideo;176 220;3 504 084\nEcuador;south america;Quito;214 534;18 386 773\nChina;asia;Beijing;9 598 077;1 350 120 000\nIndia;asia;New Delhi;3 287 590;1 224 245 000";
            using (StreamWriter sw = new StreamWriter("tables/countries.txt", false))
                sw.Write(text);



            switch (sortOption)
            {
                case "1":
                    TableSorts ts = new TableSorts("tables/countries.txt", keyIndex, delay, attributeName, attributeValue);
                    ts.DirectSort();
                    break;
                case "2":
                    NaturalSort ns = new NaturalSort("tables/countries.txt", keyIndex, delay, attributeName, attributeValue);
                    ns.Sort();
                    break;
                case "3":
                    MultipathMergeSort ms = new MultipathMergeSort("tables/countries.txt", keyIndex, delay, attributeName, attributeValue);
                    ms.Sort();
                    break;
                default:
                    Console.WriteLine("Введен некорректный способ сортировки");
                    break;
            }
        }
        static void Run3Task()
        {
            List<string> list = new List<string>();
            list.Add("a");
            list.Add("aaafc");
            list.Add("aDsba");
            list.Add("ca");
            list.Add("aaafd");
            list.Add("c");

            Algorithms.BubbleSort(list);
            Console.WriteLine("------");
            PyramidSort.Sorting(list);

            int n = 1;
            while (n <= 11)
            {
                for (int i = 0; i < 5; i++)
                {
                    Algorithms.BubbleSort(Checker.GetWordsList(n));
                    Algorithms.InsertionSort(Checker.GetWordsList(n));
                    PyramidSort.Sorting(Checker.GetWordsList(n));
                }
                Checker.GetAverageTime();
                n++;

            }
            foreach (var item in Checker.ResultTime)
            {
                Console.WriteLine(item);
            }
            Checker.ResultTime.Clear();
        }
    }
}
