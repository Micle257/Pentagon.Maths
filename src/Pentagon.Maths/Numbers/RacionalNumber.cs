namespace Pentagon.Maths.Numbers {
    using System;

    public class RacionalNumber : Number {
        public double Value { get; }

        public RacionalNumber(double value)
        {
            Value = value;
        }

        public override INumber Add(INumber second)
        {
            throw new NotImplementedException();
        }

        public override INumber Multiple(INumber second)
        {
            throw new NotImplementedException();
        }
    }
}