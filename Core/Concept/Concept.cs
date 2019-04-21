namespace Core.Concept
{
    using System;

    /// <summary>
    /// Концепт.
    /// </summary>
    /// <remarks> Узел в графе. </remarks>
    public class Concept : IEquatable<Concept>
    {
        public bool Equals(Concept other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Concept) obj);
        }

        public override int GetHashCode()
        {
            return (this.Name != null ? this.Name.GetHashCode() : 0);
        }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public ConceptType Type { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Значение концепта.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">  Название. </param>
        public Concept(string name)
        {
            this.Name = name;
        }
    }
}
