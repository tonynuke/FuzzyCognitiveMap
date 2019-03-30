namespace Core
{
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    public class DynamicModeller
    {
        /// <summary>
        /// Вычислить следующее состояние концептов.
        /// </summary>
        /// <param name="currentConceptsState"> Текущее состояние концептов. </param>
        /// <param name="conceptLinks"> Матрица связей между концептами. </param>
        /// <returns> Следующее состояние концептов. </returns>
        public Vector<double> CalculateNextState(Vector<double> currentConceptsState, Matrix<double> conceptLinks)
        {
            var conceptsCount = currentConceptsState.Count;
            var nextState = DenseVector.Build.Dense(conceptsCount);

            for (int i = 0; i < conceptsCount; i++)
            {
                var sum = currentConceptsState[i];

                for (int j = 0; j < conceptsCount; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    sum = sum + (currentConceptsState[j] * conceptLinks[j, i]);
                }

                nextState[i] = this.BivalentThresholdFunction(sum);
            }

            return nextState;
        }

        /// <summary>
        /// Пороговая функция.
        /// </summary>
        /// <param name="value"> Значение. </param>
        /// <returns> Результат. </returns>
        protected double BivalentThresholdFunction(double value)
        {
            return value > 0 ? 1 : 0;
        }
    }
}