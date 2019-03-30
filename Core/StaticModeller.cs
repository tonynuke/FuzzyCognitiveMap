namespace Core
{
    using System;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    public class StaticModeller
    {
        /// <summary>
        /// Рассчитать транзитивно замкнутую матрицу.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Транзитивно замкнутая матрица. </returns>
        public Matrix TransitiveClousure(Matrix<double> matrix)
        {
            if (matrix.ColumnCount != matrix.RowCount)
            {
                return null;
            }

            int rowsCount = matrix.RowCount;
            Matrix result = new DenseMatrix(rowsCount, rowsCount);
            //for (int index1 = 1; index1 <= rowsCount; ++index1)
            //{
            //    Matrix matrix = new Matrix(M1);
            //    for (int index2 = 1; index2 <= rowsCount; ++index2)
            //    {
            //        for (int index3 = 1; index3 <= rowsCount; ++index3)
            //            M1[index2, index3] = Value.S_Norm(matrix[index2, index3], Value.T_Norm(matrix[index2, index1], matrix[index1, index3]));
            //    }
            //}
            return result;
        }

        /// <summary>
        /// Рассчитать положительную транзитивно замкнутую матрицу.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Положительная транзитивно замкнутая матрица. </returns>
        public Matrix PositiveTransitiveClousure(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount / 2;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int index1 = 0; index1 < matrixSize; index1++)
            {
                int index2 = index1 * 2;
                for (int index3 = 0; index3 < matrixSize; index3++)
                {
                    int index4 = index3 * 2;
                    result[index1, index3] = Math.Max(matrix[index2 - 1, index4 - 1], matrix[index2, index4]);
                }
            }

            return result;
        }

        /// <summary>
        /// Рассчитать отрицательную транзитивно замкнутую матрицу.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Отрицательная транзитивно замкнутая матрица. </returns>
        public Matrix NegativeTransitiveClousure(Matrix<double> matrix)
        {
            throw new NotImplementedException();
        }

        public Matrix DoubleMatrix(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount * 2;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int index1 = 0; index1 < matrix.RowCount; index1++)
            {
                int index2 = 1;
                if (index1 >= 1)
                {
                    index2 = (index1 * 2) - 1;
                }

                for (int index3 = 0; index3 < matrix.RowCount; index3++)
                {
                    int index4 = 1;
                    if (index3 >= 1)
                    {
                        index4 = (index3 * 2) - 1;
                    }

                    double value = matrix[index1, index3];

                    if (value >= 0)
                    {
                        matrix[index2 - 1, index4 - 1] = value;
                        matrix[index2, index4] = value;
                    }
                    else
                    {
                        matrix[index2 - 1, index4] = value;
                        matrix[index2, index4 - 1] = value;
                    }
                }
            }

            return result;
        }
    }
}