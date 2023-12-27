using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Algs4Lab
{
    class Anims
    {
        public async Task BubbleSortAnimationAsync()
        {
            var highlightColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Red
            var defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#755D9A")); // Blue
            var sortedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57")); // Green

            Logs clear = new();

            for (int i = 0; i < Generator.ArraySize - 1; i++)
            {
                for (int j = 0; j < Generator.ArraySize - i - 1; j++)
                {
                    Generator.Rectangles[j].Fill = highlightColor;
                    Generator.Rectangles[j + 1].Fill = highlightColor;

                    if (GetHeight(Generator.Rectangles[j]) > GetHeight(Generator.Rectangles[j + 1]))
                    {
                        await SwapAsync(j, j + 1);
                        Logs l = new(j, j + 1);
                    }

                    Generator.Rectangles[j].Fill = defaultColor;
                    Generator.Rectangles[j + 1].Fill = defaultColor;
                }

                Generator.Rectangles[Generator.ArraySize - i - 1].Fill = sortedColor;
            }

            Generator.Rectangles[0].Fill = sortedColor;
        }

        public async Task MergeSortAnimationAsync(int low, int high)
        {
            if (low < high)
            {
                int mid = (low + high) / 2;

                await MergeSortAnimationAsync(low, mid);
                await MergeSortAnimationAsync(mid + 1, high);

                await MergeAsync(low, mid, high);
            }
        }

        private async Task MergeAsync(int low, int mid, int high)
        {
            int n1 = mid - low + 1;
            int n2 = high - mid;

            double[] leftArray = new double[n1];
            double[] rightArray = new double[n2];
            int[] leftIndices = new int[n1];
            int[] rightIndices = new int[n2];

            var highlightColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Красный
            var defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#755D9A")); // Синий
            var sortedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57")); // Зеленый

            for (int i = 0; i < n1; ++i)
            {
                leftArray[i] = GetHeight(Generator.Rectangles[low + i]);
                leftIndices[i] = low + i;
                Generator.Rectangles[low + i].Fill = highlightColor;
            }

            for (int j = 0; j < n2; ++j)
            {
                rightArray[j] = GetHeight(Generator.Rectangles[mid + 1 + j]);
                rightIndices[j] = mid + 1 + j;
                Generator.Rectangles[mid + 1 + j].Fill = highlightColor;
            }

            int k = low;
            int indexLeft = 0, indexRight = 0;

            while (indexLeft < n1 && indexRight < n2)
            {
                if (leftArray[indexLeft] <= rightArray[indexRight])
                {
                    SetHeight(Generator.Rectangles[k], leftArray[indexLeft]);
                    Logs l = new(leftIndices[indexLeft], k);

                    await Task.Delay(GetAnimationDelay());

                    Generator.Rectangles[k].Fill = defaultColor;
                    indexLeft++;
                }
                else
                {
                    SetHeight(Generator.Rectangles[k], rightArray[indexRight]);
                    Logs l = new(rightIndices[indexRight], k);

                    await Task.Delay(GetAnimationDelay());

                    Generator.Rectangles[k].Fill = defaultColor;
                    indexRight++;
                }

                k++;
            }

            while (indexLeft < n1)
            {
                SetHeight(Generator.Rectangles[k], leftArray[indexLeft]);
                Logs l = new(leftIndices[indexLeft], k);

                await Task.Delay(GetAnimationDelay());

                Generator.Rectangles[k].Fill = defaultColor;
                indexLeft++;
                k++;
            }

            while (indexRight < n2)
            {
                SetHeight(Generator.Rectangles[k], rightArray[indexRight]);
                Logs l = new(rightIndices[indexRight], k);

                await Task.Delay(GetAnimationDelay());

                Generator.Rectangles[k].Fill = defaultColor;
                indexRight++;
                k++;
            }

            for (int x = low; x <= high; x++)
            {
                Generator.Rectangles[x].Fill = sortedColor;
            }
        }

        private int GetAnimationDelay()
        {
            return (int)(Model._window.speedSlider.Maximum - Model._window.speedSlider.Value) * 10;
        }
        public async Task SelectionSortAnimationAsync()
        {
            Logs clear = new();
            int n = Generator.ArraySize;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                Generator.Rectangles[minIndex].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Red

                for (int j = i + 1; j < n; j++)
                {
                    Generator.Rectangles[j].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Red

                    if (GetHeight(Generator.Rectangles[j]) < GetHeight(Generator.Rectangles[minIndex]))
                    {
                        Generator.Rectangles[minIndex].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#755D9A")); // Blue
                        minIndex = j;
                        Generator.Rectangles[minIndex].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Red
                    }

                    await Task.Delay(50); // Задержка по необходимости
                }

                await SwapAsync(i, minIndex);
                Logs l = new(i, minIndex);

                Generator.Rectangles[i].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57")); // Green
            }

            Generator.Rectangles[n - 1].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57")); // Green
        }

        public async Task HeapSortAnimationAsync()
        {
            Logs clear = new();
            int n = Generator.ArraySize;

            var highlightColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB841")); // Красный
            var defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#755D9A")); // Синий
            var sortedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57")); // Зеленый

            // Построение кучи (Heapify)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                await HeapifyAsync(n, i, highlightColor, defaultColor);
            }

            // Извлечение элементов из кучи
            for (int i = n - 1; i > 0; i--)
            {
                await SwapAsync(0, i);
                Logs l = new(0, i);

                Generator.Rectangles[i].Fill = sortedColor;

                await HeapifyAsync(i, 0, highlightColor, defaultColor);
            }

            Generator.Rectangles[0].Fill = sortedColor;
        }

        private async Task HeapifyAsync(int n, int i, SolidColorBrush highlightColor, SolidColorBrush defaultColor)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && GetHeight(Generator.Rectangles[left]) > GetHeight(Generator.Rectangles[largest]))
                largest = left;

            if (right < n && GetHeight(Generator.Rectangles[right]) > GetHeight(Generator.Rectangles[largest]))
                largest = right;

            if (largest != i)
            {
                Generator.Rectangles[i].Fill = highlightColor;
                Generator.Rectangles[largest].Fill = highlightColor;

                await SwapAsync(i, largest);
                Logs l = new(i, largest);

                Generator.Rectangles[i].Fill = defaultColor;
                Generator.Rectangles[largest].Fill = defaultColor;

                await HeapifyAsync(n, largest, highlightColor, defaultColor);
            }
        }

        private async Task SwapAsync(int index1, int index2)
        {
            double tempHeight = GetHeight(Generator.Rectangles[index1]);
            SetHeight(Generator.Rectangles[index1], GetHeight(Generator.Rectangles[index2]));
            SetHeight(Generator.Rectangles[index2], tempHeight);

            int delayMilliseconds = (int)(Model._window.speedSlider.Maximum - Model._window.speedSlider.Value) * 10;
            await Task.Delay(delayMilliseconds);

            Canvas.SetLeft(Generator.Rectangles[index1], index2 * 40);
            Canvas.SetLeft(Generator.Rectangles[index2], index1 * 40);

            (Generator.Rectangles[index2], Generator.Rectangles[index1]) = (Generator.Rectangles[index1], Generator.Rectangles[index2]);
            (Generator.Rectangles[index2].Height, Generator.Rectangles[index1].Height) = (Generator.Rectangles[index1].Height, Generator.Rectangles[index2].Height);
        }

        private double GetHeight(Rectangle rectangle)
        {
            return rectangle.Height;
        }

        private void SetHeight(Rectangle rectangle, double height)
        {
            rectangle.Height = height;
        }
    }
}
