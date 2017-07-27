using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;

namespace DearJacob
{
    static class WorkWithFiles
    {
        public static Matrix OpenMatrix(string pathToFile)
        {
            Matrix matrix = new Matrix();

            if (File.Exists(pathToFile))
            {
                StreamReader sr;

                string buffer;
                string[] lastString = null;
                int maxWidt = 0;

                int width = 0, height = 0;

                using (sr = new StreamReader(pathToFile, System.Text.Encoding.Default))
                {
                    while ((buffer = sr.ReadLine()) != null)
                    {
                        if (width < buffer.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count())
                        {
                            width = buffer.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }

                        maxWidt += buffer.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();

                        height++;
                    }

                    height--;

                }

                bool checkMatrixForQuadro = ((maxWidt / width) * height) == (width * height);

                if (height < 0 || checkMatrixForQuadro)
                {
                    throw new Exception("Из данного файла нельзя считать матрицу." +
                        "\nЛибо в файле нет матрицы, либо матрица в файле заполнена неверно.");
                }

                float[,] arrE = new float[height, width];
                float[] arrK = new float[width];

                if (arrE.GetLength(0) != arrK.GetLength(0))
                {
                    MessageBox.Show("Количество столбцов в матрицах не равно.");
                }

                using (sr = new StreamReader(pathToFile, System.Text.Encoding.Default))
                {
                    string[] oneLineMassive = new string[width];

                    for (int i = 0; i < height; i++)
                    {
                        oneLineMassive = sr.ReadLine().Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < width; j++)
                        {
                            arrE[i, j] = float.Parse(oneLineMassive[j]);
                        }

                    }

                    lastString = sr.ReadLine().Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }


                matrix.ArrE = arrE;
                
                for (int i = 0; i < arrK.Count(); i++)
                {
                    arrK[i] = float.Parse(lastString[i]);
                }

                matrix.ArrK = arrK;
            }

            return matrix;
        }

        public static void SaveMatrixAs(string pathToFile, Matrix matrix)
        {
            using (StreamWriter sw = new StreamWriter(pathToFile))
            {
                for (int i = 0; i < matrix.ArrE.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.ArrE.GetLength(1); j++)
                    {
                        if (j < matrix.ArrE.GetLength(1) - 1)
                        {
                            sw.Write(string.Format(matrix.ArrE[i, j].ToString()) + "\t");
                        }
                        else
                        {
                            sw.Write(string.Format(matrix.ArrE[i, j].ToString()));
                        }
                    }

                    sw.Write(Environment.NewLine);
                }

                for (int i = 0; i < matrix.ArrK.GetLength(0); i++)
                {
                    if (i != matrix.ArrK.GetLength(0) - 1)
                    {
                        sw.Write(string.Format(matrix.ArrK[i].ToString() + "\t"));
                    }
                    else
                    {
                        sw.Write(string.Format(matrix.ArrK[i].ToString()));
                    }
                }

                if (matrix.ArrSum != null)
                {
                    string pathToFileSum;

                    int i = 0;
                    do
                    {
                        pathToFileSum = string.Concat(pathToFile.Substring(0, pathToFile.Length - 4), string.Format($" (Sum) {i}.txt"));
                        SaveMatrixAs(pathToFileSum, matrix.ArrSum);
                    }
                    while (i --> 0);
                }
            }
        }

        public static void SaveMatrixAs(string pathToFile, float[,] matrix)
        {
            using (StreamWriter sw = new StreamWriter(pathToFile))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (j < matrix.GetLength(1) - 1)
                        {
                            sw.Write(string.Format("{0:0.###}\t", matrix[i, j]));
                        }
                        else
                        {
                            sw.Write(string.Format("{0:0.###}", matrix[i, j]));
                        }
                    }

                    sw.Write(Environment.NewLine);
                }
            }
        }
    }
}

