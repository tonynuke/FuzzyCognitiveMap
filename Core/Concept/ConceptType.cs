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
        Ресурс,

        /// <summary>
        /// Уязвимость.
        /// </summary>
        [Display(Name = "Уязвимость")]
        Уязвимость,

        /// <summary>
        /// Угроза.
        /// </summary>
        [Display(Name = "Угроза")]
        Угроза
    }
}