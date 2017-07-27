namespace DearJacob
{
    public struct Matrix
    {
        float[,] arrE;
        float[] arrK;
        float[,] arrEPlus;
        float[,] arrF;
        float[,] arrSum;

        public float[,] ArrE { get => arrE; set => arrE = value; }
        public float[] ArrK { get => arrK; set => arrK = value; }
        public float[,] ArrEPlus { get => arrEPlus; set => arrEPlus = value; }
        public float[,] ArrF { get => arrF; set => arrF = value; }
        public float[,] ArrSum { get => arrSum; set => arrSum = value; }
    }
}
