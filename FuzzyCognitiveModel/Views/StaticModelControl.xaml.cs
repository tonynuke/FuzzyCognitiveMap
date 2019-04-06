using System.ComponentModel;
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
        }

        private void StaticModellerControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.context = this.DataContext as FuzzyCognitiveMapViewModel;
            var fuzzyCognitiveMapViewModel = this.context;
            if (fuzzyCognitiveMapViewModel != null)
            {
                fuzzyCognitiveMapViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
                fuzzyCognitiveMapViewModel.FuzzyCognitiveModel.StartStaticicModeling();
                this.Matrix.ItemsSource = context.ToDataView(this.context.FuzzyCognitiveModel.Consonance);
            }
        }

        private void StaticModelControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged -= FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.context.FuzzyCognitiveModel.StartStaticicModeling();
            this.Matrix.ItemsSource = context.ToDataView(this.context.FuzzyCognitiveModel.Consonance);
        }
    }
}
