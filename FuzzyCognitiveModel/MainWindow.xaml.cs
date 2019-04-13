using System;
using System.Collections.Generic;
using System.Windows;
using Core.Concept;
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

            var concepts = new List<Concept>()
            {
                new Concept {
                    Description = "выход из строя сервера",
                    Type = ConceptType.Ресурс,
                    Value = 20900
                },
                new Concept {
                    Description = "нар. цел магистр.",
                    Type = ConceptType.Ресурс,
                    Value = 2200
                },
                new Concept {
                    Description = "утечка информации",
                    Type = ConceptType.Ресурс,
                    Value = 10000
                },

                new Concept {
                    Description = "отсут тех обсл",
                    Type = ConceptType.Уязвимость,
                    Value = 0.12
                },
                new Concept {
                    Description = "отсут инстр",
                    Type = ConceptType.Уязвимость,
                    Value = 0.15
                },
                new Concept {
                    Description = "ошибки в проект",
                    Type = ConceptType.Уязвимость,
                    Value = 0.29
                },

                new Concept {
                    Description = "отказ работы сайта",
                    Type = ConceptType.Угроза,
                    Value = 0.09
                },
                new Concept {
                    Description = "порча информации",
                    Type = ConceptType.Угроза,
                    Value = 0.12
                },
            };

            foreach (var c in concepts)
            {
                this.FuzzyCognitiveViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.AddConcept(c);
            }

            Action<int, int, double> act = (i, j, v) => { this.FuzzyCognitiveViewModel.FuzzyCognitiveModel.FuzzyCognitiveMap.SetLinkViaMatrix(i - 1, j - 1, v); };
            act(1, 2, 0.3);
            act(1, 7, 0.8);
            act(1, 8, 0.2);

            act(2, 1, 0.3);
            act(2, 4, -0.12);
            act(2, 5, -0.2);
            act(2, 7, 0.8);

            act(3, 5, -0.32);
            act(3, 7, 0.33);
            act(3, 8, 1);

            act(4, 1, 0.2);
            act(4, 2, 0.2);
            act(4, 3, 0.1);
            act(4, 7, 0.23);
            act(4, 8, 0.13);

            act(5, 1, 0.3);
            act(5, 2, 0.2);
            act(5, 3, 0.06);
            act(5, 4, 0.5);

            act(6, 3, 0.8);
            act(6, 7, 0.8);
            act(6, 8, 0.9);

            act(7, 3, 0.56);
            act(7, 4, 0.1);
            act(7, 5, 0.1);
            act(7, 6, -0.7);
            act(7, 8, 0.7);

            act(8, 1, -0.1);
            act(8, 2, -0.12);
            act(8, 3, 1);
            act(8, 6, -0.2);
            act(8, 7, 0.8);
        }
    }
}
