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
        private FuzzyCognitiveMapViewModel context;

        public StaticModelControl()
        {
            InitializeComponent();

            this.Consonance.LoadingRow += this.Matrix_OnLoadingRow;
            this.Dissonance.LoadingRow += this.Matrix_OnLoadingRow;
            this.Influence.LoadingRow += this.Matrix_OnLoadingRow;

            this.Consonance.Loaded += this.MatrixOnLoaded;
            this.Dissonance.Loaded += this.MatrixOnLoaded;
            this.Influence.Loaded += this.MatrixOnLoaded;

            this.UpdateHeaders(this.Consonance);
            this.UpdateHeaders(this.Dissonance);
            this.UpdateHeaders(this.Influence);
        }

        private void MatrixOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.UpdateHeaders(sender as DataGrid);
        }

        private void UpdateHeaders(DataGrid dataGrid)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                dataGrid.Columns[i].Header = this.context.Concepts[i].Name;
            }
        }

        private void Matrix_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = this.context.Concepts[e.Row.GetIndex()].Name;
        }

        private void StaticModellerControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            var model = this.context;
            if (model != null)
            {
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
    }
}
