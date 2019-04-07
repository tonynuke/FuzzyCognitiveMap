using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GraphX.Controls;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для XGraphControl.xaml
    /// </summary>
    public partial class XGraphControl : UserControl
    {
        public XGraphControl()
        {
            InitializeComponent();
        }

        private void XGraphControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var graph = new GraphExample();

            //Create logic core
            var LogicCore = new GXLogicCoreExample() { Graph = graph };
            //Assign data graph
            gg_Area.LogicCore = LogicCore;
            //Call relayout. Additional parameters shows that edges must be created or updated if any exists.

            //Create and add vertices
            graph.AddVertex(new DataVertex() { ID = 1, Text = "Item 1" });
            graph.AddVertex(new DataVertex() { ID = 2, Text = "Item 2" });
            var v1 = graph.Vertices.First();
            var v2 = graph.Vertices.Last();
            var e1 = new DataEdge(v1, v2, 1) {Text = "1 -> 2"};
            graph.AddEdge(e1);
            gg_Area.LogicCore.Graph = graph;

            var vc1 = new VertexControl(v1);
            gg_Area.AddVertex(v1, vc1);
            var vc2 = new VertexControl(v2);
            gg_Area.AddVertex(v2, vc2);

            var ec = new EdgeControl(vc1, vc2, e1);
            gg_Area.InsertEdge(e1, ec);

            gg_Area.RelayoutGraph(true);
        }
    }
}
