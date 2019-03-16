namespace Core.FuzzyCognitiveMap
{
    using System;

    /// <summary>
    /// Связь между концептами.
    /// </summary>
    /// <remarks> Дуга в графе. </remarks>
    public class ConceptsLink
    {
        /// <summary>
        /// От.
        /// </summary>
        public Concept From { get; }

        /// <summary>
        /// Куда.
        /// </summary>
        public Concept To { get; }

        /// <summary>
        /// Значение связи.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="from"> От. </param>
        /// <param name="to"> Куда. </param>
        /// <param name="value"> Значение связи. </param>
        public ConceptsLink(Concept from, Concept to, double value)
        {
            this.From = from ?? throw new ArgumentNullException(nameof(@from));
            this.To = to ?? throw new ArgumentNullException(nameof(to));
            this.Value = value;
        }
    }
}