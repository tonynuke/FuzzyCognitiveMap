using System;
using System.Windows.Media;
using GraphSharp;
using QuickGraph;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Ребро связи.
    /// </summary>
    public class LinkEdge : Edge<object>
    {
        /// <summary>
        /// Цвет связи.
        /// </summary>
        public Color EdgeColor { get; set; } = Colors.Black;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public LinkEdge(object source, object target) : base(source, target)
        {
        }
    }
}