namespace Core
{
    using System;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    public class StaticModelingOperations
    {
        /// <summary>
        /// Положительная транзитивно замкнутая матрица.
        /// </summary>
        /// <param name="fuzzyCognitiveMatrix"> НКМ. </param>
        /// <returns></returns>
        public Matrix PositiveTransitiveClousure(Matrix<double> fuzzyCognitiveMatrix)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Отрицательная транзитивно замкнутая матрица.
        /// </summary>
        /// <param name="fuzzyCognitiveMatrix"> НКМ. </param>
        /// <returns></returns>
        public Matrix NegativeTransitiveClousure(Matrix<double> fuzzyCognitiveMatrix)
        {
            throw new NotImplementedException();
        }
    }
}