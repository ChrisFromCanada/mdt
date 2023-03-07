using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDT {
    public class GeneratorController {
        // 1 thread that calculates all generators so the results can be kept in sequential order
        public static async Task DoGeneratorProcessing(List<Generator> generators, List<List<decimal>> datasets, Action<decimal, Generator> processResult) {
            int maxInterval = generators.Max((gen) => gen.interval);
            int maxSeconds = maxInterval * datasets.Count;

            for (int currentSecond = 0; currentSecond < maxSeconds; currentSecond++) {
                // Check for cancelled task here

                generators.ForEach( generator => {
                    bool doesNumSecondsAlignWithInterval = currentSecond % generator.interval == 0;

                    if (doesNumSecondsAlignWithInterval) {
                        int datasetIndex = (currentSecond / generator.interval);

                        if(datasetIndex < datasets.Count) {
                            decimal result = generator.Generate(datasets[datasetIndex]);
                            processResult( result, generator );
                        }
                    }
                } );

                if ( currentSecond < maxSeconds - maxInterval ) {
                    await Task.Delay(1000);
                }
            }
        }
    }
}
