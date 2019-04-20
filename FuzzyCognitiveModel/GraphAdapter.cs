using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using FuzzyCognitiveModel.Properties;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Адаптер для визуализации графа.
    /// </summary>
    public class GraphAdapter : INotifyPropertyChanged
    {
        /// <summary>
        /// Название графа.
        /// </summary>
        private const string GraphName = "graph";

        private const string ImagesPath = "images";

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropChanged(string name)
        {
            var eh = this.PropertyChanged;
            eh?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Сгененерировать изображение графа.
        /// </summary>
        /// <param name="map"> НКМ. </param>
        /// <returns> Путь до изображение графа. </returns>
        public string GenerateGraphImage(Core.Models.FuzzyCognitiveMap map)
        {
            var g = new AdjacencyGraph<string, TaggedEdge<string, string>>();
            foreach (var concept in map.Concepts)
            {
                g.AddVertex(concept.Name);
            }

            foreach (var link in map.ConceptsLinks)
            {
                var edge = new TaggedEdge<string, string>(link.From.Name, link.To.Name,
                    link.Value.ToString(CultureInfo.InvariantCulture));
                g.AddEdge(edge);
            }

            var graphViz =
                new GraphvizAlgorithm<string, TaggedEdge<string, string>>(g, @".\", GraphvizImageType.Png);

            graphViz.FormatVertex += FormatVertex;
            graphViz.FormatEdge += FormatEdge;
            var imagePath = graphViz.Generate(new FileDotEngine(), GraphName);

            return imagePath;
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<string> e)
        {
            e.VertexFormatter.Label = e.Vertex;
            e.VertexFormatter.Shape = GraphvizVertexShape.Circle;

            e.VertexFormatter.StrokeColor = GraphvizColor.Black;
            e.VertexFormatter.Font = new GraphvizFont(System.Drawing.FontFamily.GenericMonospace.Name, 12);
        }

        private static void FormatEdge(object sender, FormatEdgeEventArgs<string, TaggedEdge<string, string>> e)
        {
            e.EdgeFormatter.Head.Label = e.Edge.Tag;

            e.EdgeFormatter.Font = new GraphvizFont(System.Drawing.FontFamily.GenericMonospace.Name, 12);
            e.EdgeFormatter.FontGraphvizColor = GraphvizColor.Black;

            double value = double.Parse(e.Edge.Tag.Replace('.', ','));

            if (value > 0)
            {
                e.EdgeFormatter.StrokeGraphvizColor = new GraphvizColor(255, 255, 46, 46);
            }
            else if (value < 0)
            {
                e.EdgeFormatter.StrokeGraphvizColor = new GraphvizColor(255, 46, 143, 255);
            }
            else
            {
                e.EdgeFormatter.StrokeGraphvizColor = GraphvizColor.Black;
            }
        }

        /// <summary>
        /// HACK: для обновлени картинки графа.
        /// </summary>
        private static int index = 0;

        public sealed class FileDotEngine : IDotEngine
        {
            public string Run(GraphvizImageType imageType, string dot, string fileName)
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string dotFileName = $"{fileName}.dot";
                string dotPath = Path.Combine(currentDirectory, dotFileName);

                string imageFileName = $"{fileName}{index}.png";
                string imagePath = Path.Combine(currentDirectory, ImagesPath, imageFileName);

                File.Delete(imagePath);

                index++;

                imageFileName = $"{fileName}{index}.png";
                string imagesDirectoryPath = Path.Combine(currentDirectory, ImagesPath);

                imagePath = Path.Combine(imagesDirectoryPath, imageFileName);

                File.WriteAllText(dotFileName, dot);

                ProcessStartInfo startInfo =
                    new ProcessStartInfo
                    {
                        FileName = Settings.Default.GraphvizDotPath,
                        Arguments = $@"dot -T png {dotPath} -o {imagePath}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };

                Process.Start(startInfo)?.WaitForExit(1000);
                return imagePath;
            }
        }

        public GraphAdapter()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagesDirectoryPath = Path.Combine(currentDirectory, ImagesPath);
            if (Directory.Exists(imagesDirectoryPath))
            {
                Directory.Delete(imagesDirectoryPath, true);
            }
            Directory.CreateDirectory(imagesDirectoryPath);
        }
    }
}