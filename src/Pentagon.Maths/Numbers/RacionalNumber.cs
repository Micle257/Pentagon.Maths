﻿namespace Pentagon.Maths.Numbers {
    using System;

    public class WholeNumber : Number
    {
        public int Value { get; }

        public WholeNumber(int value)
        {
            Value = value;
        }
        public override NumberSet NumberSet => NumberSet.Whole;
    }

    public class RacionalNumber : Number {
        public double Value { get; }

        public RacionalNumber(double value)
        {
            Value = value;
        }

        public override NumberSet NumberSet => NumberSet.Racional;
    }
}