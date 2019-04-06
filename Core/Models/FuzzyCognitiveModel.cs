namespace Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Modeling;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    public class FuzzyCognitiveModel
    {
        /// <summary>
        /// НКК.
        /// </summary>
        public FuzzyCognitiveMap FuzzyCognitiveMap = new FuzzyCognitiveMap();

        /// <summary>
        /// Динамическая модель.
        /// </summary>
        private DynamicModel dynamicModel = new DynamicModel();

        /// <summary>
        /// Статическая модель.
        /// </summary>
        private StaticModel staticModel = new StaticModel();

        public Matrix<double> TransitiveClosure { get; private set; }

        public Matrix<double> PositiveLinks { get; private set; }

        public Matrix<double> PositivePairs { get; private set; }

        public Matrix<double> NegativePairs { get; private set; }

        public Matrix<double> Consonance { get; private set; }

        public Matrix<double> Dissonance { get; private set; }

        public Matrix<double> Influence { get; private set; }

        /// <summary>
        /// Запустить статическое моделирование.
        /// </summary>
        public void StartStaticicModeling()
        {
            var weights = this.FuzzyCognitiveMap.FuzzyCognitiveMatrix;
            this.TransitiveClosure = this.staticModel.TransitiveClousure(weights);

            this.PositiveLinks = this.staticModel.PositiveLinksMatrix(this.TransitiveClosure);

            this.PositivePairs = this.staticModel.PositivePairMatrix(this.PositiveLinks);
            this.NegativePairs = this.staticModel.NegativePairMatrix(this.PositiveLinks);

            this.Consonance = this.staticModel.ConsonanceMatrix(this.PositivePairs, this.NegativePairs);
            this.Dissonance = this.staticModel.DissonanceMatrix(this.Consonance);

            this.Influence = this.staticModel.InfluenceMatrix(this.PositivePairs, this.NegativePairs);
        }

        /// <summary>
        /// Запустить динамическое моделирование.
        /// </summary>
        /// <param name="steps"> Количество шагов. </param>
        /// <returns> Состояния. </returns>
        public List<Vector<double>> StartDynamicModeling(int steps)
        {
            Vector<double> vector = new DenseVector(this.FuzzyCognitiveMap.Concepts.Select(c => c.Value).ToArray());
            return this.dynamicModel.StartModelling(vector, this.FuzzyCognitiveMap.FuzzyCognitiveMatrix, steps);
        }
    }
}