namespace Pentagon.Maths.Numbers {
    using System;
    using System.Numerics;

    public class ComplexNumber : Number
    {
        Complex _value;

        public Complex Value => _value;

        public ComplexNumber(Complex value)
        {
            _value = value;
        }

        public override INumber Add(INumber second)
        {
            switch (second)
            {
                case ComplexNumber cn:
                    return new ComplexNumber(Complex.Add(Value, cn.Value));

                case RacionalNumber rn:
                    return new ComplexNumber(Complex.Add(Value, new Complex(rn.Value, 0)));
            }

            throw new ArgumentException();
        }

        public override INumber Multiple(INumber second)
        {
            throw new System.NotImplementedException();
        }
    }
}