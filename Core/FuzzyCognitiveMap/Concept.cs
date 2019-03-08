﻿namespace Core.FuzzyCognitiveMap
{
    /// <summary>
    /// Концепт.
    /// </summary>
    public class Concept
    {
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public ConceptType Type { get; set; }

        /// <summary>
        /// Целевой.
        /// </summary>
        public bool IsTarget { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }
    }
}