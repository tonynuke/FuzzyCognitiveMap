using System;
using System.Windows;
using FuzzyCognitiveModel.ViewModels;

namespace FuzzyCognitiveModel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FuzzyCognitiveMapViewModel FuzzyCognitiveViewModel { get; set; } = new FuzzyCognitiveMapViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                this.FuzzyCognitiveViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.Save(filename);
            }
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                this.FuzzyCognitiveViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.Load(filename);
            }
        }
    }
}
