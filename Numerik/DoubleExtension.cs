using System;

namespace Numerik
{
    public static class DoubleExtension
    {
        public static bool EqualsZero(this double x, int exponentOfTenAsEpsilon)
        {
            return Math.Abs(x - 0d) < Math.Pow(10, -exponentOfTenAsEpsilon);
        }

        public static double RoundMantissa(this double x, int maxMantissaLength)
        {
            return Mantissen.RoundToMaxMantissaLength(maxMantissaLength, x);
        }

        public static double RoundAdd(this double a, double b, int maxMantissaLength)
        {
            return (a.RoundMantissa(maxMantissaLength) + b.RoundMantissa(maxMantissaLength)).RoundMantissa(maxMantissaLength);
        }

        public static double RoundSubtract(this double a, double b, int maxMantissaLength)
        {
            return (a.RoundMantissa(maxMantissaLength) - b.RoundMantissa(maxMantissaLength)).RoundMantissa(maxMantissaLength);
        }

        public static double RoundMultiply(this double a, double b, int maxMantissaLength)
        {
            return (a.RoundMantissa(maxMantissaLength) * b.RoundMantissa(maxMantissaLength)).RoundMantissa(maxMantissaLength);
        }

        public static double RoundDivide(this double a, double b, int maxMantissaLength)
        {
            return (a.RoundMantissa(maxMantissaLength) / b.RoundMantissa(maxMantissaLength)).RoundMantissa(maxMantissaLength);
        }
    }
}
