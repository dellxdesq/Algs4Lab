
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg4Lab
{
    class PyramidSort
    {

        static int AddPyramids(List<string> arr, int i, int N)
        {
            int imax;
            string buf;
            if ((2 * i + 2) < N)
            {
                if (Algorithms.GetBool(arr[2 * i + 2], arr[2 * i + 1]))
                    imax = 2 * i + 2;
                else imax = 2 * i + 1;
            }
            else imax = 2 * i + 1;
            if (imax >= N) return i;
            if (Algorithms.GetBool(arr[imax], arr[i]))
            {
                buf = arr[i];
                arr[i] = arr[imax];
                arr[imax] = buf;
                if (imax < N / 2) i = imax;
            }
            return i;
        }
        public static void Sorting(List<string> arr)
        {
            var watch = Stopwatch.StartNew();

            int len = arr.Count;
            for (int i = len / 2 - 1; i >= 0; --i)
            {
                long prev_i = i;
                i = AddPyramids(arr, i, len);
                if (prev_i != i) ++i;
            }


            string buf;
            for (int k = len - 1; k > 0; --k)
            {
                buf = arr[0];
                arr[0] = arr[k];
                arr[k] = buf;
                int i = 0, prev_i = -1;
                while (i != prev_i)
                {
                    prev_i = i;
                    i = AddPyramids(arr, i, k);
                }
            }
            Checker.CountWords(arr);
            watch.Stop();
            var elapsedTime = watch.Elapsed.TotalMilliseconds;
            Checker.Time.Add(elapsedTime * 1000);
        }

    }
}
