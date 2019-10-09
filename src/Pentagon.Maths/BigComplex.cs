// -----------------------------------------------------------------------
//  <copyright file="BigRational.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;

    [Serializable]
    public struct BigComplex : IEquatable<BigComplex>, ISerializable
    {
        public BigComplex(BigRational realPart, BigRational imaginaryPart)
        {
            Real = realPart;
            Imaginary = imaginaryPart;
        }

        BigComplex(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(paramName: "info");

            Real = (BigRational) info.GetValue(name: "Real", typeof(BigRational));
            Imaginary = (BigRational) info.GetValue(name: "Imaginary", typeof(BigRational));
        }

        public BigComplex(Complex complex) : this(complex.Real, complex.Imaginary)
        {
            
        }

        public static BigComplex Zero { get; } = new BigComplex(BigRational.Zero, BigRational.Zero);

        public static BigComplex One { get; } = new BigComplex(BigRational.One, BigRational.Zero);

        public static BigComplex ImaginaryOne { get; } = new BigComplex(BigRational.Zero, BigRational.One);

        public BigRational Real { get; private set; }

        public BigRational Imaginary { get; private set; }

        public static BigComplex FromPolarCoordinates(Double magnitude, Double phase) /* Factory method to take polar inputs and create a Complex object */
        {
            return new Complex((magnitude * Math.Cos(phase)), (magnitude * Math.Sin(phase)));
        }

        public static BigComplex Negate(BigComplex value)
        {
            return -value;
        }

        public static BigComplex Add(BigComplex left, BigComplex right)
        {
            return left + right;
        }

        public static BigComplex Subtract(BigComplex left, BigComplex right)
        {
            return left - right;
        }

        public static BigComplex Multiply(BigComplex left, BigComplex right)
        {
            return left * right;
        }

        public static BigComplex Divide(BigComplex dividend, BigComplex divisor)
        {
            return dividend / divisor;
        }

        public static Double Abs(BigComplex value)
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Conjugate(BigComplex value)
        {
            // Conjugate of a Complex number: the conjugate of x+i*y is x-i*y 

            return (new BigComplex(value.Real, (-value.Imaginary)));

        }
        public static BigComplex Reciprocal(BigComplex value)
        {
            // Reciprocal of a Complex number : the reciprocal of x+i*y is 1/(x+i*y)
            if ((value.Real == 0) && (value.Imaginary == 0))
            {
                return BigComplex.Zero;
            }

            return BigComplex.One / value;
        }

        public static BigComplex Sin(BigComplex value)
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Sinh(BigComplex value) /* Hyperbolic sin */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");

        }
        public static BigComplex Asin(BigComplex value) /* Arcsin */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Cos(BigComplex value)
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Cosh(BigComplex value) /* Hyperbolic cos */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Acos(BigComplex value) /* Arccos */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Tan(BigComplex value)
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Tanh(BigComplex value) /* Hyperbolic tan */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Atan(BigComplex value) /* Arctan */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Log(BigComplex value) /* Log of the complex number value to the base of 'e' */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");

        }
        public static BigComplex Log(BigComplex value, BigRational baseValue) /* Log of the complex number to a the base of a double */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }
        public static BigComplex Log10(BigComplex value) /* Log to the base of 10 of the complex number */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }
        public static BigComplex Exp(BigComplex value) /* The complex number raised to e */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Sqrt(BigComplex value) /* Square root ot the complex number */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Pow(BigComplex value, BigComplex power) /* A complex number raised to another complex number */
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        public static BigComplex Pow(BigComplex value, BigRational power) // A complex number raised to a real number 
        {
            throw new InvalidOperationException("Cannot compute iracional numbers.");
        }

        #region Operators

        public static bool operator ==(BigComplex x, BigComplex y) => x.Equals(y);

        public static bool operator !=(BigComplex x, BigComplex y) => !x.Equals(y);

        public static BigComplex operator +(BigComplex r) => r;

        public static BigComplex operator -(BigComplex r) => new BigComplex(-r.Real,- r.Imaginary);
        
        public static BigComplex operator +(BigComplex r1, BigComplex r2) => new BigComplex(r1.Real + r2.Real, r1.Imaginary + r2.Imaginary);

        public static BigComplex operator -(BigComplex r1, BigComplex r2) => new BigComplex(r1.Real + r2.Real, r1.Imaginary + r2.Imaginary);

        public static BigComplex operator *(BigComplex r1, BigComplex r2)
        {
            // Multiplication:  (a + bi)(c + di) = (ac -bd) + (bc + ad)i
            var result_Realpart = (r1.Real * r2.Real) - (r1.Imaginary * r2.Imaginary);
            var result_Imaginarypart = (r1.Imaginary * r2.Real) + (r1.Real * r2.Imaginary);

            return (new BigComplex(result_Realpart, result_Imaginarypart));
        }

        public static BigComplex operator /(BigComplex r1, BigComplex r2)
        {
            // Division : Smith's formula.
            var a = r1.Real;
            var b = r1.Imaginary;
            var c = r2.Real;
            var d = r2.Imaginary;

            if (BigRational.Abs(d) < BigRational.Abs(c))
            {
                var doc = d / c;

                return new BigComplex((a + b * doc) / (c + d * doc), (b - a * doc) / (c + d * doc));
            }
            else
            {
                var cod = c / d;

                return new BigComplex((b + a * cod) / (d + c * cod), (-a + b * cod) / (d + c * cod));
            }
        }
        
        public static explicit operator Complex(BigComplex value) => new Complex((double) value.Real, (double)value.Imaginary);
        
        public static implicit operator BigComplex(Complex value) => new BigComplex(value);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is BigComplex))
                return false;

            return Equals((BigComplex) obj);
        }

        public override int GetHashCode()
        {
            var n1 = 99999997;
            var hash_real = Real.GetHashCode() % n1;
            var hash_imaginary = Imaginary.GetHashCode();
            var final_hashcode = hash_real ^ hash_imaginary;
            return (final_hashcode);
        }
        
        public bool Equals(BigComplex other)
        {
            return ((Real.Equals(other.Real)) && (Imaginary.Equals(other.Imaginary)));
        }
        
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(paramName: "info");

            info.AddValue(name: "Real", Real);
            info.AddValue(name: "Imaginary", Imaginary);
        }

        const string TupleFormat = "({0}; {1})";
        
        public override string ToString()
        {
            return ToString(TupleFormat);
        }

        public string ToString(string format)
        {
            return (string.Format(CultureInfo.CurrentCulture, format, Real.ToString(), Imaginary.ToString()));
        }

        public string ToString(bool useMathNotation)
        {
            if (useMathNotation == false)
                return ToString();

            var real = Real.ToString();
            var imag = Imaginary.ToString();

            var writeReal = (Real == BigRational.Zero);
            var writeImag = (Imaginary == BigRational.Zero);
            var isImagNegative = Imaginary < BigRational.Zero;

            var sb = new StringBuilder();

            if (writeReal)
                sb.Append($"{Real}");

            if (writeReal && writeImag)
            {
                sb.Append(isImagNegative ? " - " : " + ");
            }

            if (writeImag)
                sb.Append(Imaginary.Sign == 0 ? $"{Imaginary.Numerator + 2 * Imaginary.Numerator}" : $"{Imaginary}");

            return sb.ToString();
        }
    }
}