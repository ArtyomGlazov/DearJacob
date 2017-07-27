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

        public static float[,] AddMatrix(this float[,] matrixOne, float[,] matrixTwo)
        {
            float[,] matrixResult = new float[matrixOne.GetLength(0) + matrixTwo.GetLength(0), matrixOne.GetLength(1)];

            AddMat(matrixOne, ref matrixResult);
            AddMat(matrixTwo, ref matrixResult);

            return matrixResult;
        }

        private static void AddMat(float[,] matrixOne, ref float[,] matrixResult)
        {
            for (int i = 0; i < matrixOne.GetLength(0); i++)
            {
                for (int j = 0; j < matrixOne.GetLength(1); j++)
                {
                    matrixResult[i, j] = matrixOne[i, j];
                }
            }
        }

        public static double[,] ConvertMatrixToDouble(this float[,] matrix)
        {
            double[,] matrixResult = new double[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrixResult[i, j] = Convert.ToDouble(matrix[i, j]);
                }
            }

            return matrixResult;
        }

        public static float[,] ConvertMatrixToFloat(this double[,] matrix)
        {
            float[,] matrixReslut = new float[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrixReslut[i, j] = Convert.ToSingle(matrix[i, j]);
                }
            }

            return matrixReslut;
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

        public static float[,] MatrixMultiplication(this float[,] onePacient, float chislo)
        {
            float[,] result = onePacient;

            for (int i = 0; i < onePacient.GetLength(0); i++)
            {
                for (int j = 0; j < onePacient.GetLength(1); j++)
                {
                    result[i, j] *= chislo;
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

        private static float[,] GetMinor(this float[,] matrix, int row, int column)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception("Число строк в матрице не совпадает с числом столбцов");
            float[,] buf = new float[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }
            return buf;
        }

        public static float Determ(this float[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception("Число строк в матрице не совпадает с числом столбцов");
            float det = 0;
            int Rank = matrix.GetLength(0);
            if (Rank == 1) det = matrix[0, 0];
            if (Rank == 2) det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            if (Rank > 2)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    det += (float)Math.Pow(-1, 0 + j) * matrix[0, j] * Determ(matrix.GetMinor(0, j));
                }
            }
            return det;
        }

        private static float[] HelperSolve(float[,] luMatrix, float[] b)
        {
            // Решаем luMatrix * x = b
            int n = luMatrix.GetLength(0);
            float[] x = new float[n];
            b.CopyTo(x, 0);
            for (int i = 1; i < n; ++i)
            {
                float sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i,j] * x[j];
                x[i] = sum;
            }
            x[n - 1] /= luMatrix[n - 1,n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                float sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i,j] * x[j];
                x[i] = sum / luMatrix[i,i];
            }
            return x;
        }

        private static float[,] MatrixDuplicate(float[,] matrix)
        {
            float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); ++i)
                for (int j = 0; j < matrix.GetLength(1); ++j)
                    result[i,j] = matrix[i,j];
            return result;
        }

        private static float[,] MatrixDecompose(float[,] matrix, out int[] perm, out int toggle)
        {
            // Разложение LUP Дулитла. Предполагается,
            // что матрица квадратная.
            int n = matrix.GetLength(0); // для удобства
            float[,] result = MatrixDuplicate(matrix);
            perm = new int[n];
            for (int i = 0; i < n; ++i) { perm[i] = i; }
            toggle = 1;
            for (int j = 0; j < n - 1; ++j) // каждый столбец
            {
                float colMax = Math.Abs(result[j,j]); // Наибольшее значение в столбце j
                int pRow = j;
                for (int i = j + 1; i < n; ++i)
                {
                    if (result[i,j] > colMax)
                    {
                        colMax = result[i,j];
                        pRow = i;
                    }
                }
                if (pRow != j) // перестановка строк
                {
                    float[] rowPtr = result.GetRow(pRow);
                    result = SetRow(result, result, pRow, j);
                    result = SetRow(result, rowPtr, j);
                    int tmp = perm[pRow]; // Меняем информацию о перестановке
                    perm[pRow] = perm[j];
                    perm[j] = tmp;
                    toggle = -toggle; // переключатель перестановки строк
                }
                if (Math.Abs(result[j,j]) < 1.0E-20)
                    return null;
                for (int i = j + 1; i < n; ++i)
                {
                    result[i,j] /= result[j,j];
                    for (int k = j + 1; k < n; ++k)
                        result[i,k] -= result[i,j] * result[j,k];
                }
            } // основной цикл по столбцу j
            return result;
        }

        private static float[,] SetRow(float[,] matrixOne, float[] mass, int row)
        {
            for (int i = 0; i < matrixOne.GetLength(1); i++)
            {
                matrixOne[row, i] = mass[i];
            }

            return matrixOne;
        }

        private static float[,] SetRow (float[,] matrixOne, float[,] matrixTwo, int pRow, int pRowTwo)
        {
            for (int i = 0; i < matrixOne.GetLength(1); i++)
            {
                matrixOne[pRow, i] = matrixTwo[pRowTwo, i];
            }

            return matrixOne;
        }

        private static float[] GetRow(this float[,] matrix, int row)
        {
            float[] result = new float[matrix.GetLength(0)];

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                result[j] = matrix[row, j];
            }

            return result;
        }

        public static float[,] Inverse(this float[,] matrix)
        {
            int n = matrix.GetLength(0);
            float[,] result = MatrixDuplicate(matrix);
            int[] perm;
            int toggle;
            float[,] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");
            float[] b = new float[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0F;
                    else
                        b[j] = 0.0F;
                }
                float[] x = HelperSolve(lum, b);
                for (int j = 0; j < n; ++j)
                    result[j,i] = x[j];
            }
            return result;
        }
    }
}
