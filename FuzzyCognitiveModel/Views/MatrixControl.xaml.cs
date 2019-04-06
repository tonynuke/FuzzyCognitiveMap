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
    public partial class MatrixControl : UserControl
    {
        private FuzzyCognitiveMapViewModel context;

        public MatrixControl()
        {
            this.InitializeComponent();
        }

        private void MatrixControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            var fuzzyCognitiveMapViewModel = this.context;
            if (fuzzyCognitiveMapViewModel != null)
            {
                fuzzyCognitiveMapViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
            }
        }

        private void MatrixControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged -= FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.Matrix.ItemsSource = context.ToDataView(this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.FuzzyCognitiveMatrix);
        }

        private void Matrix_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var row = e.Row.GetIndex();
            var column = int.Parse(e.Column.Header.ToString());
            var enteredText = ((TextBox) e.EditingElement).Text;
            double.TryParse(enteredText, out var value);

            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.SetLinkViaMatrix(row, column, value);
        }

        private void Matrix_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // HACK: чтобы не было exception
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                this.Matrix.CommitEdit();
            }
        }
    }
}
