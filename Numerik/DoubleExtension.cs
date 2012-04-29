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
    }
}
