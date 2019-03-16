using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FuzzyCognitiveModel.ViewModels;

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

            this.FuzzyCognitiveViewModel.FuzzyCognitiveMap
                .SetLink(this.FuzzyCognitiveViewModel.FuzzyCognitiveMap.Concepts.First(), 
                    this.FuzzyCognitiveViewModel.FuzzyCognitiveMap.Concepts.Last(), 10);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
