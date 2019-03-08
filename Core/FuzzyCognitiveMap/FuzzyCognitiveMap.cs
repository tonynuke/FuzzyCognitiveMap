using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Annotations;

namespace Core.FuzzyCognitiveMap
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

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
        private readonly ObservableCollection<Concept> concepts = new ObservableCollection<Concept>();

        /// <summary>
        /// НКМ.
        /// </summary>
        private double[,] fuzzyCognitiveMatrix;

        /// <summary>
        /// Связи между концептами.
        /// </summary>
        public readonly ObservableCollection<ConceptsLink> ConceptsLinks = new ObservableCollection<ConceptsLink>();

        public ObservableCollection<Concept> Concepts => this.concepts;

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
            var length = this.concepts.Count;
            var dictionary = new Dictionary<int, Concept>();
            for (int i = 0; i < length; i++)
            {
                dictionary.Add(i, this.concepts[i]);
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
            var length = this.concepts.Count;
            var matrix = new double[length, length];

            var dictionary = new Dictionary<Concept, int>();

            for (int i = 0; i < length; i++)
            {
                dictionary.Add(this.concepts[i], i);
            }

            foreach (var link in this.ConceptsLinks)
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

            if (value == 0)
            {
                existingLink = this.ConceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
                if (existingLink != null)
                {
                    this.ConceptsLinks.Remove(existingLink);
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

            existingLink = this.ConceptsLinks.SingleOrDefault(link => link.From == from && link.To == to);
            if (existingLink != null)
            {
                existingLink.Value = value;
                return;
            }

            var newLink = new ConceptsLink(from, to, insertingValue);
            this.ConceptsLinks.Add(newLink);
        }

        public void AddConcept()
        {
            var defaultName = $"{DefaultName}{this.concepts.Count}";
            var newConcept = new Concept
            {
                Name = defaultName
            };
            this.concepts.Add(newConcept);
        }

        public void DeleteConcept(Concept concept)
        {
            this.concepts.Remove(concept);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}