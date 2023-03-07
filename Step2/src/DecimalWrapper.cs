using System.Collections.Generic;

namespace MDT {
    public class DecimalWrapper {
        public decimal value { get; set; }

        public DecimalWrapper(decimal value) {
            this.value = value;
        }

        public static List<List<decimal>> UnwrapDecimals(List<List<DecimalWrapper>> listOfDecimalWrappers) {
            List<List<decimal>> datasets = new List<List<decimal>>();

            listOfDecimalWrappers.ForEach( wrappedDecimals => {
                List<decimal> newDoubles = new List<decimal>();

                wrappedDecimals.ForEach( decimalValue => { 
                    newDoubles.Add(decimalValue.value);
                } );

                datasets.Add(newDoubles);
            });

            return datasets;
        }
    }
}
