using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FuzzyCognitiveModel.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для DynamicModellingControl.xaml
    /// </summary>
    public partial class DynamicModellingControl : UserControl
    {
        private const int Steps = 10;

        private FuzzyCognitiveMapViewModel context;

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public DynamicModellingControl()
        {
            InitializeComponent();

            //this.SeriesCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 2",
            //        Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
            //        PointGeometry = null
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 3",
            //        Values = new ChartValues<double> { 4,2,7,2,7 },
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    }
            //};

            //this.Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //this.YFormatter = value => value.ToString("C");

            ////modifying the series collection will animate and update the chart
            //this.SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            ////modifying any series values will also animate and update the chart
            //this.SeriesCollection[3].Values.Add(5d);

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

            var values = this.context.FuzzyCognitiveMap.StartModeling(Steps);

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
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            this.SeriesCollection.Clear();

            if (this.context == null)
            {
                return;
            }

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
    }
}
