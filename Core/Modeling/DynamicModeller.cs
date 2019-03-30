namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    public class DynamicModeller
    {
        /// <summary>
        /// Запустить моделирование.
        /// </summary>
        /// <param name="initialConceptsState"> Начальное состояние концептов. </param>
        /// <param name="conceptLinks"> Матрица связей между концептами. </param>
        /// <param name="steps"> Количество шагов моделирования. </param>
        /// <returns> Матрица состояний концептов. </returns>
        public List<Vector<double>> StartModelling(Vector<double> initialConceptsState, Matrix<double> conceptLinks, int steps)
        {
            List<Vector<double>> result = new List<Vector<double>> { initialConceptsState };

            for (int step = 0; step < steps; step++)
            {
                Vector<double> nextState = this.CalculateNextState(result.Last(), conceptLinks);
                result.Add(nextState);
            }

            return result;
        }

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

                nextState[i] = this.LogisticThresholdFunction(sum);
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

        /// <summary>
        /// Пороговая функция.
        /// </summary>
        /// <param name="value"> Значение. </param>
        /// <returns> Результат. </returns>
        protected double LogisticThresholdFunction(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -5 * value));
        }
    }
}