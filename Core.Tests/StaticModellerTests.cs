using Core.Modeling;

namespace Core.Tests
{
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using NUnit.Framework;

    [TestFixture]
    public class StaticModellerTests
    {
        [Test]
        public void Test1()
        {
            Matrix<double> matrix = DenseMatrix.OfArray(new double[,]
            {
                { 1, 2 },
                { 3, 4 }
            });

            var op = new StaticModel();
            var m = op.PositiveLinksMatrix(matrix);

            Assert.IsNotNull(m);
        }

        [Test]
        public void Smoke_test()
        {
            Matrix<double> matrix = DenseMatrix.OfArray(new double[,]
            {
                { 1, 2 },
                { 3, 4 }
            });

            var op = new StaticModel();
            op.Do(matrix);
        }
    }
}