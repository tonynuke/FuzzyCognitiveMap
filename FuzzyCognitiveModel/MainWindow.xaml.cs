using System.Linq;
using System.Windows;
using Core.FuzzyCognitiveMap;
using FuzzyCognitiveModel.ViewModels;
using GraphSharp.Controls;
using QuickGraph;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FuzzyCognitiveMapViewModel FuzzyCognitiveViewModel { get; set; } = new FuzzyCognitiveMapViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.FuzzyCognitiveViewModel.AddConceptCommand.Execute(null);
            this.FuzzyCognitiveViewModel.AddConceptCommand.Execute(null);
            this.FuzzyCognitiveViewModel.AddConceptCommand.Execute(null);
            this.FuzzyCognitiveViewModel.AddConceptCommand.Execute(null);

            this.FuzzyCognitiveViewModel.fuzzyCognitiveMap.SetLink(this.FuzzyCognitiveViewModel.Concepts.First(), this.FuzzyCognitiveViewModel.Concepts.Last(), 10);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var m = this.FuzzyCognitiveViewModel.FuzzyCognitiveMatrix;
            this.FCM.Matrix.Items.Refresh();
            //this.GraphControl.CreateGraphToVisualize();
        }
    }
}
