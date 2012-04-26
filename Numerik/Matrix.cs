using System;
using System.Collections.Generic;
using System.Globalization;

namespace Numerik
{
    public class Matrix : ICloneable
    {
        // column, row ==> double value
        private Dictionary<Tuple<int, int>, double> m_Matrix;

        private readonly int m_MaxColumnCount;
        private readonly int m_MaxRowCount;

        public static int MantissaLength = Mantissen.MaxDecimalPlaces;

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

                        valueToSet += result;
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
        //x = Wird duch die LU-Zerteilung zurückgegeben
        public Matrix LUPartition(Matrix resultVector, bool useRowPivotStrategy)
        {
            throw new Exception();
        }

        //public Matrix Transponierend

        //public Matrix Normalize

        //public Matrix Inverse
        //public Matrix Determinante   --> Indikator für eine reguläre Matrix, wenn Inverse existiert Determinante != 0 und Rang = n
        //public Matrix Rang

        //Spektralnorm
        //Zeilensummennorm
        //Maximumnorm
        
        //public int Mantisse (für Rundungen)

        public bool HasMatrixSameRowsAndColumns(Matrix matrixToCompare)
        {
            return m_MaxColumnCount == matrixToCompare.MaxColumnCount && m_MaxRowCount == matrixToCompare.MaxRowCount;
        }

        public bool IsAVector(Matrix matrix)
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
            if (m_MaxRowCount != rowArray.Length)
            {
                throw new Exception("Die Reihenanzahl der Matrix und die Länge des Arrays müssen gleich sein!");
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
            if (m_MaxColumnCount != columnArray.Length)
            {
                throw new Exception("Die Splatenanzahl der Matrix und die Länge des Arrays müssen gleich sein!");
            }

            Matrix clonedMatrix = CloneMatrix();

            for (int i = 0; i < columnArray.Length; i++)
            {
                clonedMatrix.SetDoubleOfMatrix(columnArray[i], columnNumber, i);
            }

            return clonedMatrix;
        }

        /* Wendet die Zeilenpivotstrategy innerhalb einer bestimmten Zeilengrenze an für eine bestimmte Spalte*/
        private Matrix RowPivotStrategy(int minRow, int maxRow, int columnToLookAt)
        {
            double maxValue = GetDoubleFromMatrix(columnToLookAt, minRow);
            int rowNumber = minRow;

            for (int row = minRow; row < (maxRow + 1); row++)
            {
                double tempValue = GetDoubleFromMatrix(columnToLookAt, row);
                
                if (maxValue < tempValue)
                {
                    maxValue = tempValue;
                    rowNumber = row;
                }
            }

            return SwapRows(minRow, rowNumber);
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
