using System.Data;
using System.Windows.Input;
using Core.FuzzyCognitiveMap;

namespace FuzzyCognitiveModel.ViewModels
{
    public class FuzzyCognitiveMapViewModel
    {
        public FuzzyCognitiveMap FuzzyCognitiveMap { get; set; } = new FuzzyCognitiveMap();

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
                this.modellCommand = new DelegateCommand(this.Model));

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="obj"> Параметр. </param>
        private void Model(object obj)
        {
            this.FuzzyCognitiveMap.Model(10);
        }
    }
}