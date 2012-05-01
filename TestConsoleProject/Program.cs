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

            var arrayMatrix = new double[3, 3];
            arrayMatrix[0, 0] = 2.1d;
            arrayMatrix[0, 1] = -1.3d;
            arrayMatrix[0, 2] = 0.9d;

            arrayMatrix[1, 0] = 2512d;
            arrayMatrix[1, 1] = 8.8d;
            arrayMatrix[1, 2] = -6.2d;

            arrayMatrix[2, 0] = -2516d;
            arrayMatrix[2, 1] = -7.6d;
            arrayMatrix[2, 2] = 4.6d;

            var matrix1 = new Matrix(arrayMatrix);

            arrayMatrix = new double[1,3];

            arrayMatrix[0, 0] = 6.5d;
            arrayMatrix[0, 1] = -5.3d;
            arrayMatrix[0, 2] = 2.9d;

            var matrix2 = new Matrix(arrayMatrix);

            Mantissen.Active = true;
            Matrix.MantissaLength = 5;

            var newMatrix = matrix1.LUPartition(matrix2, false);

            Console.WriteLine(newMatrix["x"]);

            newMatrix = matrix1.LUPartition(matrix2, true);

            Console.WriteLine(newMatrix["x"]);

            int roundConstant = 8;

            double a = 0.23371258e-4d;
            double b = 0.33678429e+2d;
            double c = -0.33677811e+2d;

            double add1 = (a + (b + c).RoundMantissa(roundConstant)).RoundMantissa(roundConstant);
            double add2 = ((a + b).RoundMantissa(roundConstant) + c).RoundMantissa(8);

            Console.WriteLine(add1);
            Console.WriteLine(add2);
            Console.WriteLine(a+b+c);

            arrayMatrix = new double[3, 3];
            arrayMatrix[0, 0] = 2.1d;
            arrayMatrix[0, 1] = -1.3d;
            arrayMatrix[0, 2] = 0.9d;

            arrayMatrix[1, 0] = 2512d;
            arrayMatrix[1, 1] = 8.8d;
            arrayMatrix[1, 2] = -6.2d;

            arrayMatrix[2, 0] = -2516d;
            arrayMatrix[2, 1] = -7.6d;
            arrayMatrix[2, 2] = 4.6d;

            Console.ReadLine();
        }
    }
}
