using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MDT {
    public class Model {
        public ObservableCollection<List<DecimalWrapper>> Datasets { get; set; }
        public ObservableCollection<Generator> Generators { get; set; }

        public string FilePath { get; set; }
        public string ResultsText { get; set; }

        public Model(ObservableCollection<List<DecimalWrapper>> datasets, ObservableCollection<Generator> generators) {
            Datasets = datasets;
            Generators = generators;
            FilePath = "No file specified";
            ResultsText = "Load a JSON file and then press 'Run Generator Logic'";
        }
    }
}
