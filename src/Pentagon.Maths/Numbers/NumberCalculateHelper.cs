namespace Pentagon.Maths.Numbers {
    using System;
    using System.Numerics;

    public static class NumberCalculateHelper
    {
        public static INumber Add(INumber left, INumber right)
        {
            if (left.NumberSet >= right.NumberSet)
            {
                return AddCore(left, right);
            }
            else
            {
                return AddCore(right, left);
            }
        }

        static INumber AddCore(INumber left, INumber right)
        {
            switch (left)
            {
                case ComplexNumber c:
                    return new ComplexNumber(Complex.Add(c.Value, ToComplex(right)));

                case RacionalNumber q:
                    return new RacionalNumber(q.Value + ToRacional(right));

                case WholeNumber w:
                    return new WholeNumber(w.Value + ToWhole(right));

                default:
                    return null;
            }
        }
        static int ToWhole(INumber right)
        {
            switch (right)
            {
                case RacionalNumber q:
                case ComplexNumber c:
                    throw new ArithmeticException();
                    
                case WholeNumber w:
                    return w.Value;

                default:
                    return 0;
            }
        }

        static double ToRacional(INumber right)
        {
            switch (right)
            {
                case ComplexNumber c:
                    return c.Value.Real;

                case RacionalNumber q:
                    return q.Value;

                case WholeNumber w:
                    return w.Value;

                default:
                    return 0;
            }
        }

        static Complex ToComplex(INumber right)
        {
            switch (right)
            {
                case ComplexNumber c:
                    return c.Value;

                case RacionalNumber q:
                    return new Complex(q.Value, 0);
                    
                case WholeNumber w:
                    return new Complex(w.Value, 0);

                default:
                    return Complex.Zero;
            }
        }

        public static INumber Multiple(INumber left, INumber right)
        {
            if (left.NumberSet >= right.NumberSet)
            {
                return MultipleCore(left, right);
            }
            else
            {
                return MultipleCore(right, left);
            }
        }

        static INumber MultipleCore(INumber left, INumber right)
        {
            switch (left)
            {
                case ComplexNumber c:
                    return new ComplexNumber(Complex.Multiply(c.Value, ToComplex(right)));

                case RacionalNumber q:
                    return new RacionalNumber(q.Value * ToRacional(right));

                case WholeNumber w:
                    return new WholeNumber(w.Value * ToWhole(right));

                default:
                    return null;
            }
        }
    }
}