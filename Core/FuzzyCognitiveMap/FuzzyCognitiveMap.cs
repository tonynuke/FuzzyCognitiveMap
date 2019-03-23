﻿namespace Core.FuzzyCognitiveMap
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;
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
                    for (var c = 0; c < columnsCount; c++)
                    {
                        newRow[c] = this.fuzzyCognitiveMatrix[rowIndex, c];
                    }

                    dataTable.Rows.Add(newRow);
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
            this.fuzzyCognitiveMatrix[row, column] = value;

            Concept from = this.Concepts[row];
            Concept to = this.Concepts[column];
            this.SetLinkBetweenConcepts(from, to, value);

            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }

        /// <summary>
        /// НКМ.
        /// </summary>
        [CanBeNull]
        public double[,] FuzzyCognitiveMatrix
        {
            get => this.fuzzyCognitiveMatrix?.ToArray();
        }

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

            if (from == to)
            {
                value = 0;
            }

            if (value == 0)
            {
                existingLink = this.conceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
                if (existingLink != null)
                {
                    this.conceptsLinks.Remove(existingLink);
                    return;
                }
            }

            var insertingValue = value;
            if (value > 1)
            {
                insertingValue = 1;
            }
            else if (value < 0)
            {
                insertingValue = 0;
            }

            existingLink = this.conceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
            if (existingLink != null)
            {
                existingLink.Value = value;
                return;
            }

            var newLink = new ConceptsLink(from, to, insertingValue);
            this.conceptsLinks.Add(newLink);

            var rowChangedIndex = this.Concepts.IndexOf(from);
            var columnChangedIndex = this.Concepts.IndexOf(to);

            this.fuzzyCognitiveMatrix[rowChangedIndex, columnChangedIndex] = value;
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
    }
}