using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;

namespace DearJacob
{
    public static class PrintingMassives
    {
        private static void ColorizeMaxElement(RichTextBox richBox, float[,] arr)
        {
            FindMaxIndexs(arr, out int iMax, out int jMax);
            string maxElement = arr[iMax, jMax].ToString();

            Paragraph paragraph = richBox.Document.Blocks.FirstBlock as Paragraph;
            TextRange tRange = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
            tRange.ClearAllProperties();
            List<TextRange> selections = new List<TextRange>();
            string text = tRange.Text;
            int index = text.IndexOf(maxElement, StringComparison.InvariantCultureIgnoreCase);

            while (index ++>= 0)
            {
                var textPointerStart = paragraph.ContentStart.GetPositionAtOffset(index);
                var textPointerEnd = paragraph.ContentStart.GetPositionAtOffset(index + maxElement.Length);
                selections.Add(new TextRange(textPointerStart, textPointerEnd));

                if (index + maxElement.Length >= text.Length)
                {
                    break;
                }
                index = text.IndexOf(maxElement, index + maxElement.Length, StringComparison.CurrentCultureIgnoreCase);
            }

            foreach (var selection in selections)
            {
                selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.DarkViolet);
            }
        }

        public static void FindMaxIndexs(float[,] arr, out int iMax, out int jMax)
        {            
            iMax = 0;
            jMax = 1;

            for (int i = 0; i < arr.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (Math.Abs(arr[iMax, jMax]) < Math.Abs(arr[i, j]) && i != j)
                    {
                        iMax = i;
                        jMax = j;
                    }
                }
            }
        }

        public static void GiveMeMatrix(this RichTextBox richTextBox, out float[,] arr)
        {
            TextRange tRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            int height = tRange.Text.Split('\n').Where(i => i.Length > 0).Count();
            int width = 0;

            string[] matr = new string[height];
            matr = tRange.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < height; i++)
            {
                width += matr[i].Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
            }

            width /= matr[0].Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();

            if (height != width)
            {
                throw new Exception("Матрица не квадратная.");
            }

            string[] buffer = new string[width];
            arr = new float[height, width];

            for (int i = 0; i < height; i++)
            {
                buffer = matr[i].Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < width; j++)
                {
                    arr[i, j] = float.Parse(buffer[j]) == 0 ? arr[j, i] : float.Parse(buffer[j]);
                }
            }
        }

        public static void GiveMeMatrix(this RichTextBox richTextBox, out float[] arr)
        {
            TextRange tRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            int width = tRange.Text.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();

            arr = new float[width];

            string[] buffer = tRange.Text.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < width; i++)
            {
                arr[i] = float.Parse(buffer[i]);
            }
        }

        public static void PrintMassiveE(this RichTextBox richTextBox, float[,] arr, bool paintingMaxEl = false)
        {
            richTextBox.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (j != arr.GetLength(1) - 1)
                    {
                        if (arr[i,j] != 0)
                        {
                            paragraph.Inlines.Add(string.Format("{0:0.###}\t", arr[i, j]));
                        }
                        else
                        {
                            paragraph.Inlines.Add(string.Format("{0:0.###}\t", arr[j, i]));
                        }
                    }
                    else
                    {
                        paragraph.Inlines.Add(string.Format("{0:0.###}", arr[i, j]));
                    }
                }

                if (i != arr.GetLength(0) - 1)
                {
                    paragraph.Inlines.Add(Environment.NewLine);
                }
            }

            richTextBox.Document.Blocks.Add(paragraph);

            if (paintingMaxEl)
                ColorizeMaxElement(richTextBox, arr);
        }

        public static void PrintMassiveK(this RichTextBox richTexBox, float[] arr)
        {
            richTexBox.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                if (i != arr.GetLength(0) - 1)
                {
                    paragraph.Inlines.Add(arr[i].ToString() + "\t");
                }
                else
                {
                    paragraph.Inlines.Add(arr[i].ToString());
                }
            }

            richTexBox.Document.Blocks.Add(paragraph);
        }

        public static void GiveMeMatrixFromTextBoxes(ref Matrix matrix, RichTextBox richE, RichTextBox richK)
        {
            richE.GiveMeMatrix(out float[,] arrE);
            richK.GiveMeMatrix(out float[] arrK);

            matrix.ArrE = arrE;
            matrix.ArrK = arrK;
        }
    }
}
