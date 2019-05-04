namespace Core.Tests
{
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using Modeling;
    using NUnit.Framework;

    [TestFixture]
    public class DynamicModellerTests
    {
        [Test]
        public void Test1()
        {
            Vector<double> vector = DenseVector.Build.DenseOfArray(new double[]
            {
                1, 0.4
            });

            Matrix<double> matrix = DenseMatrix.OfArray(new double[,]
            {
                { 0, 0.2 },
                { 0.4, 0 }
            });

            var dynamicModeling = new DynamicModel();
            var m = dynamicModeling.StartModelling(vector, matrix);

            Assert.IsNotNull(m);
        }
    }
}