using System.Collections.Generic;
using System.Linq;

namespace MDT {
    public class Generator {
        public string name { get; set; }
        public int interval { get; set; }
        public string operation { get; set; }

        public Generator(GeneratorData data) {
            this.name = data.name;
            this.interval = data.interval;
            this.operation = data.operation;
        }

        public decimal Generate(List<decimal> dataset) {
            if (dataset.Count > 0) {
                if (operation.Equals("sum")) {
                    return dataset.Sum();
                }
                else if (operation.Equals("average")) {
                    return (decimal)dataset.Average();
                }
                else if (operation.Equals("max")) {
                    return dataset.Max();
                }
                else if (operation.Equals("min")) {
                    return dataset.Min();
                }
            }

            return 0;
        }
    }
}
