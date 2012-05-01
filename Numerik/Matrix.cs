using System;
using System.Collections.Generic;
using System.Globalization;
using MathNetLAGen = MathNet.Numerics.LinearAlgebra.Generic;
using MathNetLA = MathNet.Numerics.LinearAlgebra;

namespace Numerik
{
    public class Matrix : ICloneable
    {
        // column, row ==> double value
        private Dictionary<Tuple<int, int>, double> m_Matrix;

        private int m_MaxColumnCount;
        private int m_MaxRowCount;

        public static int MantissaLength = Mantissen.MaxDecimalPlaces;

        public Matrix(Matrix newMatrixToSet) : this(newMatrixToSet.ToArray())
        {
        }

        /* create quadratic matrix */
        public Matrix(int dimension) : this(dimension, dimension)
        {
        }

        /* initialize with 0d */
        public Matrix(int columnCount, int rowCount)
        {
            m_Matrix = new Dictionary<Tuple<int, int>, double>();

            m_MaxColumnCount = columnCount;
            m_MaxRowCount = rowCount;

            for (int column = 0; column < m_MaxColumnCount; column++)
            {
                for (int row = 0; row < m_MaxRowCount; row++)
                {
                    m_Matrix[Tuple.Create(column, row)] = 0d;
                }
            }
        }

        /* column-row arrayMatrix for initialisation */
        public Matrix(double[,] arrayMatrix)
        {
            m_Matrix = new Dictionary<Tuple<int, int>, double>();

            m_MaxColumnCount = arrayMatrix.GetLength(0);
            m_MaxRowCount = arrayMatrix.GetLength(1);

            for (int column = 0; column < m_MaxColumnCount; column++)
            {
                for (int row = 0; row < m_MaxRowCount; row++)
                {
                    m_Matrix[Tuple.Create(column, row)] = arrayMatrix[column, row];
                }
            }
        }

        public static Matrix CreateIdentityMatrix(int dimension)
        {
            var matrix = new Matrix(dimension, dimension);

            for (int row = 0; row < dimension; row++)
            {
                for (int column = 0; column < dimension; column++)
                {
                    matrix.SetDoubleOfMatrix(1d, column, row);
                    row += 1;
                }
            }

            return matrix;
        }

        public int MaxColumnCount
        {
            get { return m_MaxColumnCount; }
        }

        public int MaxRowCount
        {
            get { return m_MaxRowCount; }
        }

        public double GetDoubleFromMatrix(int column, int row)
        {
            ValidateColumnAndRowNumber(column, row);

            return m_Matrix[Tuple.Create(column, row)];
        }

        public void SetDoubleOfMatrix(double valueToSet, int column, int row)
        {
            ValidateColumnAndRowNumber(column, row);

            m_Matrix[Tuple.Create(column, row)] = valueToSet;
        }

        public void SetFromNewMatrix(Matrix newMatrixToSet)
        {
            var tempMatrix = new Matrix(newMatrixToSet.ToArray());

            m_Matrix = tempMatrix.m_Matrix;

            m_MaxColumnCount = tempMatrix.m_MaxColumnCount;
            m_MaxRowCount = tempMatrix.m_MaxRowCount;
        }

        public Matrix GetIdentityMatrix()
        {
            if (!IsMatrixQuadratic())
            {
                throw new Exception("Die Identitätsmatrix muss quadratisch sein!");
            }

            var matrix = new Matrix(m_MaxColumnCount, m_MaxRowCount);

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    matrix.SetDoubleOfMatrix(1d, column, row);
                    row += 1;
                }
            }

            return matrix;
        }

        public Matrix SwapRows(int firstRowNumber, int secondRowNumber)
        {
            double[] firstRow = GetRowFromMatrixAsArray(firstRowNumber);
            double[] secondRow = GetRowFromMatrixAsArray(secondRowNumber);

            Matrix matrixFirstRowSwapped = SetRowFromMatrix(firstRow, secondRowNumber);
            Matrix matrixSecondRowSwapped = matrixFirstRowSwapped.SetRowFromMatrix(secondRow, firstRowNumber);

            return matrixSecondRowSwapped;
        }

        public Matrix SwapColumns(int firstColumnNumber, int secondColumnNumber)
        {
            double[] firstColumn = GetColumnFromMatrixAsArray(firstColumnNumber);
            double[] secondColumn = GetColumnFromMatrixAsArray(secondColumnNumber);

            Matrix matrixFirstColumnSwapped = SetColumnFromMatrix(firstColumn, secondColumnNumber);
            Matrix matrixSecondColumnSwapped = matrixFirstColumnSwapped.SetColumnFromMatrix(secondColumn, firstColumnNumber);

            return matrixSecondColumnSwapped;
        }

        public bool IsMatrixQuadratic()
        {
            return m_MaxColumnCount == m_MaxRowCount;
        }

        public Matrix Add(Matrix matrixToAdd)
        {
            if (!HasMatrixSameRowsAndColumns(matrixToAdd))
            {
                throw new Exception("Die zu addierende Matrix hat nicht die selben Zeilen und Spaltenanzahl!");
            }

            var matrix = new Matrix(m_MaxColumnCount, m_MaxRowCount);

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    double doubleOfFirstMatrix = Mantissen.RoundToMaxMantissaLength(
                        MantissaLength,
                        GetDoubleFromMatrix(column, row));

                    double doubleOfSecondMatrix = Mantissen.RoundToMaxMantissaLength(
                        MantissaLength,
                        matrixToAdd.GetDoubleFromMatrix(column, row));
                    
                    matrix.SetDoubleOfMatrix(Mantissen.RoundToMaxMantissaLength(MantissaLength, doubleOfFirstMatrix + doubleOfSecondMatrix), column, row);
                }
            }

            return matrix;
        }

        public Matrix Subtract(Matrix matrixToSubtract)
        {
            if (!HasMatrixSameRowsAndColumns(matrixToSubtract))
            {
                throw new Exception("Die zu subtrahierende Matrix hat nicht die selben Zeilen und Spaltenanzahl!");
            }

            var matrix = new Matrix(m_MaxColumnCount, m_MaxRowCount);

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    matrix.SetDoubleOfMatrix(-matrixToSubtract.GetDoubleFromMatrix(column, row), column, row);
                }
            }

            return Add(matrix);
        }

        public Matrix MultiplyByScalar(double scalar)
        {
            var matrix = new Matrix(m_MaxColumnCount, m_MaxRowCount);

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    double doubleOfMatrix = Mantissen.RoundToMaxMantissaLength(
                        MantissaLength,
                        GetDoubleFromMatrix(column, row));

                    double roundedScalar = Mantissen.RoundToMaxMantissaLength(MantissaLength, scalar);

                    matrix.SetDoubleOfMatrix(Mantissen.RoundToMaxMantissaLength(MantissaLength, doubleOfMatrix * roundedScalar), column, row);
                }
            }

            return matrix;
        }

        public Matrix Multiply(Matrix matrixToMultiply)
        {
            if (MaxColumnCount != matrixToMultiply.MaxRowCount)
            {
                throw new Exception("Die Spaltenanzahl der ersten Matrix muss genauso groß sein, wie die Zeilenanzahl der zweiten Matrix!");
            }

            var multipliedMatrix = new Matrix(matrixToMultiply.MaxColumnCount, m_MaxRowCount);
            double valueToSet = 0d;

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < matrixToMultiply.m_MaxColumnCount; column++)
                {
                    for (int columnAndRow = 0; columnAndRow < MaxColumnCount; columnAndRow++)
                    {
                        double columnCell = Mantissen.RoundToMaxMantissaLength(MantissaLength, GetDoubleFromMatrix(columnAndRow, row));
                        double rowCell = Mantissen.RoundToMaxMantissaLength(MantissaLength, matrixToMultiply.GetDoubleFromMatrix(column, columnAndRow));
                        double result = Mantissen.RoundToMaxMantissaLength(MantissaLength, columnCell*rowCell);

                        valueToSet = (valueToSet + result).RoundMantissa(MantissaLength);
                    }

                    multipliedMatrix.SetDoubleOfMatrix(Mantissen.RoundToMaxMantissaLength(MantissaLength, valueToSet), column, row);
                    valueToSet = 0d;
                }
            }

            return multipliedMatrix;
        }

        //Ax = b
        //A = reguläre, quadratische Matrix
        //b = Ergebnisvektor
        //x = Wird duch die LU-Zerteilung zurückgegeben Ly = b; Ux = y
        //U = Dreicksmatrix
        //KeyValuePair "L" --> L, "U" --> U, "y" --> y, "x" --> x
        //resultVector 
        public Dictionary<string, Matrix> LUPartition(
            Matrix resultVectorAsB,
            bool useRowPivotStrategy)
        {
            Matrix resultVector = resultVectorAsB.CloneMatrix();

            if (!resultVector.IsAVector())
            {
                throw new Exception("Der Ergebnisvektor hat nicht die Dimensionen eines Vektors!");
            }

            if (!IsMatrixQuadratic())
            {
                throw new Exception("Für die LU-Zerteilung muss die Matrix quadratisch sein!");
            }

            if (MaxColumnCount != resultVector.MaxRowCount)
            {
                throw new Exception("Die Matrix und der Vektor müssen die selbe Zeilenanzahl haben!");
            }

            var matrices = new Dictionary<string, Matrix>();

            Matrix tempMatrix = CloneMatrix();
            
            Matrix matrixL = GetIdentityMatrix();

            for (int column = 0; column < m_MaxColumnCount - 1; column++)
            {
                if (useRowPivotStrategy)
                {
                    tempMatrix = tempMatrix.RowPivotStrategy(resultVector, column, m_MaxRowCount, column);
                }

                for (int row = column + 1; row < m_MaxRowCount; row++)
                {
                    Matrix firstRowOfMatrix = tempMatrix.GetRowFromMatrixAsMatrix(column);
                    Matrix secondRowOfMatrix = tempMatrix.GetRowFromMatrixAsMatrix(row);

                    double valueOfFirstCellInRow = Mantissen.RoundToMaxMantissaLength(MantissaLength, tempMatrix.GetDoubleFromMatrix(column, column));
                    double valueOfSecondCellInRow = Mantissen.RoundToMaxMantissaLength(MantissaLength, tempMatrix.GetDoubleFromMatrix(column, row));

                    //Lässt Nullen genau, da bei großen Differenzen 34123e+30 und 0.000005 ein großer fehlerwert entstehen kann
                    if (valueOfSecondCellInRow.EqualsZero(MantissaLength))
                    {
                        continue;
                    }

                    double unknownValueToGetZero = Mantissen.RoundToMaxMantissaLength(MantissaLength, -(valueOfSecondCellInRow/valueOfFirstCellInRow));

                    firstRowOfMatrix = firstRowOfMatrix.MultiplyByScalar(unknownValueToGetZero);

                    matrixL.SetDoubleOfMatrix(-unknownValueToGetZero, column, row);

                    double[] resultingRowAdding = firstRowOfMatrix.Add(secondRowOfMatrix).GetRowFromMatrixAsArray(0);

                    tempMatrix = tempMatrix.SetRowFromMatrix(resultingRowAdding, row);

                    Console.WriteLine(tempMatrix);
                }
            }

            Matrix matrixU = tempMatrix;

            Matrix vectory = matrixL.SolveLowerTriangularMatrix(resultVector);
            Matrix vectorx = tempMatrix.SolveUpperTriangularMatrix(vectory);

            matrices["L"] = matrixL;
            matrices["U"] = matrixU;
            matrices["y"] = vectory;
            matrices["x"] = vectorx;

            return matrices;
        }

        public Matrix SolveUpperTriangularMatrix(Matrix resultVector)
        {
            var resultMatrix = new Matrix(1, resultVector.m_MaxRowCount);

            int rowCounter = 0;

            for (int row = (m_MaxRowCount - 1); row > -1; row--)
            {
                double valueOfCurrentresultVectorCell = Mantissen.RoundToMaxMantissaLength(MantissaLength, resultVector.GetDoubleFromMatrix(0, row));
                double sumOfCurrentMatrixRow = 0;

                double quotientToDivideSum = GetDoubleFromMatrix(row, m_MaxColumnCount - 1 - rowCounter);

                for (int column = (m_MaxColumnCount - 1); column > (m_MaxColumnCount - 1 - rowCounter); column--)
                {
                    double valueOfCurrentCellOfMatrix = Mantissen.RoundToMaxMantissaLength(MantissaLength, GetDoubleFromMatrix(column, row));
                    double valueOfVariableCorrespondingToCell = Mantissen.RoundToMaxMantissaLength(MantissaLength, resultMatrix.GetDoubleFromMatrix(0, column));
                    double productOfCellAndVariable = Mantissen.RoundToMaxMantissaLength(MantissaLength, valueOfCurrentCellOfMatrix * valueOfVariableCorrespondingToCell);

                    sumOfCurrentMatrixRow = Mantissen.RoundToMaxMantissaLength(MantissaLength, sumOfCurrentMatrixRow + productOfCellAndVariable);
                }

                valueOfCurrentresultVectorCell = Mantissen.RoundToMaxMantissaLength(MantissaLength,
                                                                                    valueOfCurrentresultVectorCell -
                                                                                    sumOfCurrentMatrixRow);

                //Saving one Result of the Equation
                double resultOfOneVariable = Mantissen.RoundToMaxMantissaLength(MantissaLength,
                                                                                valueOfCurrentresultVectorCell /
                                                                                quotientToDivideSum);

                resultMatrix.SetDoubleOfMatrix(resultOfOneVariable, 0, row);

                rowCounter++;
            }

            return resultMatrix;
        }

        public Matrix SolveLowerTriangularMatrix(Matrix resultVector)
        {
            Matrix tempResultVector = resultVector.TurningUpSideDown();
            Matrix tempMatrix = MirrorMatrixOnDiagonal();

            Matrix resultMatrix = tempMatrix.SolveUpperTriangularMatrix(tempResultVector);
            return resultMatrix.TurningUpSideDown();
        }

        public Matrix CreateLowerTriangularMatrix(bool useRowPivotStrategy)
        {
            Matrix tempMatrix = CloneMatrix();

            for (int column = 0; column < m_MaxColumnCount - 1; column++)
            {
                if (useRowPivotStrategy)
                {
                    tempMatrix = tempMatrix.RowPivotStrategy(column, m_MaxRowCount, column);
                }

                for (int row = column + 1; row < m_MaxRowCount; row++)
                {
                    Matrix firstRowOfMatrix = tempMatrix.GetRowFromMatrixAsMatrix(column);
                    Matrix secondRowOfMatrix = tempMatrix.GetRowFromMatrixAsMatrix(row);

                    double valueOfFirstCellInRow = Mantissen.RoundToMaxMantissaLength(MantissaLength, tempMatrix.GetDoubleFromMatrix(column, column));
                    double valueOfSecondCellInRow = Mantissen.RoundToMaxMantissaLength(MantissaLength, tempMatrix.GetDoubleFromMatrix(column, row));

                    //Lässt Nullen genau, da bei großen Differenzen 34123e+30 und 0.000005 ein großer fehlerwert entstehen kann
                    if (valueOfSecondCellInRow.EqualsZero(MantissaLength))
                    {
                        continue;
                    }

                    double unknownValueToGetZero = Mantissen.RoundToMaxMantissaLength(MantissaLength, -(valueOfSecondCellInRow / valueOfFirstCellInRow));

                    firstRowOfMatrix = firstRowOfMatrix.MultiplyByScalar(unknownValueToGetZero);

                    double[] resultingRowAdding = firstRowOfMatrix.Add(secondRowOfMatrix).GetRowFromMatrixAsArray(0);
                    resultingRowAdding[column] = 0d;

                    tempMatrix = tempMatrix.SetRowFromMatrix(resultingRowAdding, row);
                }
            }

            return tempMatrix;
        }

        public Matrix Inverse()
        {
            if (!IsMatrixQuadratic())
            {
                throw new Exception("Für eine Inverse Matrix muss diese quadratisch sein!");
            }

            //Math.net Library
            MathNetLAGen.Matrix<double> netMatrix = new MathNetLA.Double.DenseMatrix(ToArray());
            var inversedMatrix = new Matrix(netMatrix.Inverse().ToArray());
            return inversedMatrix;
        }

        public double Determinant()
        {
            if (!IsMatrixQuadratic())
            {
                throw new Exception("Für die Berechnung der Determinante muss die Matrix quadratisch sein!");
            }

            //Math.net Library
            MathNetLAGen.Matrix<double> netMatrix = new MathNetLA.Double.DenseMatrix(ToArray());
            return netMatrix.Determinant();
        }

        public Matrix Transponing()
        {
            var transponedMatrix = new Matrix(m_MaxRowCount, m_MaxColumnCount);

            for (int column = 0; column < m_MaxColumnCount; column++)
            {
                var collectedCellsOfRows = new double[m_MaxRowCount];

                for (int row = 0; row < m_MaxRowCount; row++)
                {
                    collectedCellsOfRows[row] = GetDoubleFromMatrix(column, row);
                }

                transponedMatrix = transponedMatrix.SetRowFromMatrix(collectedCellsOfRows, column);
            }

            return transponedMatrix;
        }

        public int GetRankOfMatrix()
        {
            int dimensionOfMatrix = Math.Max(m_MaxColumnCount, m_MaxRowCount);
            var resultMatrix = new Matrix(dimensionOfMatrix);
            var tempCopy = CloneMatrix();

            if (m_MaxColumnCount > m_MaxRowCount)
            {
                //rang(A) = rang(A-Transponierend)
                tempCopy = tempCopy.Transponing();
            }
            if (m_MaxColumnCount != m_MaxRowCount)
            {
                for (int row = 0; row < tempCopy.m_MaxRowCount; row++)
                {
                    for (int column = 0; column < tempCopy.m_MaxColumnCount; column++)
                    {
                        resultMatrix.SetDoubleOfMatrix(tempCopy.GetDoubleFromMatrix(column, row), column, row);
                    }
                }
            }
            else
            {
                resultMatrix = tempCopy.CloneMatrix();
            }

            resultMatrix = resultMatrix.CreateLowerTriangularMatrix(true);

            int rankOfMatrix = dimensionOfMatrix;

            for (int row = 0; row < resultMatrix.m_MaxRowCount; row++)
            {
                bool hasRowJustZeros = true;

                for (int column = 0; column < resultMatrix.m_MaxColumnCount; column++)
                {
                    hasRowJustZeros &= resultMatrix.GetDoubleFromMatrix(column, row).EqualsZero(MantissaLength);
                }

                if (hasRowJustZeros)
                {
                    rankOfMatrix--;
                }
            }

            return rankOfMatrix;
        }

        public bool IsMatrixRegular()
        {
            int maxRankPossible = Math.Max(m_MaxColumnCount, m_MaxRowCount);

            return maxRankPossible == GetRankOfMatrix() && GetIdentityMatrix().Equals(Multiply(Inverse()));
        }

        public double RowSumNorm()
        {
            double maxValue = 0d;

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                double tempValue = 0d;

                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    tempValue = tempValue.RoundAdd(Math.Abs(GetDoubleFromMatrix(column, row)), MantissaLength);
                }

                if (maxValue < tempValue)
                {
                    maxValue = tempValue;
                }
            }

            return maxValue;
        }

        public double MaxNormOfAVector()
        {
            if (!IsAVector())
            {
                throw new Exception("Für die Maximumnorm muss ein Vektor benutzt werden.!");
            }

            double maxValue = 0d;

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                double tempValue = 0d;

                tempValue = tempValue.RoundAdd(Math.Abs(GetDoubleFromMatrix(0, row)), MantissaLength);

                if (maxValue < tempValue)
                {
                    maxValue = tempValue;
                }
            }

            return maxValue;
        }

        // = Kappa(Matrix)
        public double ConditionNumberByRowSumNorm()
        {
            if (!IsMatrixQuadratic())
            {
                throw new Exception("Für die Konditionszahl durch Zeilensummennorm muss die Matrix quadratisch sein!");
            }

            return RowSumNorm()*Inverse().RowSumNorm().RoundMantissa(MantissaLength);
        }

        public Matrix ResidualVector(Matrix resultVector)
        {
            Matrix x = LUPartition(resultVector, true)["x"];

            Mantissen.Active = false;

            Matrix residualVector = Multiply(x).Subtract(resultVector);

            Mantissen.Active = true;

            return residualVector;
        }

        /* Es werden von jeder Zeiler der Matrix das absolute summiert und von jeder Zelle in */
        /* der Zeile abgezogen! Der Ergebnisvektor wird dabei nicht mit summiert.*/
        /* r = Ax - b*/
        public Matrix NormalizeByRowSum(Matrix resultVector, double valueForEachSumOfRow)
        {
            var tempMatrix = new Matrix(m_MaxColumnCount);

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                double tempValue = GetAbsSumOfARow(row);

                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    tempMatrix.SetDoubleOfMatrix(GetDoubleFromMatrix(column, row).RoundDivide(tempValue, MantissaLength), column, row);
                }

                resultVector.SetDoubleOfMatrix(resultVector.GetDoubleFromMatrix(0, row).RoundDivide(tempValue, MantissaLength), 0, row);
            }

            return tempMatrix;
        }

        /* ||deltax|| / ||x|| = error x wird durch LU-Partition berechnet*/
        public double ActualErrorWithRowSumNorm(Matrix resultVector, bool useRowPivotStrategy)
        {
            Matrix x_approx = LUPartition(resultVector, useRowPivotStrategy)["x"];

            Mantissen.Active = false;

            Matrix x_exact = LUPartition(resultVector, true)["x"];

            Mantissen.Active = true;

            x_exact = x_exact.RoundCurrentMatrixToMantissaLength();

            Matrix deltax = x_exact.Subtract(x_approx);

            return deltax.MaxNormOfAVector().RoundDivide(x_exact.MaxNormOfAVector(), MantissaLength);
        }

        //Spektralnorm

        public bool HasMatrixSameRowsAndColumns(Matrix matrixToCompare)
        {
            return m_MaxColumnCount == matrixToCompare.MaxColumnCount && m_MaxRowCount == matrixToCompare.MaxRowCount;
        }

        public bool IsAVector()
        {
            return m_MaxColumnCount == 1;
        }

        public double[,] ToArray()
        {
            var arrayMatrix = new double[m_MaxColumnCount, m_MaxRowCount];

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    arrayMatrix[column, row] = m_Matrix[Tuple.Create(column, row)];
                }
            }

            return arrayMatrix;
        }

        public override string ToString()
        {
            string matrixString = string.Empty;

            var nfi = new NumberFormatInfo
                        {
                            NumberDecimalSeparator = ".",
                            NumberGroupSeparator = string.Empty
                        };

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    matrixString += m_Matrix[Tuple.Create(column, row)].ToString(nfi).PadLeft(20);
                }

                matrixString += Environment.NewLine;
            }

            return matrixString;
        }

        public Matrix CloneMatrix()
        {
            return Clone() as Matrix;
        }
        
        /* Using ToArray For Cloning */
        public object Clone()
        {
            return new Matrix(ToArray());
        }

        //Difference is Mantissa 10!!!
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
            {
                return false;
            }

            var objMatrix = obj as Matrix;

            if (!HasMatrixSameRowsAndColumns(objMatrix))
            {
                return false;
            }

            bool cellComparision = true;

            for (int column = 0; column < m_MaxColumnCount; column++)
            {
                for (int row = 0; row < m_MaxRowCount; row++)
                {
                    cellComparision &= (GetDoubleFromMatrix(column, row) - objMatrix.GetDoubleFromMatrix(column, row)).EqualsZero(10);
                }
            }

            return cellComparision;
        }

        protected Matrix GetRowFromMatrixAsMatrix(int rowNumber)
        {
            var matrixRowArray = new double[m_MaxColumnCount, 1];
            double[] tempArray = GetRowFromMatrixAsArray(rowNumber);

            for(int cell = 0; cell < m_MaxColumnCount; cell++)
            {
                matrixRowArray[cell, 0] = tempArray[cell];
            }

            return new Matrix(matrixRowArray);
        }

        protected double[] GetRowFromMatrixAsArray(int rowNumber)
        {
            var rowFromMatrix = new double[m_MaxColumnCount];

            for(int i = 0; i < m_MaxColumnCount; i++)
            {
                rowFromMatrix[i] = GetDoubleFromMatrix(i, rowNumber);
            }

            return rowFromMatrix;
        }

        protected double[] GetColumnFromMatrixAsArray(int columnNumber)
        {
            var columnFromMatrix = new double[m_MaxRowCount];

            for (int i = 0; i < m_MaxRowCount; i++)
            {
                columnFromMatrix[i] = GetDoubleFromMatrix(columnNumber, i);
            }

            return columnFromMatrix;
        }

        /* Returns Copy Of Matrix, Where Row was set */
        protected Matrix SetRowFromMatrix(double[] rowArray, int rowNumber)
        {
            if (m_MaxColumnCount != rowArray.Length)
            {
                throw new Exception("Die Spaltenanzahl der Matrix und die Länge des Arrays müssen gleich sein!");
            }
            
            Matrix clonedMatrix = CloneMatrix();

            for (int i = 0; i < rowArray.Length; i++)
            {
                clonedMatrix.SetDoubleOfMatrix(rowArray[i], i, rowNumber);
            }

            return clonedMatrix;
        }

        /* Returns Copy Of Matrix, Where Column was set */
        protected Matrix SetColumnFromMatrix(double[] columnArray, int columnNumber)
        {
            if (m_MaxRowCount != columnArray.Length)
            {
                throw new Exception("Die Reihenanzahl der Matrix und die Länge des Arrays müssen gleich sein!");
            }

            Matrix clonedMatrix = CloneMatrix();

            for (int i = 0; i < columnArray.Length; i++)
            {
                clonedMatrix.SetDoubleOfMatrix(columnArray[i], columnNumber, i);
            }

            return clonedMatrix;
        }

        private Matrix TurningUpSideDown()
        {
            Matrix tempMatrix = CloneMatrix();

            for (int row = 0; row < (m_MaxRowCount / 2); row++)
            {
                tempMatrix = SwapRows(row, m_MaxRowCount - 1 - row);
            }

            return tempMatrix;
        }

        private Matrix ReverseLeftToRight()
        {
            Matrix tempMatrix = CloneMatrix();

            for (int column = 0; column < (m_MaxColumnCount / 2); column++)
            {
                tempMatrix = SwapColumns(column, m_MaxColumnCount - 1 - column);
            }

            return tempMatrix;
        }

        // Stellt die Matrix auf dem Kopf
        private Matrix MirrorMatrixOnDiagonal()
        {
            Matrix tempMatrix = CloneMatrix();

            tempMatrix = tempMatrix.TurningUpSideDown();
            tempMatrix = tempMatrix.ReverseLeftToRight();

            return tempMatrix;
        }

        private Matrix RowPivotStrategy(int minRow, int maxRow, int columnToLookAt)
        {
            double maxValue = GetDoubleFromMatrix(columnToLookAt, minRow);
            int rowNumber = minRow;

            for (int row = minRow; row < maxRow; row++)
            {
                double tempValue = Math.Abs(GetDoubleFromMatrix(columnToLookAt, row));

                if (maxValue < tempValue)
                {
                    maxValue = tempValue;
                    rowNumber = row;
                }
            }

            return SwapRows(minRow, rowNumber);
        }

        /* Wendet die Zeilenpivotstrategie innerhalb einer bestimmten Zeilengrenze an für eine bestimmte Spalte*/
        private Matrix RowPivotStrategy(Matrix resultVector, int minRow, int maxRow, int columnToLookAt)
        {
            double maxValue = GetDoubleFromMatrix(columnToLookAt, minRow);
            int rowNumber = minRow;

            for (int row = minRow; row < maxRow; row++)
            {
                double tempValue = Math.Abs(GetDoubleFromMatrix(columnToLookAt, row));
                
                if (maxValue < tempValue)
                {
                    maxValue = tempValue;
                    rowNumber = row;
                }
            }

            //swapping rows of result vector
            resultVector.SetFromNewMatrix(resultVector.SwapRows(minRow, rowNumber));

            return SwapRows(minRow, rowNumber);
        }

        private Matrix RoundCurrentMatrixToMantissaLength()
        {
            Matrix tempMatrix = CloneMatrix();

            for (int row = 0; row < m_MaxRowCount; row++)
            {
                for (int column = 0; column < m_MaxColumnCount; column++)
                {
                    tempMatrix.SetDoubleOfMatrix(GetDoubleFromMatrix(column, row).RoundMantissa(MantissaLength), row, column);
                }
            }

            return tempMatrix;
        }

        private double GetAbsSumOfARow(int row)
        {
            double tempValue = 0d;

            for (int column = 0; column < m_MaxColumnCount; column++)
            {
                tempValue = tempValue.RoundAdd(Math.Abs(GetDoubleFromMatrix(column, row)), MantissaLength);
            }

            return tempValue;
        }

        private void ValidateColumnAndRowNumber(int columnNumber, int rowNumber)
        {
            if (!IsColumnNumberValid(columnNumber) && !IsRowNumberValid(rowNumber))
            {
                throw new Exception("Spalte oder Zeilenangabe existiert nicht!");
            }
        }

        private bool IsColumnNumberValid(int columnNumber)
        {
            return columnNumber > -1 && columnNumber < m_MaxColumnCount;
        }

        private bool IsRowNumberValid(int rowNumber)
        {
            return rowNumber > -1 && rowNumber < m_MaxRowCount;
        }
    }
}