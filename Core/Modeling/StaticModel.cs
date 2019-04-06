namespace Core.Modeling
{
    using System;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// Статическое можелирование.
    /// </summary>
    public class StaticModel
    {
        /// <summary>
        /// Рассчитать транзитивно замкнутую матрицу.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Транзитивно замкнутая матрица. </returns>
        public Matrix<double> TransitiveClousure(Matrix<double> matrix)
        {
            if (matrix.ColumnCount != matrix.RowCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount;

            Matrix result = new DenseMatrix(matrixSize, matrixSize);
            int i, j, k;

            for (i = 0; i < matrixSize; i++)
            {
                for (j = 0; j < matrixSize; j++)
                {
                    result[i, j] = matrix[i, j];
                }
            }

            for (k = 0; k < matrixSize; k++)
            {
                for (i = 0; i < matrixSize; i++)
                {
                    for (j = 0; j < matrixSize; j++)
                    {
                        result[i, j] = (result[i, j] != 0) ||
                                       ((result[i, k] != 0) &&
                                        (result[k, j] != 0))
                            ? 1
                            : 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить матрицу положительных связей.
        /// </summary>
        /// <param name="matrix"> Матрица влияний. </param>
        /// <returns> Матрица положительных связей. </returns>
        public Matrix<double> PositiveLinksMatrix(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount * 2;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrix.RowCount; i++)
            {
                int i2 = this.CalculateIndex(i);

                for (int j = 0; j < matrix.RowCount; j++)
                {
                    int j2 = this.CalculateIndex(j);

                    double value = matrix[i, j];

                    if (value > 0)
                    {
                        result[i2 - 1, j2 - 1] = value;
                        result[i2, j2] = value;
                    }
                    else
                    {
                        result[i2 - 1, j2] = -value;
                        result[i2, j2 - 1] = -value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить матрицу положительных пар.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Матрица положительных пар </returns>
        public Matrix<double> PositivePairMatrix(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount / 2;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                int i2 = this.CalculateIndex(i);
                for (int j = 0; j < matrixSize; j++)
                {
                    int j2 = this.CalculateIndex(j);
                    result[i, j] = Math.Max(matrix[i2 - 1, j2 - 1], matrix[i2, j2]);
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить матрицу отрицательных пар.
        /// </summary>
        /// <param name="matrix"> Матрица. </param>
        /// <returns> Матрица отрицательных пар </returns>
        public Matrix<double> NegativePairMatrix(Matrix<double> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                return null;
            }

            int matrixSize = matrix.RowCount / 2;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                int i2 = this.CalculateIndex(i);
                for (int j = 0; j < matrixSize; j++)
                {
                    int j2 = this.CalculateIndex(j);
                    result[i, j] = -Math.Max(matrix[i2 - 1, j2], matrix[i2, j2 - 1]);
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить консонанс.
        /// </summary>
        /// <param name="positivePairs"> Матрица положительных пар. </param>
        /// <param name="negativePairs"> Матрица отрицательных пар. </param>
        /// <returns> Матрица отрицательных пар </returns>
        public Matrix<double> ConsonanceMatrix(
            Matrix<double> positivePairs,
            Matrix<double> negativePairs)
        {
            var matrixSize = positivePairs.ColumnCount;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    var value = Math.Abs(positivePairs[i, j] + negativePairs[i, j]) /
                                (Math.Abs(positivePairs[i, j]) + Math.Abs(negativePairs[i, j]));

                    if (double.IsNaN(value))
                    {
                        value = 0;
                    }

                    result[i, j] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить дисонанс.
        /// </summary>
        /// <param name="consonanceMatrix"> Матрица консонанса. </param>
        /// <returns> Матрица отрицательных пар </returns>
        public Matrix<double> DissonanceMatrix(Matrix<double> consonanceMatrix)
        {
            var matrixSize = consonanceMatrix.ColumnCount;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    result[i, j] = 1 - consonanceMatrix[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить влияние.
        /// </summary>
        /// <param name="positivePairs"> Матрица положительных пар. </param>
        /// <param name="negativePairs"> Матрица отрицательных пар. </param>
        /// <returns> Матрица влияния. </returns>
        public Matrix<double> InfluenceMatrix(
            Matrix<double> positivePairs,
            Matrix<double> negativePairs)
        {
            var matrixSize = positivePairs.ColumnCount;
            Matrix result = new DenseMatrix(matrixSize, matrixSize);

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var sum = positivePairs[i, j] + negativePairs[i, j];
                    var max = Math.Max(Math.Abs(positivePairs[i, j]), Math.Abs(negativePairs[i, j]));
                    result[i, j] = Math.Sign(sum) * max;
                }
            }

            return result;
        }

        /// <summary>
        /// Вычислить влияние концептов на систему.
        /// </summary>
        /// <param name="matrix"> Консонанс. </param>
        /// <returns> Влияние концептов на систему. </returns>
        public Vector<double> ConceptInfluence(Matrix<double> matrix)
        {
            int vectorSize = matrix.ColumnCount;
            Vector<double> result = DenseVector.Build.Dense(vectorSize);

            for (int i = 0; i < vectorSize; i++)
            {
                result[i] = matrix.Row(i).Sum() / vectorSize;
            }

            return result;
        }

        /// <summary>
        /// Вычислить вероятность наступления неблагоприятного события.
        /// </summary>
        /// <param name="criticality"> Критичность уязвимости. </param>
        /// <param name="probability"> Вероятность возникновения неблагоприятного события. </param>
        /// <returns> Вероятность наступления неблагоприятного события. </returns>
        public double CalculateProbability(Vector<double> criticality, Vector<double> probability)
        {
            double result = 0;

            for (int k = 0; k < criticality.Count; k++)
            {
                for (int l = 0; l < probability.Count; l++)
                {
                    result += criticality[k] * probability[l];
                }
            }

            return result / criticality.Count;
        }

        /// <summary>
        /// Вычислить ущерб.
        /// </summary>
        /// <param name="probability"> Вероятность наступления неблагоприятного события. </param>
        /// <param name="influence"> Влияние концептов на систему. </param>
        /// <returns> Ущерб. </returns>
        public double CalculateDamage(double probability, Vector<double> influence)
        {
            return probability * influence.Sum();
        }

        /// <summary>
        /// Вычислить индекс с учетом поправки.
        /// </summary>
        /// <param name="sourceIndex"> Исходный индекс. </param>
        /// <returns> Индекс с учетом поправки. </returns>
        private int CalculateIndex(int sourceIndex)
        {
            return sourceIndex == 0 ? 1 : (sourceIndex * 2) + 1;
        }
    }
}