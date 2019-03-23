using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,] {
                {1,1,1,1},
                {1,2,3,4},
                {4,3,2,1}});
        }
    }
}