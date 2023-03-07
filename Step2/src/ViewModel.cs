using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDT {
    public class ViewModel : INotifyPropertyChanged {
        private Model model;

        // Simple fields for enabling/disabling buttons

        private bool jsonIsLoaded = false;
        private bool isGenerating = false;

        public ViewModel() {
            model = new Model(new ObservableCollection<List<DecimalWrapper>>(), new ObservableCollection<Generator>());
        }

        public string FilePath {
            get => model.FilePath;
            set {
                model.FilePath = value;
                OnPropertyChange(nameof(FilePath));

                parseJsonFile();
            }
        }

        public string ResultsText {
            get => model.ResultsText;
            set {
                model.ResultsText = value;
                OnPropertyChange(nameof(ResultsText));
            }
        }

        public ObservableCollection<Generator> Generators {
            get => model.Generators;
            set {
                model.Generators = value;
                OnPropertyChange(nameof(Generators));
            }
        }

        public ObservableCollection<List<DecimalWrapper>> Datasets {
            get => model.Datasets;
            set {
                model.Datasets = value;
                OnPropertyChange(nameof(Datasets));
            }
        }

        private void parseJsonFile() {
            using (StreamReader file = File.OpenText(model.FilePath)) {
                JsonSerializer serializer = new JsonSerializer();
                Data data = (Data)serializer.Deserialize(file, typeof(Data));


                if (data is null) {
                    return;
                }

                Datasets.Clear();
                
                data.datasets.ForEach(dataset => {
                    List<DecimalWrapper> doubles = new List<DecimalWrapper>();

                    dataset.ForEach( decimalValue => {
                        doubles.Add(new DecimalWrapper(decimalValue));
                    } );

                    Datasets.Add(doubles);
                });

                OnPropertyChange("Datasets");

                // Initialize generators
                Generators.Clear();

                data.generators.ForEach(generatorData => {
                    Generators.Add(new Generator(generatorData));
                });

                OnPropertyChange("Generators");
            }
        }

        private ICommand browseCommand;
        public ICommand BrowseCommand => browseCommand ?? (browseCommand = new RelayCommand(Browse, obj => !isGenerating));

        private ICommand processGeneratorsCommand;
        public ICommand ProcessGeneratorsCommand => processGeneratorsCommand ?? (processGeneratorsCommand = new RelayCommand(ProcessGenerators, obj => !isGenerating && jsonIsLoaded));
        
        private ICommand addValueCommand;
        public ICommand AddValueCommand => addValueCommand ?? (addValueCommand = new RelayCommand(AddValue, obj => !isGenerating));
        
        private ICommand removeValueCommand;
        public ICommand RemoveValueCommand => removeValueCommand ?? (removeValueCommand = new RelayCommand(RemoveValue, obj => !isGenerating));
        
        private void Browse(object obj) {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog {
                DefaultExt = "json",
                Filter = $"Files (*.json)|*.json"
            };

            if (dlg.ShowDialog() == true) {
                FilePath = dlg.FileName;
                jsonIsLoaded = true;
            }
        }

        private void AddValue(object obj) {
            int index = Datasets.IndexOf((List<DecimalWrapper>)obj);

            if ( index >= 0 ) {
                ObservableCollection<List<DecimalWrapper>> newDatasets = cloneDatasets();
                newDatasets[index].Add(new DecimalWrapper(0));
                Datasets = newDatasets;
            }
        }

        private void RemoveValue(object obj) {
            ObservableCollection<List<DecimalWrapper>> newDatasets = cloneDatasets();

            newDatasets.ToList().ForEach( dataset => {
                if (dataset.Contains(obj)) {
                    dataset.Remove((DecimalWrapper)obj);
                }
            });

            Datasets = newDatasets;
        }

        private async void ProcessGenerators(object obj) {
            isGenerating = true;

            ResultsText = "Generating Results...\n\n";

            List<List<decimal>> datasets = DecimalWrapper.UnwrapDecimals(Datasets.ToList());

            await GeneratorController.DoGeneratorProcessing(Generators.ToList(), datasets, (value, generator) => {
                ResultsText += $"{DateTime.Now.ToString("HH:mm:ss")} {generator.name} {value}\n";
            });

            ResultsText += "\nProcessing Complete!";

            isGenerating = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private ObservableCollection<List<DecimalWrapper>> cloneDatasets() {
            return new ObservableCollection<List<DecimalWrapper>>(Datasets.Select(dataset => new List<DecimalWrapper>(dataset)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
