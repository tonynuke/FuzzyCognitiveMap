using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FuzzyCognitiveModel.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для DynamicModellingControl.xaml
    /// </summary>
    public partial class DynamicModelControl : UserControl
    {
        private const int Steps = 10;

        private FuzzyCognitiveMapViewModel context => (FuzzyCognitiveMapViewModel)this.DataContext;

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public DynamicModelControl()
        {
            InitializeComponent();
            this.Chart.DataContext = this;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.SeriesCollection.Clear();
            foreach (var concept in this.context.Concepts)
            {
                var series = new LineSeries
                {
                    Title = concept.Name,
                    Values = new ChartValues<double> { concept.Value }
                };
                this.SeriesCollection.Add(series);
            }

            var values = this.context.FuzzyCognitiveModel.StartDynamicModeling();

            foreach (var value in values)
            {
                for (int i = 0; i < this.context.Concepts.Count; i++)
                {
                    this.SeriesCollection.Single(sc => sc.Title == this.context.Concepts[i].Name).Values.Add(value[i]);
                }
            }
        }

        private void DynamicModellingControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.context == null)
            {
                return;
            }

            this.UpdateChart();
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }

        private void DynamicModelControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged -= FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.UpdateChart();
        }

        private void UpdateChart()
        {
            this.SeriesCollection.Clear();
            foreach (var concept in this.context.Concepts)
            {
                var series = new LineSeries
                {
                    Title = concept.Name,
                    Values = new ChartValues<double> { concept.Value }
                };
                this.SeriesCollection.Add(series);
            }
        }

        private static readonly Regex _regex = new Regex("[0-9]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }
    }
}
