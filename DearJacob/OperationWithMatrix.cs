using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DearJacob
{
    static class OperationWithMatrix
    {
        public static void GiveMeMatrixFromTextBox(this float[,] matrix, RichTextBox rich)
        {
            rich.GiveMeMatrix(out matrix);
        }

        public static void GiveMeMatrixFromTextBox(this float[] matrix, RichTextBox rich)
        {
            rich.GiveMeMatrix(out matrix);
        }

        public static float[,] MatrixMultiplication(this float[,] onePacient, float[,] twoPacient)
        {
            float[,] result = new float[onePacient.GetLength(0), twoPacient.GetLength(1)];

            for (int i = 0; i < onePacient.GetLength(0); i++)
            {
                for (int j = 0; j < twoPacient.GetLength(1); j++)
                {
                    for (int k = 0; k < twoPacient.GetLength(0) || k < onePacient.GetLength(1); k++)
                    {
                        result[i, j] += onePacient[i, k] * twoPacient[k, j];
                    }
                }
            }

            return result;
        }

        public static float[,] MatrixTransposition(this float[,] arrU)
        {
            float[,] arrUT = new float[arrU.GetLength(0), arrU.GetLength(1)];

            for (int i = 0; i < arrU.GetLength(0); i++)
            {
                for (int j = 0; j < arrU.GetLength(1); j++)
                {
                    arrUT[i, j] = arrU[j, i];
                }
            }

            return arrUT;
        }

        public static float[,] SqrtFromMatrix(this float[,] arrA)
        {
            for (int i = 0; i < arrA.GetLength(0); i++)
            {
                for (int j = 0; j < arrA.GetLength(1); j++)
                {
                    arrA[i, j] = (float)Math.Sqrt(Math.Abs(arrA[i, j]));
                }
            }

            return arrA;
        }

        public static void MakeUnitMatrix(this float[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        arr[i, j] = 1;
                    }
                    else
                    {
                        arr[i, j] = 0;
                    }
                }
            }
        }

        public static float SumNeDiagElem(this float[,] matrix)
        {
            float result = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i != j)
                    {
                        result += (float)Math.Pow(matrix[i, j], 2);
                    }
                }
            }

            return (float)Math.Sqrt(result);
        }

        public static float GiveMeMaxEl(this float[,] matrix, out int iMax, out int jMax)
        {
            float maxEl = 0;
            iMax = jMax = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((Math.Abs(maxEl) < Math.Abs(matrix[i, j])) && (i != j))
                    {
                        maxEl = matrix[i, j];
                        iMax = i;
                        jMax = j;
                    }
                }
            }

            return maxEl;
        }

        public static float[,] MatrixRotate(this float[,] matrix, int iMax, int jMax)
        {
            float angle = matrix.AngleRotate(iMax, jMax);

            float[,] arrU = new float[matrix.GetLength(0), matrix.GetLength(1)];

            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            arrU.MakeUnitMatrix();

            arrU[iMax, jMax] = -sin;
            arrU[jMax, iMax] = sin;

            arrU[iMax, iMax] = cos;
            arrU[jMax, jMax] = cos;

            return arrU;
        }

        private static float AngleRotate(this float[,] matrix, int iMax, int jMax)
        {
            float maxEl = matrix[iMax, jMax];
            float iDiag = matrix[iMax, iMax];
            float jDiag = matrix[jMax, jMax];

            float angleRotate;

            if (iDiag == jDiag)
            {
                angleRotate = (float)Math.PI / 4;
            }
            else
            {
                angleRotate = (float)((Math.Atan2((2 * maxEl), (iDiag - jDiag))) / 2);
            }

            return angleRotate;
        }
    }
}
