namespace Core.Tests
{
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using Modeling;
    using NUnit.Framework;

    [TestFixture]
    public class StaticModellerTests
    {
        [Test]
        public void Static_model_does_not_fail_smoke_test()
        {
            Matrix<double> matrix = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0.2 },
                { 0.1, 0 }
            });

            var op = new StaticModel();

            // шаг 1
            var R = op.PositiveLinksMatrix(matrix);

            // шаг 2
            var pv = op.PositivePairMatrix(R);
            var nv = op.NegativePairMatrix(R);

            // шаг 3
            var cons = op.ConsonanceMatrix(pv, nv);
            var diss = op.DissonanceMatrix(cons);
            var influence = op.InfluenceMatrix(pv, nv);

            // шаг 4
            var consInfluence = op.ConceptInfluence(cons);
            var dissInfluence = op.ConceptInfluence(diss);
            var conceptInfluence = op.ConceptInfluence(influence);

            // шаг 5
            var probability = op.CalculateProbability(conceptInfluence, conceptInfluence);
            var damage = op.CalculateDamage(probability, consInfluence);
        }
    }
}