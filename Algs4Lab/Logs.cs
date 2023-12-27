using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algs4Lab
{
    public class Logs
    {
        private StringBuilder permutationsLog = new();

        public Logs(int index1, int index2)
        {
            LogPermutation(index1, index2);
            UpdatePermutationsText();
        }

        public Logs()
        {
            ClearLog();
        }

        public void LogPermutation(int index1, int index2)
        {
            permutationsLog.AppendLine($"Элемент {index1} со значением: {Generator.Rectangles[index1].Height} \nменяется на элемент {index2} со значением: {Generator.Rectangles[index2].Height}");
        }

        public void UpdatePermutationsText()
        {
            Model._window.permutationsTextBox.Text += permutationsLog.ToString();
        }

        public void ClearLog()
        {
            permutationsLog.Clear();
            Model._window.permutationsTextBox.Text = string.Empty;
        }
    }
}
