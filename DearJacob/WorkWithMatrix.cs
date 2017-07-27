using System;

namespace DearJacob
{
    class WorkWithMatrix
    {
        private float[,] arrUNext;
        private byte kolIter;
        float accuracy;
        private Matrix myMatrix;

        public WorkWithMatrix(Matrix matrix, float accuracy)
        {
            this.accuracy = accuracy;
            myMatrix = matrix;
            arrUNext = new float[matrix.ArrE.GetLength(0), matrix.ArrE.GetLength(1)];
        }

        public byte KolIter { get => kolIter; }
        public Matrix MyMatrix { get => myMatrix; }

        public float[,] MatricaKPlusOne()
        {
            kolIter = 0;

            float[,] arrU;
            float[,] arrUT;

            float[,] arrA = MyMatrix.ArrE;

            arrUNext.MakeUnitMatrix();

            do
            {
                arrA.GiveMeMaxEl(out int iMax, out int jMax);

                arrU = arrA.MatrixRotate(iMax, jMax);

                arrUT = arrU.MatrixTransposition();

                arrUNext = arrUNext.MatrixMultiplication(arrU);

                arrA = arrUT.MatrixMultiplication(arrA);
                arrA = arrA.MatrixMultiplication(arrU);

                kolIter++;
            }
            while (arrA.SumNeDiagElem() > accuracy);

            myMatrix.ArrEPlus = arrA;
            myMatrix.ArrF = arrUNext;

            return arrA;
        }

        private float[,] RightTerm()
        {
            float[,] arrA = MatricaKPlusOne();
            float[,] randomArr = GenerateRandomMatrix();

            float[,] arrRigthTerm;

            arrRigthTerm = arrA.SqrtFromMatrix();

            arrRigthTerm = arrUNext.MatrixMultiplication(arrA);

            arrRigthTerm = randomArr.MatrixMultiplication(arrA);

            return arrRigthTerm;
        }

        public float[,] SummaTerm()
        {
            float[,] arrSummaTerm = RightTerm();
            float[] arrM = MyMatrix.ArrK;

            for (int i = 0; i < arrSummaTerm.GetLength(0); i++)
            {
                for (int j = 0; j < arrSummaTerm.GetLength(1); j++)
                {
                    arrSummaTerm[i, j] += arrM[j];
                }
            }

            return arrSummaTerm;
        }

        private float[,] GenerateRandomMatrix()
        {
            float[,] randomArr = new float[200, MyMatrix.ArrE.GetLength(1)];

            Random rand = new Random();

            for (int i = 0; i < randomArr.GetLength(0); i++)
            {
                for (int j = 0; j < randomArr.GetLength(1); j++)
                {
                    switch (rand.Next(1, 3))
                    {
                        case 1:
                            randomArr[i, j] = rand.Next(1, 100) / 100;
                            break;
                        case 2:
                            randomArr[i, j] = rand.Next(100, 200) / 100;
                            break;
                        case 3:
                            randomArr[i, j] = rand.Next(200, 300) / 100;
                            break;
                    }
                }
            }

            return randomArr;
        }
    }
}
