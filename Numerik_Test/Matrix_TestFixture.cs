using System;
using NUnit.Framework;
using Numerik;

namespace Matrix_TestFixture
{
    [TestFixture]
    public abstract class Matrix_TestFixtureBase
    {
        protected Matrix InputMatrix { get; set; }
        protected Matrix OutputMatrix { get; set; }

        [TestFixtureSetUp]
        public void TestFixtureTearUp()
        {
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
    public class Constructor_CreatingAQuadraticMatrix_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void Getting_Quadratic_Matrix_From_Constructor()
        {
            Assert.AreEqual(3, OutputMatrix.MaxColumnCount);
            Assert.AreEqual(3, OutputMatrix.MaxRowCount);
        }

        public override void PrepareContext()
        {
            OutputMatrix = new Matrix(3);
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class Constructor_EnterColumnCount2AndRowCount3_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void OutputMatrix_Doubles_Should_All_Be_0d()
        {
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 2));
        }

        [Test]
        public void OutputMatrix_Should_Have_MaxColumnCount2()
        {
            Assert.AreEqual(2, OutputMatrix.MaxColumnCount);
        }

        [Test]
        public void OutputMatrix_Should_Have_MaxRowCount3()
        {
            Assert.AreEqual(3, OutputMatrix.MaxRowCount);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = new Matrix(2, 3);
        }
    }

    [TestFixture]
    public class Constructor_ArrayMatrixWithColumnCount2AndRowCount3_TestFixture : Matrix_TestFixtureBase
    {
        protected double[,] ArrayMatrix { get; set; }

        [Test]
        public void OutputMatrix_Doubles_Should_Have_The_Correct_Values_From_ArrayMatrix()
        {
            Assert.AreEqual(1.123d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(4.2133d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(2314.1d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(0.0001d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(999E+80d, OutputMatrix.GetDoubleFromMatrix(1, 2));
        }

        [Test]
        public void OutputMatrix_Should_Have_MaxColumnCount2()
        {
            Assert.AreEqual(2, OutputMatrix.MaxColumnCount);
        }

        [Test]
        public void OutputMatrix_Should_Have_MaxRowCount3()
        {
            Assert.AreEqual(3, OutputMatrix.MaxRowCount);
        }

        public override void PrepareContext()
        {
            ArrayMatrix = new double[2,3];

            ArrayMatrix[0, 0] = 1.123d;
            ArrayMatrix[0, 1] = 4.2133d;
            ArrayMatrix[0, 2] = 2314.1d;

            ArrayMatrix[1, 0] = 0.0001d;
            ArrayMatrix[1, 1] = 0d;
            ArrayMatrix[1, 2] = 999E+80d;
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = new Matrix(ArrayMatrix);
        }
    }

    [TestFixture]
    public class CreateIdentityMatrix_TestFixture : Matrix_TestFixtureBase
    {

        [Test]
        public void CreatingATwoDimensionalIdentityMatrix()
        {
            OutputMatrix = Matrix.CreateIdentityMatrix(2);

            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 1));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(1, 1));
        }

        [Test]
        public void CreatingAThreeDimensionalIdentityMatrix()
        {
            OutputMatrix = Matrix.CreateIdentityMatrix(3);

            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 2));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(2, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(2, 1));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(2, 2));
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class IsMatrixQuadratic_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void MatrixIsQuadratic()
        {
            Assert.IsTrue(new Matrix(3, 3).IsMatrixQuadratic());
        }

        [Test]
        public void MatrixIsNotQuadratic()
        {
            Assert.IsFalse(new Matrix(2, 5).IsMatrixQuadratic());
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class MaxColumnCount_And_MaxRowCount_Getter_TestFixture: Matrix_TestFixtureBase
    {
        [Test]
        public void ColumnCount_Should_Be_Three()
        {
            Assert.AreEqual(3, InputMatrix.MaxColumnCount);
        }

        [Test]
        public void RowCount_Should_Be_Four()
        {
            Assert.AreEqual(4, InputMatrix.MaxRowCount);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
            InputMatrix = new Matrix(3, 4);
        }
    }

    [TestFixture]
    public class GetIdentityMatrix_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die Identitätsmatrix muss quadratisch sein!")]
        public void Matrix_Is_Not_Quadratic()
        {
            new Matrix(3, 4).GetIdentityMatrix();
        }

        [Test]
        public void Matrix_Is_Quadratic()
        {
            InputMatrix = new Matrix(3);
            OutputMatrix = InputMatrix.GetIdentityMatrix();

            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(1, 2));

            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(2, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(2, 1));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(2, 2));
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class GetDoubleFromMatrix_TestFixtre : Matrix_TestFixtureBase
    {
        protected double DoubleFromMatrix { get; set; }

        [Test]
        public void Should_Return_Correct_Value_From_Matrix()
        {
            Assert.AreEqual(7d, DoubleFromMatrix);
        }

        public override void PrepareContext()
        {
            var arrayMatix = new double[5, 4];
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

            InputMatrix = new Matrix(arrayMatix);
        }

        public override void CallMethodToTest()
        {
            DoubleFromMatrix = InputMatrix.GetDoubleFromMatrix(3, 0);
        }
    }

    [TestFixture]
    public class SetDoubleFromMatrix_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void Get_Correct_Field_From_Matrix()
        {
            Assert.AreEqual(9000.9d, InputMatrix.GetDoubleFromMatrix(2, 3));
        }

        public override void PrepareContext()
        {
            InputMatrix = new Matrix(3);
        }

        public override void CallMethodToTest()
        {
            InputMatrix.SetDoubleOfMatrix(9000.9d, 2, 3);
        }
    }

    [TestFixture]
    public class ToArray_TestFixture : Matrix_TestFixtureBase
    {
        protected double[,] StartArray { get; set; }
        protected double[,] ResultArray { get; set; }

        [Test]
        public void DoubleArray_Must_Contain_The_Same_Values_As_The_Matrix()
        {
            Assert.AreEqual(StartArray[0, 0], ResultArray[0, 0]);
            Assert.AreEqual(StartArray[0, 1], ResultArray[0, 1]);
            Assert.AreEqual(StartArray[0, 2], ResultArray[0, 2]);

            Assert.AreEqual(StartArray[1, 0], ResultArray[1, 0]);
            Assert.AreEqual(StartArray[1, 1], ResultArray[1, 1]);
            Assert.AreEqual(StartArray[1, 2], ResultArray[1, 2]);

            Assert.AreEqual(StartArray[2, 0], ResultArray[2, 0]);
            Assert.AreEqual(StartArray[2, 1], ResultArray[2, 1]);
            Assert.AreEqual(StartArray[2, 2], ResultArray[2, 2]);
        }

        public override void PrepareContext()
        {
            var arrayMatix = new double[3, 3];
            arrayMatix[0, 0] = 1d;
            arrayMatix[0, 1] = 2d;
            arrayMatix[0, 2] = 3d;

            arrayMatix[1, 0] = 4d;
            arrayMatix[1, 1] = 5d;
            arrayMatix[1, 2] = 6d;

            arrayMatix[2, 0] = 7d;
            arrayMatix[2, 1] = 8d;
            arrayMatix[2, 2] = 9d;

            StartArray = arrayMatix;

            InputMatrix = new Matrix(arrayMatix);
        }

        public override void CallMethodToTest()
        {
            ResultArray = InputMatrix.ToArray();
        }
    }

    [TestFixture]
    public class SwapColumns_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void Should_Return_Correct_Matrix()
        {
            Assert.AreEqual(7d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(8d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(9d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(4d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(5d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(6d, OutputMatrix.GetDoubleFromMatrix(1, 2));

            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(2, 0));
            Assert.AreEqual(2d, OutputMatrix.GetDoubleFromMatrix(2, 1));
            Assert.AreEqual(3d, OutputMatrix.GetDoubleFromMatrix(2, 2));
        }

        public override void PrepareContext()
        {
            var arrayMatix = new double[3, 3];
            arrayMatix[0, 0] = 1d;
            arrayMatix[0, 1] = 2d;
            arrayMatix[0, 2] = 3d;

            arrayMatix[1, 0] = 4d;
            arrayMatix[1, 1] = 5d;
            arrayMatix[1, 2] = 6d;

            arrayMatix[2, 0] = 7d;
            arrayMatix[2, 1] = 8d;
            arrayMatix[2, 2] = 9d;

            InputMatrix = new Matrix(arrayMatix);
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = InputMatrix.SwapColumns(0, 2);
        }
    }

    [TestFixture]
    public class SwapRows_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void Should_Return_Correct_Matrix()
        {
            Assert.AreEqual(3d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(2d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(1d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(6d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(5d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(4d, OutputMatrix.GetDoubleFromMatrix(1, 2));

            Assert.AreEqual(9d, OutputMatrix.GetDoubleFromMatrix(2, 0));
            Assert.AreEqual(8d, OutputMatrix.GetDoubleFromMatrix(2, 1));
            Assert.AreEqual(7d, OutputMatrix.GetDoubleFromMatrix(2, 2));
        }

        public override void PrepareContext()
        {
            var arrayMatix = new double[3, 3];
            arrayMatix[0, 0] = 1d;
            arrayMatix[0, 1] = 2d;
            arrayMatix[0, 2] = 3d;

            arrayMatix[1, 0] = 4d;
            arrayMatix[1, 1] = 5d;
            arrayMatix[1, 2] = 6d;

            arrayMatix[2, 0] = 7d;
            arrayMatix[2, 1] = 8d;
            arrayMatix[2, 2] = 9d;

            InputMatrix = new Matrix(arrayMatix);
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = InputMatrix.SwapRows(0, 2);
        }
    }

    [TestFixture]
    public class HasMatrixSameRowsAndColumns_TestFixture : Matrix_TestFixtureBase
    {
        protected bool ComparisonResult { get; set; }

        [Test]
        public void Matrix_Has_Not_The_Same_Rows_And_Columns()
        {
            ComparisonResult = new Matrix(2, 5).HasMatrixSameRowsAndColumns(new Matrix(2));
            Assert.AreEqual(false, ComparisonResult);
        }

        [Test]
        public void Matrix_Has_The_Same_Rows_And_Columns()
        {
            ComparisonResult = new Matrix(2, 2).HasMatrixSameRowsAndColumns(new Matrix(2));
            Assert.AreEqual(true, ComparisonResult);
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class Add_WithMantissaFive_TestFixture : Matrix_TestFixtureBase
    {
        protected Matrix MatrixToAdd { get; set; }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die zu addierende Matrix hat nicht die selben Zeilen und Spaltenanzahl!")]
        public void Should_Throw_Exception_When_RowNumber_And_ColumnNumber_Are_Not_The_Same()
        {
            new Matrix(3).Add(new Matrix(3, 4));
        }

        [Test]
        public void Should_Add_Matrix_Correctly()
        {
            Assert.AreEqual(2d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(8d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(32d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(1.9999d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(998e+30d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(111110d, OutputMatrix.GetDoubleFromMatrix(1, 2));
        }

        public override void PrepareContext()
        {
            Matrix.MantissaLength = 5;

            var arrayMatrix = new double[2, 3];
            arrayMatrix[0, 0] = 1d;
            arrayMatrix[0, 1] = 4d;
            arrayMatrix[0, 2] = 16d;

            arrayMatrix[1, 0] = 0.111119d;
            arrayMatrix[1, 1] = 1.111119d;
            arrayMatrix[1, 2] = 111111.9d;

            InputMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[2, 3];
            arrayMatrix[0, 0] = 1d;
            arrayMatrix[0, 1] = 4d;
            arrayMatrix[0, 2] = 16d;

            arrayMatrix[1, 0] = 1.88882d;
            arrayMatrix[1, 1] = 998e+30d;
            arrayMatrix[1, 2] = 0.8888d;

            MatrixToAdd = new Matrix(arrayMatrix);
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = InputMatrix.Add(MatrixToAdd);
        }
    }

    [TestFixture]
    public class Subtract_WithMantissaFive_TestFixture : Matrix_TestFixtureBase
    {
        protected Matrix MatrixToAdd { get; set; }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die zu subtrahierende Matrix hat nicht die selben Zeilen und Spaltenanzahl!")]
        public void Should_Throw_Exception_When_RowNumber_And_ColumnNumber_Are_Not_The_Same()
        {
            new Matrix(3).Subtract(new Matrix(3, 4));
        }

        [Test]
        public void Should_Subtract_Matrix_Correctly()
        {
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(0d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(19d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(48d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(27d, OutputMatrix.GetDoubleFromMatrix(1, 2));
        }

        public override void PrepareContext()
        {
            Matrix.MantissaLength = 5;

            var arrayMatrix = new double[2, 3];
            arrayMatrix[0, 0] = 1d;
            arrayMatrix[0, 1] = 4d;
            arrayMatrix[0, 2] = 16d;

            arrayMatrix[1, 0] = 20d;
            arrayMatrix[1, 1] = 50d;
            arrayMatrix[1, 2] = 30d;

            InputMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[2, 3];
            arrayMatrix[0, 0] = 1d;
            arrayMatrix[0, 1] = 4d;
            arrayMatrix[0, 2] = 16d;

            arrayMatrix[1, 0] = 1d;
            arrayMatrix[1, 1] = 2d;
            arrayMatrix[1, 2] = 3d;

            MatrixToAdd = new Matrix(arrayMatrix);
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = InputMatrix.Subtract(MatrixToAdd);
        }
    }

    [TestFixture]
    public class MultiplyByScalar_WithMantissaFive_TestFixture : Matrix_TestFixtureBase
    {
        [Test]
        public void Should_Multiply_With_Scalar_Correctly()
        {
            Assert.AreEqual(1000000d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(4000000d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(16000000d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(111120d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(1111100d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(111110000000d, OutputMatrix.GetDoubleFromMatrix(1, 2));
        }

        public override void PrepareContext()
        {
            Matrix.MantissaLength = 5;

            var arrayMatrix = new double[2, 3];
            arrayMatrix[0, 0] = 1d;
            arrayMatrix[0, 1] = 4d;
            arrayMatrix[0, 2] = 16d;

            arrayMatrix[1, 0] = 0.111119d;
            arrayMatrix[1, 1] = 1.111119d;
            arrayMatrix[1, 2] = 111111.9d;

            InputMatrix = new Matrix(arrayMatrix);
        }

        public override void CallMethodToTest()
        {
            OutputMatrix = InputMatrix.MultiplyByScalar(1000001);
        }
    }

    [TestFixture]
    public class Multiply_WithMantissaFive_TestFixture : Matrix_TestFixtureBase
    {
        protected Matrix FirstMatrix { get; set; }
        protected Matrix SecondMatrix { get; set; }

        // m x n = row & column, beware, in Matrix row and column are swapped
        [Test]
        public void FirstMatrix_Is_1x3_And_Second_Is_3x2()
        {
            var arrayMatrix = new double[3, 1];

            arrayMatrix[0, 0] = 1d;
            arrayMatrix[1, 0] = 5.555544d;
            arrayMatrix[2, 0] = 3d;

            FirstMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[2, 3];

            arrayMatrix[0, 0] = 5d;
            arrayMatrix[0, 1] = 0.0001d;
            arrayMatrix[0, 2] = 99E+10d;
            arrayMatrix[1, 0] = 6d;
            arrayMatrix[1, 1] = 9d;
            arrayMatrix[1, 2] = 33.3d;

            SecondMatrix = new Matrix(arrayMatrix);

            OutputMatrix = FirstMatrix.Multiply(SecondMatrix);

            Assert.AreEqual(2, OutputMatrix.MaxColumnCount);
            Assert.AreEqual(1, OutputMatrix.MaxRowCount);

            Assert.AreEqual(297E+10d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(155.9d, OutputMatrix.GetDoubleFromMatrix(1, 0));
        }

        [Test]
        public void FirstMatrix_Is_3x3_And_Second_Is_3x3()
        {
            var arrayMatrix = new double[3, 3];

            arrayMatrix[0, 0] = 5d;
            arrayMatrix[0, 1] = 5d;
            arrayMatrix[0, 2] = 5d;
            arrayMatrix[1, 0] = 5d;
            arrayMatrix[1, 1] = 5d;
            arrayMatrix[1, 2] = 5d;
            arrayMatrix[2, 0] = 5d;
            arrayMatrix[2, 1] = 5d;
            arrayMatrix[2, 2] = 5d;

            FirstMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[3, 3];

            arrayMatrix[0, 0] = 5d;
            arrayMatrix[0, 1] = 5d;
            arrayMatrix[0, 2] = 5d;
            arrayMatrix[1, 0] = 5d;
            arrayMatrix[1, 1] = 5d;
            arrayMatrix[1, 2] = 5d;
            arrayMatrix[2, 0] = 5d;
            arrayMatrix[2, 1] = 5d;
            arrayMatrix[2, 2] = 5d;

            SecondMatrix = new Matrix(arrayMatrix);

            OutputMatrix = FirstMatrix.Multiply(SecondMatrix);

            Assert.AreEqual(3, OutputMatrix.MaxColumnCount);
            Assert.AreEqual(3, OutputMatrix.MaxRowCount);

            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(0, 1));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(0, 2));

            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(1, 1));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(1, 2));

            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(2, 0));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(2, 1));
            Assert.AreEqual(75d, OutputMatrix.GetDoubleFromMatrix(2, 2));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die Spaltenanzahl der ersten Matrix muss genauso groß sein, wie die Zeilenanzahl der zweiten Matrix!")]
        public void FirstMatrix_ColumnNumber_Is_Not_Equal_To_SecondMatrix_RowNumber()
        {
            var arrayMatrix = new double[2, 3];

            arrayMatrix[0, 0] = 5d;
            arrayMatrix[0, 1] = 5d;
            arrayMatrix[0, 2] = 5d;
            arrayMatrix[1, 0] = 5d;
            arrayMatrix[1, 1] = 5d;
            arrayMatrix[1, 2] = 5d;

            FirstMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[3, 3];

            arrayMatrix[0, 0] = 5d;
            arrayMatrix[0, 1] = 5d;
            arrayMatrix[0, 2] = 5d;
            arrayMatrix[1, 0] = 5d;
            arrayMatrix[1, 1] = 5d;
            arrayMatrix[1, 2] = 5d;
            arrayMatrix[2, 0] = 5d;
            arrayMatrix[2, 1] = 5d;
            arrayMatrix[2, 2] = 5d;

            SecondMatrix = new Matrix(arrayMatrix);

            OutputMatrix = FirstMatrix.Multiply(SecondMatrix);
        }

        public override void TestFixtureTearDown()
        {
            FirstMatrix = null;
            SecondMatrix = null;

            OutputMatrix = null;
        }

        public override void PrepareContext()
        {
            Matrix.MantissaLength = 5;
        }

        public override void CallMethodToTest()
        {
        }
    }

    [TestFixture]
    public class LUPartition_WithMantissaFive_TestFixture : Matrix_TestFixtureBase
    {
        protected Matrix RegularQuadraticMatrix { get; set; }
        protected Matrix ResultVector { get; set; }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Der Ergebnisvektor hat nicht die Dimensionen eines Vektors!")]
        public void The_ResultVector_Is_Actually_Not_Vector()
        {
            var vector = new Matrix(2, 3);
            
            new Matrix(3).LUPartition(vector, false);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Für die LU-Zerteilung muss die Matrix quadratisch sein!")]
        public void The_Matrix_Is_Not_Quadratic()
        {
            var vector = new Matrix(1, 3);

            new Matrix(2, 3).LUPartition(vector, false);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(Exception), ExpectedMessage = "Die Matrix und der Vektor müssen die selbe Zeilenanzahl haben!")]
        public void The_ResultVector_Must_Have_The_Same_RowNumbers_Than_The_Matrix()
        {
            var vector = new Matrix(1, 3);

            new Matrix(4).LUPartition(vector, false);
        }

        [Test]
        public void Should_Get_The_Correct_Results_For_x()
        {
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

            RegularQuadraticMatrix = new Matrix(arrayMatrix);

            arrayMatrix = new double[3,1];

            arrayMatrix[0, 0] = 6.5d;
            arrayMatrix[1, 0] = -5.3d;
            arrayMatrix[2, 0] = 2.9d;

            ResultVector = new Matrix(arrayMatrix);

            OutputMatrix = RegularQuadraticMatrix.LUPartition(ResultVector, true);

            Assert.AreEqual(5.1905d, OutputMatrix.GetDoubleFromMatrix(0, 0));
            Assert.AreEqual(1.0990d, OutputMatrix.GetDoubleFromMatrix(1, 0));
            Assert.AreEqual(1.0990d, OutputMatrix.GetDoubleFromMatrix(2, 0));
        }

        public override void PrepareContext()
        {
        }

        public override void CallMethodToTest()
        {
        }
    }
}