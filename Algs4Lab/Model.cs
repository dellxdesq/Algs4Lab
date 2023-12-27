using Algs4Lab;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Algs4Lab
{
    public class Model : INotifyPropertyChanged
    {
        public static MainWindow? _window;

        public ICommand SortButton { get; set; }
        public ICommand DescriptionsButton { get; set; }
        public ICommand ArrayButton { get; set; }

        public Model(MainWindow window)
        {
            _window = window;
            ArrayButton = new RelayCommand(Array);
            SortButton = new RelayCommand(Sort);

        }

        public static void Array(object? o)
        {
            Generator arrayCreator = new(_window);
        }

        public static async void Sort(object? o)
        {
            Anims animation = new();
            if (_window.sortComboBox.SelectedItem != null)
            {
                string selectedSort = ((ComboBoxItem)_window.sortComboBox.SelectedItem).Content.ToString();

                switch (selectedSort)
                {
                    case "Bubble Sort":
                        await animation.BubbleSortAnimationAsync();
                        break;
                    case "Select Sort":
                        await animation.SelectionSortAnimationAsync();
                        break;
                    case "Heap Sort":
                        await animation.HeapSortAnimationAsync();
                        break;
                    case "Merge Sort":
                        await animation.MergeSortAnimationAsync(0, Generator.ArraySize - 1);
                        break;
                    case "Select a sort:":
                        MessageBox.Show("Выберите тип сортировки.");
                        break;
                    default:
                        MessageBox.Show("Выберите тип сортировки.");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Выберите тип сортировки.");
            }
        }

        public event PropertyChangedEventHandler ?PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
