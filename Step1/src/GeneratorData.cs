namespace MDT {
    public class GeneratorData {
        public string name { get; set; }
        public int interval { get; set; }
        public string operation { get; set; }

        public GeneratorData() {
            name = "";
            interval = 1;
            operation = "min"; // I would prefer this to be an Enum so I can later use it as a drop down in XAML
        }
    }
}
