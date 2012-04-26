using System;
using System.Globalization;
using System.Numerics;

namespace Numerik
{
    public static class Mantissen
    {
        public const int MaxDecimalPlaces = 28;
        public const int MaxDoublePlaces = 16;

        public static decimal RoundToMaxMantissaLength(int maxMantissaLength, decimal digit)
        {
            if (maxMantissaLength < 2)
            {
                throw new Exception("Die Mantissenlaenge muss groeßer 1 sein!");
            }

            int sign = Math.Sign(digit);
            decimal absDigit = Math.Abs(digit);
            const int baseDigit = 10;

            int absExponent = GetExponentOfDecimal(absDigit);
            int exponent = GetExponentOfDecimal(digit);
            int includingPlusOneMantissa = maxMantissaLength + 1;
            int actualExponentToRound;

            if (exponent > 0)
            {
                actualExponentToRound = Math.Min(includingPlusOneMantissa - absExponent, maxMantissaLength);
            }
            else
            {
                actualExponentToRound = includingPlusOneMantissa - exponent;
            }
            

            decimal baseDigitPowActualExponent = Convert.ToDecimal(Math.Pow(baseDigit, actualExponentToRound));
            decimal baseDigitPowActualExponentExcludingPlusOneMantissa =
                Convert.ToDecimal(Math.Pow(baseDigit, actualExponentToRound - 1));

            var digitIncludingMantissaToInteger = (BigInteger)Math.Floor(absDigit * baseDigitPowActualExponent);
            var digitIncludingMantissaToIntegerExcludingPlusOneMantissa =
                (BigInteger)Math.Floor(absDigit * baseDigitPowActualExponentExcludingPlusOneMantissa);

            var differenceForFindingOutRounding = (decimal)(digitIncludingMantissaToInteger - digitIncludingMantissaToIntegerExcludingPlusOneMantissa);

            if (differenceForFindingOutRounding > (baseDigit / 2m))
            {
                digitIncludingMantissaToIntegerExcludingPlusOneMantissa += 1;
            }

            decimal result = (decimal)digitIncludingMantissaToIntegerExcludingPlusOneMantissa/
                             baseDigitPowActualExponentExcludingPlusOneMantissa;

            if (sign == 0)
            {
                return -result;
            }

            return result;
        }

        public static double RoundToMaxMantissaLength(int maxMantissaLength, double digit)
        {
            const int baseDigit = 10;

            if (maxMantissaLength < 2)
            {
                throw new Exception("Die Mantissenlaenge muss groeßer 1 sein!");
            }

            int sign = Math.Sign(digit);
            double absDigit = Math.Abs(digit);
            int temporaryAbsoluteExponent = GetExponentOfDecimal(absDigit);

            digit = digit*Math.Pow(baseDigit, -temporaryAbsoluteExponent);
            absDigit = Math.Abs(digit);

            int absExponent = GetExponentOfDecimal(absDigit);
            int includingPlusOneMantissa = maxMantissaLength + 1;
            int actualExponentToRound;

            if (absExponent > 0)
            {
                actualExponentToRound = includingPlusOneMantissa - absExponent;
            }
            else
            {
                actualExponentToRound = includingPlusOneMantissa;
            }

            double baseDigitPowActualExponent = Math.Pow(baseDigit, actualExponentToRound);
            double baseDigitPowActualExponentExcludingPlusOneMantissa = Math.Pow(baseDigit, actualExponentToRound - 1);

            double digitIncludingMantissaToInteger = Math.Floor(absDigit * baseDigitPowActualExponent);
            double digitIncludingMantissaToIntegerExcludingPlusOneMantissa = Math.Floor(absDigit * baseDigitPowActualExponentExcludingPlusOneMantissa);

            /* rounding */
            double differenceForFindingOutRounding = digitIncludingMantissaToInteger - digitIncludingMantissaToIntegerExcludingPlusOneMantissa * baseDigit;

            if (differenceForFindingOutRounding > (baseDigit / 2d))
            {
                digitIncludingMantissaToIntegerExcludingPlusOneMantissa += 1;
            }
            double quotientResult = (digitIncludingMantissaToIntegerExcludingPlusOneMantissa/
                                     baseDigitPowActualExponentExcludingPlusOneMantissa);

            /* return to root exponent*/
            double temporaryExponentWithBase = Math.Pow(Convert.ToDouble(baseDigit), -temporaryAbsoluteExponent);
            double result;
            
            if (temporaryExponentWithBase < 1)
            {
                temporaryExponentWithBase = Math.Pow(Convert.ToDouble(baseDigit), temporaryAbsoluteExponent);
                result = quotientResult * temporaryExponentWithBase;
            }
            else
            {
                result = quotientResult / temporaryExponentWithBase;
            }

            /* destinguish sign */
            if (sign == -1)
            {
                return -result;
            }

            return result;
        }

        public static int GetExponentOfDecimal(decimal digit)
        {
            //if (digit == 0m)
            //{
            //    throw new Exception("Im Logarithmus darf die Zahl nicht 0 sein.");
            //}

            double absoluteDecimalToDoubleDigit = Math.Log10(Convert.ToDouble(Math.Abs(digit)));

            double exponent = absoluteDecimalToDoubleDigit > 0 ? Math.Ceiling(absoluteDecimalToDoubleDigit) : Math.Floor(absoluteDecimalToDoubleDigit);

            return (int)exponent;
        }

        public static int GetExponentOfDecimal(double digit)
        {
            //if (digit == 0d)
            //{
            //    throw new Exception("Im Logarithmus darf die Zahl nicht 0 sein.");
            //}

            double absoluteDecimalToDoubleDigit = Math.Log10(Math.Abs(digit));

            double exponent = absoluteDecimalToDoubleDigit > 0 ? Math.Ceiling(absoluteDecimalToDoubleDigit) : Math.Floor(absoluteDecimalToDoubleDigit);

            return (int)exponent;
        }

        public static BigInteger DecimalToBigInteger(int maxDecimalPlaces, decimal digit)
        {
            var nfi = new NumberFormatInfo
                          {
                              NumberDecimalSeparator = ".",
                              NumberGroupSeparator = string.Empty
                          };

            string stringedDigit = digit.ToString(nfi);
            int placeOfDecimalSeparator = stringedDigit.LastIndexOf('.');
            int lengthOfStringedDigit = stringedDigit.Length;

            if (placeOfDecimalSeparator < 0)
            {
                return (BigInteger)digit;
            }

            string integer = stringedDigit.Split('.')[0];
            string decimalPlaces = stringedDigit.Split('.')[1];
            stringedDigit = integer + decimalPlaces;

            if (decimalPlaces.Length < maxDecimalPlaces)
            {
                return BigInteger.Parse(stringedDigit);
            }

            stringedDigit = stringedDigit.Insert(Math.Max(integer.Length + maxDecimalPlaces - 1, 0), ".");

            return BigInteger.Parse(stringedDigit.Split('.')[0]);
        }
    }
}
