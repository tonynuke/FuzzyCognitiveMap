namespace Core.FuzzyCognitiveMap
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Интерфейс когнитивной карты.
    /// </summary>
    public interface IFuzzyCognitiveMap
    {
        /// <summary>
        /// Концепты.
        /// </summary>
        ObservableCollection<Concept> Concepts { get; }

        /// <summary>
        /// Нечеткая когнитивная матрица.
        /// </summary>
        double[,] FuzzyCognitiveMatrix { get; set; }

        /// <summary>
        /// Установить связь между концептами.
        /// </summary>
        /// <param name="from"> Начальный концепт. </param>
        /// <param name="to"> Конечный концепт. </param>
        /// <param name="value"> Значение связи. </param>
        void SetLink(Concept from, Concept to, double value);

        /// <summary>
        /// Добавить концепт.
        /// </summary>
        void AddConcept();

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        void DeleteConcept(Concept concept);
    }
}