using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using Core.Concept;
using MathNet.Numerics.LinearAlgebra;

namespace FuzzyCognitiveModel.ViewModels
{
    public class FuzzyCognitiveMapViewModel
    {
        /// <summary>
        /// Когнитивная карта.
        /// </summary>
        public Core.Models.FuzzyCognitiveModel FuzzyCognitiveModel { get; } = new Core.Models.FuzzyCognitiveModel();

        /// <summary>
        /// Концепты.
        /// </summary>
        public ObservableCollection<Concept> Concepts { get; set; } = new ObservableCollection<Concept>();

        public DataView Consonance => this.ToDataView(this.FuzzyCognitiveModel.Consonance);

        public DataView ConsonanceInfluence => this.ToDataView(this.FuzzyCognitiveModel.ConsonanceInfluence);

        public DataView Dissonance => this.ToDataView(this.FuzzyCognitiveModel.Dissonance);

        public DataView DissonanceInfluence => this.ToDataView(this.FuzzyCognitiveModel.DissonanceInfluence);

        public DataView Influence => this.ToDataView(this.FuzzyCognitiveModel.Influence);

        public DataView ConceptInfluence => this.ToDataView(this.FuzzyCognitiveModel.ConceptInfluence);

        public DataView DataView => this.ToDataView(this.FuzzyCognitiveModel.FuzzyCognitiveMap.FuzzyCognitiveMatrix);

        /// <summary>
        /// Преобразовать в таблицу данных.
        /// </summary>
        public DataView ToDataView(Matrix<double> matrix)
        {
            var rowsCount = matrix?.RowCount;
            var columnsCount = matrix?.ColumnCount;
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
                    newRow[columnIndex] = matrix[rowIndex, columnIndex];
                }
            }

            return dataTable.DefaultView;
        }

        /// <summary>
        /// Преобразовать в таблицу данных.
        /// </summary>
        public DataView ToDataView(Vector<double> vector)
        {
            var rowsCount = vector?.Count;
            var columnsCount = 1;
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
                    newRow[columnIndex] = vector[rowIndex];
                }
            }

            return dataTable.DefaultView;
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
            this.FuzzyCognitiveModel.FuzzyCognitiveMap.AddConcept();
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
            this.FuzzyCognitiveModel.FuzzyCognitiveMap.DeleteConcept(obj as Concept);
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
            this.FuzzyCognitiveModel.StartDynamicModeling(10);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FuzzyCognitiveMapViewModel()
        {
            FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // TODO: оптимизировать через посылку события о кокретном удаленном/добавленном концепте.
            this.Concepts.Clear();

            foreach (var concept in FuzzyCognitiveModel.FuzzyCognitiveMap.Concepts)
            {
                this.Concepts.Add(concept);
            }
        }
    }
}