using System.Data;
using Core.FuzzyCognitiveMap;

namespace FuzzyCognitiveModel.ViewModels
{
    /// <summary>
    /// Матрица.
    /// </summary>
    public class MatrixViewModel
    {
        public readonly FuzzyCognitiveMap FuzzyCognitiveMap;

        public MatrixViewModel(FuzzyCognitiveMap fuzzyCognitiveMap)
        {
            this.FuzzyCognitiveMap = fuzzyCognitiveMap;
        }

        public DataView DataView
        {
            //set { this.FuzzyCognitiveMap.FuzzyCognitiveMatrixDataTable = value.Table; }
            get => this.FuzzyCognitiveMap.FuzzyCognitiveMatrixDataTable.DefaultView;
        }
    }
}