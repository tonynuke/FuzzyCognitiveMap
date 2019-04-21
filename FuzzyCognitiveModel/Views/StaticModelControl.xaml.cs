using System;
using System.Windows;
using System.Windows.Controls;
using FuzzyCognitiveModel.ViewModels;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для StaticModellerControl.xaml
    /// </summary>
    public partial class StaticModelControl : UserControl
    {
        private FuzzyCognitiveMapViewModel context => (FuzzyCognitiveMapViewModel)this.DataContext;

        public StaticModelControl()
        {
            InitializeComponent();

            this.Loaded += OnStaticModellerControlLoaded;
            this.Unloaded += OnStaticModellerControlUnloaded;
        }

        private void MatrixOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.UpdateHeaders(sender as DataGrid);
        }

        private void Matrix_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var rowIndex = e.Row.GetIndex();
            e.Row.Header = this.context.Concepts[rowIndex].Name;
        }

        private void OnStaticModellerControlLoaded(object sender, RoutedEventArgs e)
        {
            this.Consonance.Loaded += this.MatrixOnLoaded;
            this.Dissonance.Loaded += this.MatrixOnLoaded;
            this.Influence.Loaded += this.MatrixOnLoaded;

            var model = this.context;
            if (model != null)
            {
                this.IsEnabled = model.FuzzyCognitiveModel.CanBeModeled;
                if (this.IsEnabled == false)
                {
                    Dispatcher.BeginInvoke((Action)(() => this.StaticModel.SelectedIndex = 0));
                    return;
                }

                this.Consonance.LoadingRow += this.Matrix_OnLoadingRow;
                this.Dissonance.LoadingRow += this.Matrix_OnLoadingRow;
                this.Influence.LoadingRow += this.Matrix_OnLoadingRow;

                this.ConsonanceInfluence.LoadingRow += this.Matrix_OnLoadingRow;
                this.DissonanceInfluence.LoadingRow += this.Matrix_OnLoadingRow;
                this.ConceptInfluence.LoadingRow += this.Matrix_OnLoadingRow;

                model.FuzzyCognitiveModel.StartStaticicModeling();

                this.Consonance.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.Consonance);
                this.ConsonanceInfluence.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.ConsonanceInfluence);

                this.Dissonance.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.Dissonance);
                this.DissonanceInfluence.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.DissonanceInfluence);

                this.Influence.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.Influence);
                this.ConceptInfluence.ItemsSource = model.ToDataView(model.FuzzyCognitiveModel.ConceptInfluence);

                this.Probability.Content = model.FuzzyCognitiveModel.Probability;
                this.Damage.Content = model.FuzzyCognitiveModel.Damage;
            }
        }

        private void OnStaticModellerControlUnloaded(object sender, RoutedEventArgs e)
        {
            this.Consonance.Loaded -= this.MatrixOnLoaded;
            this.Dissonance.Loaded -= this.MatrixOnLoaded;
            this.Influence.Loaded -= this.MatrixOnLoaded;

            this.Consonance.LoadingRow -= this.Matrix_OnLoadingRow;
            this.Dissonance.LoadingRow -= this.Matrix_OnLoadingRow;
            this.Influence.LoadingRow -= this.Matrix_OnLoadingRow;

            this.ConsonanceInfluence.LoadingRow -= this.Matrix_OnLoadingRow;
            this.DissonanceInfluence.LoadingRow -= this.Matrix_OnLoadingRow;
            this.ConceptInfluence.LoadingRow -= this.Matrix_OnLoadingRow;
        }

        private void UpdateHeaders(DataGrid dataGrid)
        {
            var iterationsCount = Math.Min(this.context.Concepts.Count, dataGrid.Columns.Count);

            for (int i = 0; i < iterationsCount; i++)
            {
                dataGrid.Columns[i].Header = this.context.Concepts[i].Name;
            }
        }
    }
}
