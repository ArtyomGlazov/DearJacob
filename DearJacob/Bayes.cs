using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearJacob
{
    class Bayes
    {
        float[,] covMatOne, covMatTwo;
        float[,] bigMatrixSumm;
        float[] matMOne, matMTwo;

        public event Action<int> progressBarChange;

        public Bayes(Matrix matrixOne, Matrix matrixTwo)
        {
            covMatOne = matrixOne.ArrE;
            covMatTwo = matrixTwo.ArrE;

            bigMatrixSumm = matrixOne.ArrSum.AddMatrix(matrixTwo.ArrSum);

            matMOne = matrixOne.ArrK;
            matMTwo = matrixTwo.ArrK;
        }

        private float FirstStep(float[,] covMat, float[] matM, int k)
        {            
            float result = 0;
            float[,] tempM = new float[8, 8];
            float[,] mul = new float[8, 8];

            for (int j = 0; j < covMat.GetLength(1); j++)
            {
                tempM[0, j] = bigMatrixSumm[k, j];
                tempM[0, j] -= matM[j];
            }

            mul = tempM.MatrixTransposition().MatrixMultiplication(covMat.Inverse());

            mul = mul.MatrixMultiplication(tempM);
            mul = mul.MatrixMultiplication(0.5F);

            for (int i = 0; i < covMat.GetLength(1); i++)
            {
                result += mul[0, i];
            }

            return result;
        }

        private float ElenFromDeter()
        {
            float result = 0.5F * (float)Math.Log((covMatOne.Determ() / covMatTwo.Determ()));

            return result;
        }

        private bool SravneniePriznaka(int k)
        {
            float one = FirstStep(covMatOne, matMOne, k);
            float two = FirstStep(covMatTwo, matMTwo, k);
            float three = ElenFromDeter();
            float four = one - two + three;

            if (four < 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            /*bool priznak = (FirstStep(covMatOne, matMOne, k) - FirstStep(covMatTwo, matMTwo, k) + ElenFromDeter()) > 0 ? true: false;

            return priznak;*/
        }

        public void Classification (out int classOne, out int classTwo, out float oshibka)
        {
            classOne = classTwo = 0;

            for (int i = 0; i < 400; i++)
            {
                if (SravneniePriznaka(i))
                {
                    classOne++;
                }
                else
                {
                    classTwo++;
                }

                progressBarChange?.Invoke(i);
            }

            oshibka = Math.Abs((classOne - 200F)) / 200F * 100F;
        }

        /*public void Classification(out int classOne, out int classTwo)
        {
            classOne = 0; classTwo = 0;
            float priznak = 0;
            float mult1 = 0, mult2 = 0;

            float[,] inversOneCov = covMatOne.Inverse();
            float[,] inversTwoCov = covMatTwo.Inverse();

            float determinant1 = 0, determinant2 = 0;
            float[,] matrix = new float[8, 8];
            float[,] tempM1 = new float[8, 8];
            float[,] tempM2 = new float[8, 8];
            float[,] multM1 = new float[8, 8];
            float[,] multM2 = new float[8, 8];

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tempM1[0, j] = tempM2[0, j] = bigMatrixSumm[i, j];//запись очередной строки  из массива 200x8

                    tempM1[0, j] = tempM1[0, j] - matMOne[j];//X-M1
                    tempM2[0, j] = tempM2[0, j] - matMTwo[j];//X-M2
                }
                for (int k = 0; k < 8; k++)
                {
                    for (int m = 0; m < 8; m++)
                    {
                        multM1[0, k] += tempM1[0, m] * inversOneCov[m, k];// (X-M1)*E1^(-1)
                        multM2[0, k] += tempM2[0, m] * inversTwoCov[m, k];// (X-M2)*E2^(-1)
                    }
                }
                for (int k = 0; k < 8; k++)
                {
                    mult1 += 0.5F * (multM1[0, k] * tempM1[0, k]);// 0.5*(X-M1)*E1^(-1)*(X-M1)
                    mult2 += 0.5F * (multM2[0, k] * tempM2[0, k]);// 0.5*(X-M2)*E2^(-1)*(X-M2)
                }


                //////////////////////////////////////////////////////////

                determinant1 = covMatOne.Determ();
                determinant2 = covMatTwo.Determ();

                priznak = mult1 - mult2 + 0.5F * ((float)Math.Log(determinant1 / determinant2));
                if (priznak < 0)
                    classOne++;
                else classTwo++;
                mult1 = 0; mult2 = 0;
            }
        }*/
    }
}
