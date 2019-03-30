namespace Core.FuzzyCognitiveMap
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// Нечетная когнитивная карта.
    /// </summary>
    public class FuzzyCognitiveMap : INotifyPropertyChanged
    {
        /// <summary>
        /// Название концепта по умолчанию.
        /// </summary>
        private const string DefaultName = "Безымянный";

        /// <summary>
        /// Концепты.
        /// </summary>
        public ObservableCollection<Concept> Concepts { get; set; } = new ObservableCollection<Concept>();

        /// <summary>
        /// Связи между концептами.
        /// </summary>
        private readonly List<ConceptsLink> conceptsLinks = new List<ConceptsLink>();

        /// <summary>
        /// Нечеткая когнитивная матрица (НКМ).
        /// </summary>
        private Matrix<double> fuzzyCognitiveMatrix;

        /// <summary>
        /// Моделлер.
        /// </summary>
        private DynamicModeller DynamicModeller = new DynamicModeller();

        /// <summary>
        /// Моделировать.
        /// </summary>
        /// <param name="steps"> Количество шагов. </param>
        /// <returns> Состояния. </returns>
        public List<Vector<double>> Model(int steps)
        {
            Vector<double> vector = new DenseVector(this.Concepts.Select(c => c.Value).ToArray());
            return this.DynamicModeller.StartModelling(vector, this.fuzzyCognitiveMatrix, steps);
        }

        public List<List<double>> Matrix
        {
            get
            {
                List<List<double>> matrix = new List<List<double>>();
                var rowsCount = this.fuzzyCognitiveMatrix?.RowCount;
                var columnsCount = this.fuzzyCognitiveMatrix?.ColumnCount;

                for (int row = 0; row < rowsCount; row++)
                {
                    matrix.Add(new List<double>());

                    for (int column = 0; column < columnsCount; column++)
                    {
                        matrix[row].Add(this.fuzzyCognitiveMatrix[row, column]);
                    }
                }

                return matrix;
            }
        }

        /// <summary>
        /// Таблица для отображения НКМ.
        /// </summary>
        public DataTable FuzzyCognitiveMatrixDataTable
        {
            get
            {
                var rowsCount = this.fuzzyCognitiveMatrix?.RowCount;
                var columnsCount = this.fuzzyCognitiveMatrix?.ColumnCount;
                var dataTable = new DataTable();

                for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    dataTable.Columns.Add(new DataColumn(columnIndex.ToString()));
                }

                for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {
                    var newRow = dataTable.NewRow();
                    dataTable.Rows.Add(newRow);

                    for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                    {
                        newRow[columnIndex] = this.fuzzyCognitiveMatrix[rowIndex, columnIndex];
                    }
                }

                return dataTable;
            }

            set
            {
                for (var row = 0; row < value.Rows.Count; row++)
                {
                    for (var column = 0; column < value.Columns.Count; row++)
                    {
                        this.fuzzyCognitiveMatrix[row, column] = (double)value.Rows[row][column];
                    }
                }
            }
        }

        /// <summary>
        /// Установить значение связи через НКМ.
        /// </summary>
        /// <param name="row"> Строка. </param>
        /// <param name="column"> Столбец. </param>
        /// <param name="value"> Значение связи. </param>
        public void SetLinkViaMatrix(int row, int column, double value)
        {
            this.fuzzyCognitiveMatrix = DenseMatrix.OfArray(this.FuzzyCognitiveMatrix);

            Concept from = this.Concepts[row];
            Concept to = this.Concepts[column];
            this.SetLinkBetweenConcepts(from, to, value);
        }

        /// <summary>
        /// НКМ.
        /// </summary>
        public double[,] FuzzyCognitiveMatrix => this.fuzzyCognitiveMatrix?.ToArray();

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Установить связь между концептами.
        /// </summary>
        /// <param name="from"> От. </param>
        /// <param name="to"> К. </param>
        /// <param name="value"> Значение связи. </param>
        public void SetLinkBetweenConcepts(Concept from, Concept to, double value)
        {
            ConceptsLink existingLink = null;
            bool isSameConceptLinked = from == to;

            if (isSameConceptLinked)
            {
                this.UpdateCognitiveMatrix(from, to, 0);
                return;
            }

            if (value == 0)
            {
                existingLink = this.conceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
                if (existingLink != null)
                {
                    this.conceptsLinks.Remove(existingLink);
                    this.UpdateCognitiveMatrix(from, to, value);
                    return;
                }
            }

            var insertingValue = value;
            if (value > 1)
            {
                insertingValue = 1;
            }
            else if (value < -1)
            {
                insertingValue = -1;
            }

            existingLink = this.conceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
            if (existingLink != null)
            {
                existingLink.Value = value;
                this.UpdateCognitiveMatrix(from, to, insertingValue);
                return;
            }

            var newLink = new ConceptsLink(from, to, insertingValue);
            this.conceptsLinks.Add(newLink);
            this.UpdateCognitiveMatrix(from, to, insertingValue);
        }

        /// <summary>
        /// Добавить новый концепт.
        /// </summary>
        public void AddConcept()
        {
            var defaultName = $"{DefaultName}{this.Concepts.Count}";
            var newConcept = new Concept
            {
                Name = defaultName
            };
            this.Concepts.Add(newConcept);

            if (this.Concepts.Count == 1)
            {
                this.fuzzyCognitiveMatrix = new DenseMatrix(1);
            }
            else if (this.Concepts.Count > 1)
            {
                var insertIndex = this.Concepts.Count - 1;
                var insertingRow = new DenseVector(this.Concepts.Count - 1);
                var insertingColumn = new DenseVector(this.Concepts.Count);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertRow(insertIndex, insertingRow);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertColumn(insertIndex, insertingColumn);
            }

            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        public void DeleteConcept(Concept concept)
        {
            var removeIndex = this.Concepts.IndexOf(concept);

            if (this.Concepts.Count > 1)
            {
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.RemoveColumn(removeIndex);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.RemoveRow(removeIndex);
            }
            else
            {
                this.fuzzyCognitiveMatrix = null;
            }

            this.Concepts.Remove(concept);
            this.RemoveConveptLinks(concept);

            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }

        /// <summary>
        /// Удалить связи в которых участвует концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        private void RemoveConveptLinks(Concept concept)
        {
            var linksToRemove = this.conceptsLinks.Where(link => link.From == concept || link.To == concept).ToList();
            foreach (var link in linksToRemove)
            {
                this.conceptsLinks.Remove(link);
            }
        }

        /// <summary>
        /// Обновить связь между концептами в НКМ.
        /// </summary>
        /// <param name="from"> От. </param>
        /// <param name="to"> К. </param>
        /// <param name="value"> Значение связи. </param>
        private void UpdateCognitiveMatrix(Concept from, Concept to, double value)
        {
            var rowChangedIndex = this.Concepts.IndexOf(from);
            var columnChangedIndex = this.Concepts.IndexOf(to);
            this.fuzzyCognitiveMatrix[rowChangedIndex, columnChangedIndex] = value;

            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }
    }
}