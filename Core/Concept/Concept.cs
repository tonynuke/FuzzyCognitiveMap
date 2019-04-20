using System.Collections.Generic;

namespace Core.Concept
{
    /// <summary>
    /// Концепт.
    /// </summary>
    /// <remarks> Узел в графе. </remarks>
    public class Concept
    {
        private sealed class NameEqualityComparer : IEqualityComparer<Concept>
        {
            public bool Equals(Concept x, Concept y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name);
            }

            public int GetHashCode(Concept obj)
            {
                return (obj.Name != null ? obj.Name.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<Concept> NameComparer { get; } = new NameEqualityComparer();

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
    }
}
