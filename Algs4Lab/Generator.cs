using Algs4Lab;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Algs4Lab
{
    class Generator
    {
        private MainWindow _window;
        private Canvas _canvas;
        private List<Rectangle> _originalRectangles = new();


        public static List<Rectangle> Rectangles = new();
        public static readonly int ArraySize = 15;

        public Generator(MainWindow window)
        {
            _window = window;
            _canvas = _window.canvas;
            Logs clear = new();
            ClearArray();
            InitializeArray();
        }

        public void ClearArray()
        {
            Rectangles.Clear();
            _originalRectangles.Clear();
            _canvas.Children.Clear();
        }

        private static Rectangle CloneRectangle(Rectangle source)
        {
            Rectangle clone = new Rectangle
            {
                Width = source.Width,
                Height = source.Height,
                Fill = source.Fill,
                Margin = source.Margin
            };

            return clone;
        }

        private void InitializeArray()
        {
            for (int i = 0; i < ArraySize; i++)
            {
                Random random = new();
                int height = random.Next(80, 350);
                Rectangle rect = new Rectangle
                {
                    Width = 30,
                    Height = height,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#755D9A")),
                    Margin = new Thickness(5, 0, 5, 0)
                };

                Rectangles.Add(rect);
                _originalRectangles.Add(CloneRectangle(rect));
                _window.canvas.Children.Add(rect);
                Canvas.SetBottom(rect, 0);
                Canvas.SetLeft(rect, i * 40);
            }
        }
    }
}
