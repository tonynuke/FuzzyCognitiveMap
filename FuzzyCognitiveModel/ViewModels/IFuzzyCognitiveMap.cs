namespace Core.FuzzyCognitiveMap
{
    /// <summary>
    /// Интерфейс нечеткой когнитивной карты.
    /// </summary>
    public interface IFuzzyCognitiveMap
    {
        /// <summary>
        /// Добавить концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        void AddConcept(Concept concept);

        /// <summary>
        /// Удалить концепт.
        /// </summary>
        /// <param name="concept"> Концепт. </param>
        void DeleteConcept(Concept concept);

        /// <summary>
        /// Получить нечеткую когнитивную матрицу.
        /// </summary>
        /// <returns> Нечеткая когнитивная матрицу. </returns>
        double[,] GetFuzzyCognitiveMatrix();
    }
}