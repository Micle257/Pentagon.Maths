namespace Pentagon.Maths.Numbers {
    using System;

    public class RacionalNumber : Number {
        public double Value { get; }

        public RacionalNumber(double value)
        {
            Value = value;
        }

        public override NumberSet NumberSet => NumberSet.Racional;
    }
}