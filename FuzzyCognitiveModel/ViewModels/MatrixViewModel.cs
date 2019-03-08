using System.Data;
using Core.FuzzyCognitiveMap;

namespace FuzzyCognitiveModel.ViewModels
{
    /// <summary>
    /// Матрица.
    /// </summary>
    public class MatrixViewModel
    {
        double[,] dataArray;

        public MatrixViewModel(double[,] dataArray)
        {
            this.dataArray = dataArray;
            this.Redraw();
        }

        public void Redraw()
        {
            var array = this.dataArray;
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);
            var t = new DataTable();

            for (var c = 0; c < columns; c++)
            {
                t.Columns.Add(new DataColumn(c.ToString()));
            }

            for (var r = 0; r < rows; r++)
            {
                var newRow = t.NewRow();
                for (var c = 0; c < columns; c++)
                {
                    newRow[c] = array[r, c];
                }
                t.Rows.Add(newRow);
            }
            this.DataView = t.DefaultView;
        }

        public DataView DataView { get; set; }
    }
}