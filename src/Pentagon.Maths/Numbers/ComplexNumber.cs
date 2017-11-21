namespace Pentagon.Maths.Numbers
{
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

        public override NumberSet NumberSet => NumberSet.Complex;
    }
}