using System;
using System.Windows.Media;
using Core.FuzzyCognitiveMap;
using GraphSharp;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Ребро связи.
    /// </summary>
    public class LinkEdge : TypedEdge<Object>
    {
        /// <summary>
        /// Значение связи.
        /// </summary>
        /// <remarks><see cref="ConceptsLink.Value"/></remarks>
        public string Value { get; set; }

        /// <summary>
        /// Цвет связи.
        /// </summary>
        public Color EdgeColor { get; set; } = Colors.Black;
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public LinkEdge(object source, object target) : base(source, target, EdgeTypes.General) { }
    }
}