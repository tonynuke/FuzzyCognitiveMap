using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FuzzyCognitiveModel.ViewModels;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для FuzzyCognitiveMatrixUser.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {
        private FuzzyCognitiveMapViewModel context;

        public MatrixControl()
        {
            this.InitializeComponent();
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.Matrix.ItemsSource = this.context.FuzzyCognitiveMap.FuzzyCognitiveMatrixDataTable.DefaultView;
        }

        private void MatrixControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            this.context.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }

        private void Matrix_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            this.context.FuzzyCognitiveMap.SetLinkViaMatrix(1, 1, 1);
        }
    }
}
