namespace Core.FuzzyCognitiveMap
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;

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
        /// НКМ.
        /// </summary>
        private double[,] fuzzyCognitiveMatrix = new double[1, 1];

        /// <summary>
        /// Связи между концептами.
        /// </summary>
        private readonly ObservableCollection<ConceptsLink> conceptsLinks = new ObservableCollection<ConceptsLink>();

        /// <summary>
        /// Концепты.
        /// </summary>
        public ObservableCollection<Concept> Concepts { get; set; } = new ObservableCollection<Concept>();

        public DataTable FuzzyCognitiveMatrixDataTable
        {
            get
            {
                var rows = this.FuzzyCognitiveMatrix.GetLength(0);
                var columns = this.FuzzyCognitiveMatrix.GetLength(1);
                var dataTable = new DataTable();

                for (var c = 0; c < columns; c++)
                {
                    dataTable.Columns.Add(new DataColumn(c.ToString()));
                }

                for (var r = 0; r < rows; r++)
                {
                    var newRow = dataTable.NewRow();
                    for (var c = 0; c < columns; c++)
                    {
                        newRow[c] = this.FuzzyCognitiveMatrix[r, c];
                    }

                    dataTable.Rows.Add(newRow);
                }

                return dataTable;
            }

            //set
            //{
            //    for (var row = 0; row < value.Rows.Count; row++)
            //    {
            //        for (var column = 0; column < value.Columns.Count; row++)
            //        {
            //            this.fuzzyCognitiveMatrix[row, column] = (double)value.Rows[row][column];
            //        }
            //    }
            //}
        }

        /// <summary>
        /// НКМ.
        /// </summary>
        public double[,] FuzzyCognitiveMatrix
        {
            get => this.GetFuzzyCognitiveMatrix();
            set
            {
                this.fuzzyCognitiveMatrix = value;
                this.UpdateConceptLinks();
                this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
            }
        }

        /// <summary>
        /// Обновить связи между концептами.
        /// </summary>
        private void UpdateConceptLinks()
        {
            var length = this.Concepts.Count;
            var dictionary = new Dictionary<int, Concept>();
            for (int i = 0; i < length; i++)
            {
                dictionary.Add(i, this.Concepts[i]);
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var value = this.fuzzyCognitiveMatrix[i, j];
                    Concept from = dictionary[i];
                    Concept to = dictionary[j];

                    this.SetLink(from, to, value);
                }
            }
        }

        /// <summary>
        /// Получить НКМ.
        /// </summary>
        /// <returns> НКМ. </returns>
        private double[,] GetFuzzyCognitiveMatrix()
        {
            var length = this.Concepts.Count;
            var matrix = new double[length, length];

            var dictionary = new Dictionary<Concept, int>();

            for (int i = 0; i < length; i++)
            {
                dictionary.Add(this.Concepts[i], i);
            }

            foreach (var link in this.conceptsLinks)
            {
                var i = dictionary[link.From];
                var j = dictionary[link.To];

                matrix[i, j] = link.Value;
            }

            return matrix;
        }

        public void SetLink(Concept from, Concept to, double value)
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
        }

        public void AddConcept()
        {
            var defaultName = $"{DefaultName}{this.Concepts.Count}";
            var newConcept = new Concept
            {
                Name = defaultName
            };
            this.Concepts.Add(newConcept);
            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }

        public void DeleteConcept(Concept concept)
        {
            this.Concepts.Remove(concept);
            this.OnPropertyChanged(nameof(this.FuzzyCognitiveMatrix));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}