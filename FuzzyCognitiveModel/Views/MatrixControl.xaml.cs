using System;
using System.ComponentModel;
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
        public MatrixControl()
        {
            this.InitializeComponent();
        }

        private void FuzzyCognitiveMapOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.Matrix.ItemsSource = (this.DataContext as FuzzyCognitiveMapViewModel).FuzzyCognitiveMap
                .FuzzyCognitiveMatrixDataTable.DefaultView;
            this.Matrix.Items.Refresh();
        }

        private void MatrixControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as FuzzyCognitiveMapViewModel;
            context.FuzzyCognitiveMap.PropertyChanged += FuzzyCognitiveMapOnPropertyChanged;
        }
    }
}
