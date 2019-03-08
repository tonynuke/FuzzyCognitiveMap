using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.FuzzyCognitiveMap;

namespace FuzzyCognitiveModel.ViewModels
{
    public class FuzzyCognitiveMapViewModel
    {
        public readonly FuzzyCognitiveMap fuzzyCognitiveMap = new FuzzyCognitiveMap();

        public ObservableCollection<Concept> Concepts => this.fuzzyCognitiveMap.Concepts;

        public MatrixViewModel FuzzyCognitiveMatrix => new MatrixViewModel(this.fuzzyCognitiveMap.FuzzyCognitiveMatrix);

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
            this.fuzzyCognitiveMap.AddConcept();
            this.FuzzyCognitiveMatrix.Redraw();
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
            this.fuzzyCognitiveMap.DeleteConcept(obj as Concept);
        }
    }
}