﻿namespace Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Concept;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Numerics.LinearAlgebra.Double;
    using Newtonsoft.Json;

    /// <summary>
    /// Нечетная когнитивная карта.
    /// </summary>
    public class FuzzyCognitiveMap : INotifyPropertyChanged
    {
        /// <summary>
        /// Название концепта по умолчанию.
        /// </summary>
        private const string DefaultName = "C";

        /// <summary>
        /// Индекс концепта.
        /// </summary>
        private static int conceptIndex = 1;

        /// <summary>
        /// Концепты.
        /// </summary>
        private List<Concept> concepts = new List<Concept>();

        /// <summary>
        /// Концепты.
        /// </summary>
        public IEnumerable<Concept> Concepts => this.concepts;

        /// <summary>
        /// Вероятности возникнвоения угроз.
        /// </summary>
        public IEnumerable<double> ThreatProbabilities => this.concepts.Where(c => c.Type == ConceptType.Threat).Select(c => c.Value);

        /// <summary>
        /// Ценности ресурсов.
        /// </summary>
        public IEnumerable<double> ResourceValues => this.concepts.Where(c => c.Type == ConceptType.Resource).Select(c => c.Value);

        /// <summary>
        /// Критичность уязвимостей.
        /// </summary>
        public IEnumerable<double> VulnerabilityCriticalities => this.concepts.Where(c => c.Type == ConceptType.Vulnerability).Select(c => c.Value);

        /// <summary>
        /// Связи между концептами.
        /// </summary>
        private List<ConceptsLink> conceptsLinks = new List<ConceptsLink>();

        /// <summary>
        /// Связи между концептами.
        /// </summary>
        public IEnumerable<ConceptsLink> ConceptsLinks => this.conceptsLinks;

        /// <summary>
        /// Нечеткая когнитивная матрица (НКМ).
        /// </summary>
        private Matrix<double> fuzzyCognitiveMatrix;

        /// <summary>
        /// Нечеткая когнитивная матрица (НКМ).
        /// </summary>
        public Matrix<double> FuzzyCognitiveMatrix => this.fuzzyCognitiveMatrix;

        /// <summary>
        /// Установить значение связи через НКМ.
        /// </summary>
        /// <param name="row"> Строка. </param>
        /// <param name="column"> Столбец. </param>
        /// <param name="value"> Значение связи. </param>
        public void SetLinkViaMatrix(int row, int column, double value)
        {
            var matrixSize = this.concepts.Count;
            if (this.fuzzyCognitiveMatrix == null)
            {
                this.fuzzyCognitiveMatrix = new DenseMatrix(matrixSize, matrixSize);
            }

            Concept from = this.concepts[row];
            Concept to = this.concepts[column];
            this.SetLinkBetweenConcepts(from, to, value);
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
                }

                this.UpdateCognitiveMatrix(from, to, value);
                return;
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
        /// <returns> Концепт. </returns>
        public Concept AddConcept()
        {
            var defaultName = $"{DefaultName}{conceptIndex}";
            var newConcept = new Concept(defaultName);

            this.concepts.Add(newConcept);
            conceptIndex++;

            if (this.concepts.Count == 1)
            {
                this.fuzzyCognitiveMatrix = new DenseMatrix(1);
            }
            else if (this.concepts.Count > 1)
            {
                var insertIndex = this.concepts.Count - 1;
                var insertingRow = new DenseVector(this.concepts.Count - 1);
                var insertingColumn = new DenseVector(this.concepts.Count);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertRow(insertIndex, insertingRow);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertColumn(insertIndex, insertingColumn);
            }

            this.OnPropertyChanged(nameof(this.ConceptsLinks));

            return newConcept;
        }

        /// <summary>
        /// Добавить новый концепт.
        /// </summary>
        private void AddConcept(Concept concept)
        {
            this.concepts.Add(concept);

            if (this.concepts.Count == 1)
            {
                this.fuzzyCognitiveMatrix = new DenseMatrix(1);
            }
            else if (this.concepts.Count > 1)
            {
                var insertIndex = this.concepts.Count - 1;
                var insertingRow = new DenseVector(this.concepts.Count - 1);
                var insertingColumn = new DenseVector(this.concepts.Count);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertRow(insertIndex, insertingRow);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.InsertColumn(insertIndex, insertingColumn);
            }

            conceptIndex++;
        }

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        public void DeleteConcept(Concept concept)
        {
            var removeIndex = this.concepts.IndexOf(concept);

            if (this.concepts.Count > 1)
            {
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.RemoveColumn(removeIndex);
                this.fuzzyCognitiveMatrix = this.fuzzyCognitiveMatrix.RemoveRow(removeIndex);
            }
            else
            {
                this.fuzzyCognitiveMatrix = null;
            }

            this.concepts.Remove(concept);
            this.RemoveConceptLinks(concept);

            this.OnPropertyChanged(nameof(this.ConceptsLinks));
        }

        /// <summary>
        /// Удалить связи в которых участвует концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        private void RemoveConceptLinks(Concept concept)
        {
            var linksToRemove = this.conceptsLinks.Where(link => link.From.Equals(concept) || link.To.Equals(concept)).ToList();
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
            var rowChangedIndex = this.concepts.IndexOf(from);
            var columnChangedIndex = this.concepts.IndexOf(to);
            this.fuzzyCognitiveMatrix[rowChangedIndex, columnChangedIndex] = value;

            this.OnPropertyChanged(nameof(this.ConceptsLinks));
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
        /// Сохранить модель.
        /// </summary>
        /// <param name="fileName"> Название модели. </param>
        public void Save(string fileName)
        {
            var save = new SaveResult
            {
                Concepts = this.concepts,
                ConceptsLinks = this.conceptsLinks
            };

            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, save);
            }
        }

        /// <summary>
        /// Загрузить модель.
        /// </summary>
        /// <param name="fileName"> Название модели. </param>
        public void Load(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                var save = JsonConvert.DeserializeObject<SaveResult>(json);

                this.concepts = new List<Concept>();
                this.conceptsLinks = new List<ConceptsLink>();

                conceptIndex = 1;

                foreach (var concept in save.Concepts)
                {
                    this.AddConcept(concept);
                }

                foreach (var conceptsLink in save.ConceptsLinks)
                {
                    var rowChangedIndex = this.concepts.IndexOf(conceptsLink.From);
                    var columnChangedIndex = this.concepts.IndexOf(conceptsLink.To);
                    this.fuzzyCognitiveMatrix[rowChangedIndex, columnChangedIndex] = conceptsLink.Value;
                }

                this.conceptsLinks.AddRange(save.ConceptsLinks);

                this.OnPropertyChanged(nameof(this.ConceptsLinks));
            }
        }
    }
}