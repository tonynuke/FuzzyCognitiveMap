using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using Core.FuzzyCognitiveMap;

namespace FuzzyCognitiveModel.ViewModels
{
    public class FuzzyCognitiveMapViewModel
    {
        /// <summary>
        /// Когнитивная карта.
        /// </summary>
        public FuzzyCognitiveMap FuzzyCognitiveMap { get; } = new FuzzyCognitiveMap();

        /// <summary>
        /// Концепты.
        /// </summary>
        public ObservableCollection<Concept> Concepts { get; set; } = new ObservableCollection<Concept>();

        public DataView DataView
        {
            set { this.FuzzyCognitiveMap.FuzzyCognitiveMatrixDataTable = value.Table; }
            get => this.FuzzyCognitiveMap.FuzzyCognitiveMatrixDataTable.DefaultView;
        }

        private ICommand addConceptCommand;

        /// <summary>
        /// Команда добавления концепта.
        /// </summary>
        public ICommand AddConceptCommand =>
            this.addConceptCommand ?? (
                this.addConceptCommand = new DelegateCommand(this.AddConcept));

        /// <summary>
        /// Добавить концепт.
        /// </summary>
        /// <param name="obj"> Параметр. </param>
        private void AddConcept(object obj)
        {
            this.FuzzyCognitiveMap.AddConcept();
        }

        private ICommand deleteConceptCommand;

        /// <summary>
        /// Команда удаления концепта.
        /// </summary>
        public ICommand DeleteConceptCommand =>
            this.deleteConceptCommand ?? (
                this.deleteConceptCommand = new DelegateCommand(this.DeleteConcept));

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="obj"> Параметр. </param>
        private void DeleteConcept(object obj)
        {
            this.FuzzyCognitiveMap.DeleteConcept(obj as Concept);
        }

        private ICommand modellCommand;

        /// <summary>
        /// Команда моделирования.
        /// </summary>
        public ICommand ModellCommand =>
            this.modellCommand ?? (
                this.modellCommand = new DelegateCommand(this.StartModeling));

        /// <summary>
        /// Запустить динамическое моделирование.
        /// </summary>
        /// <param name="obj"> Параметр. </param>
        private void StartModeling(object obj)
        {
            this.FuzzyCognitiveMap.StartModeling(10);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FuzzyCognitiveMapViewModel()
        {
            FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // TODO: оптимизировать через посылку события о кокретном удаленном/добавленном концепте.
            this.Concepts.Clear();

            foreach (var concept in FuzzyCognitiveMap.Concepts)
            {
                this.Concepts.Add(concept);
            }
        }
    }
}