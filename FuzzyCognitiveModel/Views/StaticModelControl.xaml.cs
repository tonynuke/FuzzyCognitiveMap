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
