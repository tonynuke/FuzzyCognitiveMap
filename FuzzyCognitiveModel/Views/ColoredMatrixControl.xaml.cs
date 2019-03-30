using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FuzzyCognitiveModel.ViewModels;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для FuzzyCognitiveMatrixUser.xaml
    /// </summary>
    public partial class ColoredMatrixControl : UserControl
    {
        private FuzzyCognitiveMapViewModel context;

        public ColoredMatrixControl()
        {
            this.InitializeComponent();
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.Matrix.ItemsSource = this.context.FuzzyCognitiveMap.Matrix;
        }

        private void MatrixColored_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            var fuzzyCognitiveMapViewModel = this.context;
            if (fuzzyCognitiveMapViewModel != null)
            {
                fuzzyCognitiveMapViewModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
            }
        }

        private void MatrixColored_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }
    }
}
