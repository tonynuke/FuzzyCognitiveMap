namespace Core
{
    using System.Collections.Generic;
    using Concept;

    /// <summary>
    /// Сохранение.
    /// </summary>
    public class SaveResult
    {
        /// <summary>
        /// Концепты.
        /// </summary>
        public List<Concept.Concept> Concepts { get; set; }

        /// <summary>
        /// Ссылки.
        /// </summary>
        public List<ConceptsLink> ConceptsLinks { get; set; }
    }
}