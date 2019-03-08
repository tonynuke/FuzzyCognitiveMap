using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using FuzzyCognitiveModel.ViewModels;
using QuickGraph;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl, INotifyPropertyChanged
    {
        private IBidirectionalGraph<object, IEdge<object>> _graphToVisualize;

        public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize
        {
            get => this._graphToVisualize;
            set
            {
                if (!Equals(value, this._graphToVisualize))
                {
                    this._graphToVisualize = value;
                    this.RaisePropChanged("GraphToVisualize");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropChanged(string name)
        {
            var eh = this.PropertyChanged;
            eh?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public GraphControl()
        {
            //this.CreateGraphToVisualize();
            InitializeComponent();
        }

        public void CreateGraphToVisualize()
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();

            if (!(this.DataContext is FuzzyCognitiveMapViewModel model))
            {
                _graphToVisualize = g;
                return;
            }

            var concepts = model.Concepts;
            string[] vertices = new string[concepts.Count];
            for (int i = 0; i < concepts.Count; i++)
            {
                vertices[i] = concepts[i].Name;
                g.AddVertex(vertices[i]);
            }

            foreach (var conceptsLink in model.fuzzyCognitiveMap.ConceptsLinks)
            {
                var edge = new LinkEdge(conceptsLink.From.Name, conceptsLink.To.Name);
                if (conceptsLink.Value > 0)
                {
                    edge.EdgeColor = Colors.GreenYellow;
                }
                else if(conceptsLink.Value < 0)
                {
                    edge.EdgeColor = Colors.Red;
                }
                g.AddEdge(edge);
            }

            GraphToVisualize = g;
        }
    }
}
