using System;
using System.Globalization;
using System.Numerics;
using NUnit.Framework;
using Numerik;

namespace Mantissen_TestFixture
{
    [TestFixture]
    public abstract class Mantissen_TestFixtureBase
    {
        [TestFixtureSetUp]
        public void TestFixtureTearUp()
        {
            Mantissen.Active = true;

            PrepareContext();
            CallMethodToTest();
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }

        public abstract void PrepareContext();
        public abstract void CallMethodToTest();
    }

    [TestFixture]
    public class RoundToMaxMantissaLength_Decimal_TestFixture : Mantissen_TestFixtureBase
    {
        private decimal m_DigitInput;
        private decimal m_DigitOutput;

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die Mantissenlaenge muss groeßer 1 sein!")]
        public void MantissaLengthIsOne()
        {
            m_DigitInput = 2m;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(1, m_DigitInput);
        }
        
        [Test]
        public void ExponentIsEqualToOneAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 1.111111111119867676858765876m;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(1.11111111112m, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToFourAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 1030.1111111194635365365m;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(1030.11111112m, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusFourAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 0.0001111111111119675757m;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(0.000111111111112, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToTwelveAndMantissaLengthIs28()
        {
            m_DigitInput = 801999999990.11111111111111111111111111119m;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(28, m_DigitInput);

            Assert.AreEqual(8019999999990.1111111111111111m, m_DigitOutput);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class GetExponentOfDecimal_Decimal_TestFixture : Mantissen_TestFixtureBase
    {
        private decimal m_DigitInput;
        private decimal m_DigitOutput;

        [Test]
        public void ExponentIsEqualToOne()
        {
            m_DigitInput = 1.111111111119998685m;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(1, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToFour()
        {
            m_DigitInput = 1030.11111111924324245324m;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(4, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusFour()
        {
            m_DigitInput = 0.00011111111111119m;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(-4, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusThirty()
        {
            m_DigitInput = 0.00000000000000000000000000000019m;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitOutput);

            Assert.AreEqual(decimal.Zero, m_DigitOutput);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class RoundToMaxMantissaLength_Double_TestFixture : Mantissen_TestFixtureBase
    {
        private double m_DigitInput;
        private double m_DigitOutput;

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die Mantissenlaenge muss groeßer 1 sein!")]
        public void MantissaLengthIsOne()
        {
            m_DigitInput = 2d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(1, m_DigitInput);
        }

        [Test]
        public void ExponentIsEqualToOneAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 1.111111111119867676858765876d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(1.11111111112d, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToFourAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 1030.1111111194635365365d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(1030.11111112d, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusFourAndMantissaLengthIsTwelve()
        {
            m_DigitInput = 0.0001111111111119675757d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(0.000111111111112d, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualTo30AndMantissaLengthIsTwelve()
        {
            m_DigitInput = 3.143512451234123423412341255E+30d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(12, m_DigitInput);

            Assert.AreEqual(3.14351245123E+30d, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToTwelveAndMantissaLengthIs28()
        {
            m_DigitInput = 801999999990.11111111111111111111111111119d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(28, m_DigitInput);

            Assert.AreEqual(801999999990.111d, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinus40AndMantissaLengthIs13()
        {
            m_DigitInput = 1.1111111111119E-40d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(13, m_DigitInput);

            Assert.AreEqual(1.111111111112E-40d, m_DigitOutput);
        }

        [Test]
        public void NegativeNumber_ExponentIsEqualToMinus40AndMantissaLengthIs13()
        {
            m_DigitInput = -1.1111111111119E-40d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(13, m_DigitInput);

            Assert.AreEqual(-1.111111111112E-40d, m_DigitOutput);
        }

        [Test]
        public void Exponent_Is_One_And_MantissaLength_Is_5()
        {
            m_DigitInput = 4.6d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(5, m_DigitInput);

            Assert.AreEqual(m_DigitInput, m_DigitOutput);
        }

        [Test]
        public void Exponent_Is_MinusOne_And_MantissaLength_Is_5()
        {
            m_DigitInput = -4.6d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(5, m_DigitInput);

            Assert.AreEqual(m_DigitInput, m_DigitOutput);
        }

        [Test]
        public void Exponent_Is_PlusFour_And_MantissaLength_Is_5()
        {
            m_DigitInput = 1555.0536d;
            m_DigitOutput = Mantissen.RoundToMaxMantissaLength(5, m_DigitInput);

            Assert.AreEqual("1555.1", m_DigitOutput.ToString(NumberFormatInfo.InvariantInfo));
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class GetExponentOfDecimal_Double_TestFixture : Mantissen_TestFixtureBase
    {
        private double m_DigitInput;
        private double m_DigitOutput;

        [Test]
        public void ExponentIsEqualToOne()
        {
            m_DigitInput = 1.111111111119998685d;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(1, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToFour()
        {
            m_DigitInput = 1030.11111111924324245324d;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(4, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusFour()
        {
            m_DigitInput = 0.00011111111111119d;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitInput);

            Assert.AreEqual(-4, m_DigitOutput);
        }

        [Test]
        public void ExponentIsEqualToMinusThirty()
        {
            m_DigitInput = 1.9e-31d;
            m_DigitOutput = Mantissen.GetExponentOfDecimal(m_DigitOutput);

            Assert.AreEqual(-31, m_DigitOutput);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class DecimalToBigInteger_TestFixture
    {
        [Test]
        public void PlaceOfDecimalSeparatorIsLessThanZero()
        {
            Assert.AreEqual(BigInteger.Parse("831231239231123"), Mantissen.DecimalToBigInteger(Mantissen.MaxDecimalPlaces, 831231239231123.00m));
        }

        [Test]
        public void PlaceOfDecimalSeparatorIsGreaterThanZero()
        {
            Assert.AreEqual(BigInteger.Parse("831231239231123.99999999999999999999"), Mantissen.DecimalToBigInteger(Mantissen.MaxDecimalPlaces, 831231239231123.99999999999999999999m));
        }
    }
}
