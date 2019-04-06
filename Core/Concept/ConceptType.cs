namespace Core.Concept
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Тип концепта.
    /// </summary>
    public enum ConceptType
    {
        /// <summary>
        /// Ресурс.
        /// </summary>
        [Display(Name = "Ресурс")]
        Resource,

        /// <summary>
        /// Уязвимость.
        /// </summary>
        [Display(Name = "Уязвимость")]
        Vulnerability,

        /// <summary>
        /// Угроза.
        /// </summary>
        [Display(Name = "Угроза")]
        Threat
    }
}