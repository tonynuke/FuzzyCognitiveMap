using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FuzzyCognitiveModel.ViewModels;

namespace FuzzyCognitiveModel.Views
{
    /// <summary>
    /// Логика взаимодействия для GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        private readonly GraphAdapter GraphAdapter = new GraphAdapter();

        private FuzzyCognitiveMapViewModel context => (FuzzyCognitiveMapViewModel)this.DataContext;

        public GraphControl()
        {
            InitializeComponent();
        }

        private void GraphControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(this.context == null)
                return;

            var map = this.context.FuzzyCognitiveModel.FuzzyCognitiveMap;
            var imagePath = this.GraphAdapter.GenerateGraphImage(map);
            Uri uri = new Uri(imagePath);
            this.Graph.Source = BitmapFromUri(uri);

            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }

        private void GraphControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.context.FuzzyCognitiveModel.FuzzyCognitiveMap.PropertyChanged -= FuzzyCognitiveMapOnPropertyChanged;
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var map = this.context.FuzzyCognitiveModel.FuzzyCognitiveMap;
            var imagePath = this.GraphAdapter.GenerateGraphImage(map);
            Uri uri = new Uri(imagePath);
            this.Graph.Source = BitmapFromUri(uri);
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}
