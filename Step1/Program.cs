using System.Text.Json;

namespace MDT {
    class Program {
        static async Task Main(string[] args) {
            try {
                // Read the JSON file
                string jsonString = File.ReadAllText("data.json");
                JsonSerializerOptions options = new JsonSerializerOptions { AllowTrailingCommas = true };
                Data? data = JsonSerializer.Deserialize<Data>(jsonString, options);

                if (data is null) {
                    return;
                }

                // Initialize generators
                List<Generator> generators = new List<Generator>();

                data.generators.ForEach( (generatorData) => { 
                    generators.Add(new Generator(generatorData)); 
                } );

                // Process Generator Logic
                try {
                    await GeneratorController.DoGeneratorProcessing(generators, data.datasets, (decimal result, Generator generator) => {
                        // Do something with the result (in this case write it to stdout)
                        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {generator.name} {result}");
                    });
                }
                catch( Exception e ) {
                    Console.WriteLine( "Failure in Generator Processing [" + e.Message + "]" );
                }
            }
            catch( Exception e )
            {
                Console.WriteLine("Failed to execute. Likely caused by missing 'data.json' file in the same folder as the exe\n\n[" + e.Message + "]");
            }
        }
    }
}
