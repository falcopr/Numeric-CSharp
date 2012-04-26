using System;
using Numerik;
using MathNet.Numerics;

namespace TestConsoleProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(new Complex32(0.000073489174176812342378429f, 0).ToString());
            Console.WriteLine(0.000073489174176812342378426857575465465469m);
            Console.WriteLine(0.00000000000000000000000000000019m == decimal.Zero);

            double[,] arrayMatix = new double[5,4];
            arrayMatix[0, 0] = 1d;
            arrayMatix[0, 1] = 2d;
            arrayMatix[0, 2] = 3d;
            arrayMatix[0, 3] = 3d;

            arrayMatix[1, 0] = 4d;
            arrayMatix[1, 1] = 5d;
            arrayMatix[1, 2] = 6d;
            arrayMatix[1, 3] = 3d;

            arrayMatix[2, 0] = 7d;
            arrayMatix[2, 1] = 8d;
            arrayMatix[2, 2] = 9d;
            arrayMatix[2, 3] = 3d;

            arrayMatix[3, 0] = 7d;
            arrayMatix[3, 1] = 8d;
            arrayMatix[3, 2] = 9d;
            arrayMatix[3, 3] = 3d;

            arrayMatix[4, 0] = 7d;
            arrayMatix[4, 1] = 8d;
            arrayMatix[4, 2] = 9d;
            arrayMatix[4, 3] = 3d;

            Matrix matrix = new Matrix(arrayMatix);
            Console.WriteLine(matrix.ToString());

            Console.WriteLine(Matrix.CreateIdentityMatrix(3));

            Console.ReadLine();
        }
    }
}
