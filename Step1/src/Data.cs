using System.Collections.Generic;

namespace MDT {
    public class Data {
        public List<List<decimal>> datasets { get; set; }
        public List<GeneratorData> generators { get; set; }

        public Data() {
            datasets = new List<List<decimal>>();
            generators = new List<GeneratorData>();
        }
    }
}
