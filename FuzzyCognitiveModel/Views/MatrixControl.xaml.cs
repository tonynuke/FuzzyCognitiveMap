using System.ComponentModel;
using System.Text.RegularExpressions;
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
        private FuzzyCognitiveMapViewModel context => (FuzzyCognitiveMapViewModel)this.DataContext;

        private Regex doubleRegex = new Regex(@"[-]{0,1}(?:0|[1-9][0-9]*)[\,|\.]{0,1}[0-9]*");

        public MatrixControl()
        {
            this.InitializeComponent();
        }

        private void MatrixControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.context != null)
            {
                this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
                this.UpdateHeaders();
            }
        }

        private void MatrixControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged -= FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.Matrix.ItemsSource = context.ToDataView(this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.FuzzyCognitiveMatrix);
            this.UpdateHeaders();
        }

        private void Matrix_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var enteredText = ((TextBox)e.EditingElement).Text;
            if (!this.doubleRegex.IsMatch(enteredText))
            {
                ((TextBox) e.EditingElement).Text = "0";
                return;
            }

            enteredText = enteredText.Replace('.', ',');

            var row = e.Row.GetIndex();
            var column = e.Column.DisplayIndex;
            double.TryParse(enteredText, out var value);

            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.SetLinkViaMatrix(row, column, value);
        }

        private void Matrix_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // HACK: чтобы не было исключений.
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                this.Matrix.CommitEdit();
            }
        }

        private void Matrix_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = this.context.Concepts[e.Row.GetIndex()].Name;
        }

        private void UpdateHeaders()
        {
            for (int i = 0; i < this.Matrix.Columns.Count; i++)
            {
                this.Matrix.Columns[i].Header = this.context.Concepts[i].Name;
            }
        }
    }
}
