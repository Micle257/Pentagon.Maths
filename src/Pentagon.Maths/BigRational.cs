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
    public struct BigRational : IComparable, IComparable<BigRational>, IDeserializationCallback, IEquatable<BigRational>, ISerializable
    {
        const int DoubleMaxScale = 308;

        const int DecimalScaleMask = 0x00FF0000;

        const int DecimalSignMask = unchecked((int) 0x80000000);

        const int DecimalMaxScale = 28;

        const string FractionSolidus = @"/";

        static readonly BigInteger DoublePrecision = BigInteger.Pow(10, DoubleMaxScale);

        static readonly BigInteger DoubleMaxValue = (BigInteger) double.MaxValue;

        static readonly BigInteger DoubleMinValue = (BigInteger) double.MinValue;

        static readonly BigInteger DecimalPrecision = BigInteger.Pow(10, DecimalMaxScale);

        static readonly BigInteger DecimalMaxValue = (BigInteger) decimal.MaxValue;

        static readonly BigInteger DecimalMinValue = (BigInteger) decimal.MinValue;
        
        public BigRational(BigInteger numerator)
        {
            Numerator = numerator;
            Denominator = BigInteger.One;
        }
        
        public BigRational(double value)
        {
            if (double.IsNaN(value))
                throw new ArgumentException(message: "Argument is not a number", paramName: "value");
            if (double.IsInfinity(value))
                throw new ArgumentException(message: "Argument is infinity", paramName: "value");

            SplitDoubleIntoParts(value, out var sign, out var exponent, out var significand, out var isFinite);

            if (significand == 0)
            {
                this = Zero;
                return;
            }

            Numerator = significand;
            Denominator = 1 << 52;

            if (exponent > 0)
                Numerator = BigInteger.Pow(Numerator, exponent);
            else if (exponent < 0)
                Denominator = BigInteger.Pow(Denominator, -exponent);
            if (sign < 0)
                Numerator = BigInteger.Negate(Numerator);
            Simplify();
        }
        
        public BigRational(decimal value)
        {
            var bits = decimal.GetBits(value);
            if (bits == null || bits.Length != 4 || (bits[3] & ~(DecimalSignMask | DecimalScaleMask)) != 0 || (bits[3] & DecimalScaleMask) > 28 << 16)
                throw new ArgumentException(message: "invalid Decimal", paramName: "value");

            if (value == decimal.Zero)
            {
                this = Zero;
                return;
            }

            // build up the numerator
            var ul = ((ulong) (uint) bits[2] << 32) | (uint) bits[1]; // (hi    << 32) | (mid)
            Numerator = (new BigInteger(ul) << 32) | (uint) bits[0]; // (hiMid << 32) | (low)

            var isNegative = (bits[3] & DecimalSignMask) != 0;
            if (isNegative)
                Numerator = BigInteger.Negate(Numerator);

            // build up the denominator
            var scale = (bits[3] & DecimalScaleMask) >> 16; // 0-28, power of 10 to divide numerator by
            Denominator = BigInteger.Pow(10, scale);

            Simplify();
        }

        public BigRational(BigInteger numerator, BigInteger denominator)
        {
            if (denominator.Sign == 0)
                throw new DivideByZeroException();

            if (numerator.Sign == 0)
            {
                // 0/m -> 0/1
                Numerator = BigInteger.Zero;
                Denominator = BigInteger.One;
            }
            else if (denominator.Sign < 0)
            {
                Numerator = BigInteger.Negate(numerator);
                Denominator = BigInteger.Negate(denominator);
            }
            else
            {
                Numerator = numerator;
                Denominator = denominator;
            }

            Simplify();
        }

        public BigRational(BigInteger whole, BigInteger numerator, BigInteger denominator)
        {
            if (denominator.Sign == 0)
                throw new DivideByZeroException();

            if (numerator.Sign == 0 && whole.Sign == 0)
            {
                Numerator = BigInteger.Zero;
                Denominator = BigInteger.One;
            }
            else if (denominator.Sign < 0)
            {
                Denominator = BigInteger.Negate(denominator);
                Numerator = BigInteger.Negate(whole) * Denominator + BigInteger.Negate(numerator);
            }
            else
            {
                Denominator = denominator;
                Numerator = whole * denominator + numerator;
            }

            Simplify();
        }

        BigRational(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(paramName: "info");

            Numerator = (BigInteger) info.GetValue(name: "Numerator", typeof(BigInteger));
            Denominator = (BigInteger) info.GetValue(name: "Denominator", typeof(BigInteger));
        }
        
        public static BigRational Zero { get; } = new BigRational(BigInteger.Zero);

        public static BigRational One { get; } = new BigRational(BigInteger.One);

        public static BigRational MinusOne { get; } = new BigRational(BigInteger.MinusOne);

        public int Sign => Numerator.Sign;

        public BigInteger Numerator { get; private set; }

        public BigInteger Denominator { get; private set; }

        #region Operators

        public static bool operator ==(BigRational x, BigRational y) => Compare(x, y) == 0;

        public static bool operator !=(BigRational x, BigRational y) => Compare(x, y) != 0;

        public static bool operator <(BigRational x, BigRational y) => Compare(x, y) < 0;

        public static bool operator <=(BigRational x, BigRational y) => Compare(x, y) <= 0;

        public static bool operator >(BigRational x, BigRational y) => Compare(x, y) > 0;

        public static bool operator >=(BigRational x, BigRational y) => Compare(x, y) >= 0;

        public static BigRational operator +(BigRational r) => r;

        public static BigRational operator -(BigRational r) => new BigRational(-r.Numerator, r.Denominator);

        public static BigRational operator ++(BigRational r) => r + One;

        public static BigRational operator --(BigRational r) => r - One;

        public static BigRational operator +(BigRational r1, BigRational r2) => new BigRational(r1.Numerator * r2.Denominator + r1.Denominator * r2.Numerator, r1.Denominator * r2.Denominator);

        public static BigRational operator -(BigRational r1, BigRational r2) => new BigRational(r1.Numerator * r2.Denominator - r1.Denominator * r2.Numerator, r1.Denominator * r2.Denominator);

        public static BigRational operator *(BigRational r1, BigRational r2) => new BigRational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);

        public static BigRational operator /(BigRational r1, BigRational r2) => new BigRational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);

        public static BigRational operator %(BigRational r1, BigRational r2) => new BigRational(r1.Numerator * r2.Denominator % (r1.Denominator * r2.Numerator), r1.Denominator * r2.Denominator);

        // ----- SECTION: explicit conversions from BigRational to numeric base types  ----------------*
        [CLSCompliant(false)]
        public static explicit operator sbyte(BigRational value) => (sbyte) BigInteger.Divide(value.Numerator, value.Denominator);

        [CLSCompliant(false)]
        public static explicit operator ushort(BigRational value) => (ushort) BigInteger.Divide(value.Numerator, value.Denominator);

        [CLSCompliant(false)]
        public static explicit operator uint(BigRational value) => (uint) BigInteger.Divide(value.Numerator, value.Denominator);

        [CLSCompliant(false)]
        public static explicit operator ulong(BigRational value) => (ulong) BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator byte(BigRational value) => (byte) BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator short(BigRational value) => (short) BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator int(BigRational value) => (int) BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator long(BigRational value) => (long) BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator BigInteger(BigRational value) => BigInteger.Divide(value.Numerator, value.Denominator);

        public static explicit operator float(BigRational value) => (float) (double) value;

        public static explicit operator double(BigRational value)
        {
            // The Double value type represents a double-precision 64-bit number with
            // values ranging from -1.79769313486232e308 to +1.79769313486232e308
            // values that do not fit into this range are returned as +/-Infinity
            if (SafeCastToDouble(value.Numerator) && SafeCastToDouble(value.Denominator))
                return (double) value.Numerator / (double) value.Denominator;

            // scale the numerator to preseve the fraction part through the integer division
            var denormalized = value.Numerator * DoublePrecision / value.Denominator;
            if (denormalized.IsZero)
                return value.Sign < 0 ? BitConverter.Int64BitsToDouble(unchecked((long) 0x8000000000000000)) : 0d; // underflow to -+0

            double result = 0;
            var isDouble = false;
            var scale = DoubleMaxScale;

            while (scale > 0)
            {
                if (!isDouble)
                {
                    if (SafeCastToDouble(denormalized))
                    {
                        result = (double) denormalized;
                        isDouble = true;
                    }
                    else
                        denormalized = denormalized / 10;
                }

                result = result / 10;
                scale--;
            }

            if (!isDouble)
                return value.Sign < 0 ? double.NegativeInfinity : double.PositiveInfinity;
            return result;
        }

        public static explicit operator decimal(BigRational value)
        {
            // The Decimal value type represents decimal numbers ranging
            // from +79,228,162,514,264,337,593,543,950,335 to -79,228,162,514,264,337,593,543,950,335
            // the binary representation of a Decimal value is of the form, ((-2^96 to 2^96) / 10^(0 to 28))
            if (SafeCastToDecimal(value.Numerator) && SafeCastToDecimal(value.Denominator))
                return (decimal) value.Numerator / (decimal) value.Denominator;

            // scale the numerator to preseve the fraction part through the integer division
            var denormalized = value.Numerator * DecimalPrecision / value.Denominator;
            if (denormalized.IsZero)
                return decimal.Zero; // underflow - fraction is too small to fit in a decimal
            for (var scale = DecimalMaxScale; scale >= 0; scale--)
            {
                if (!SafeCastToDecimal(denormalized))
                    denormalized = denormalized / 10;
                else
                {
                    var dec = new DecimalUInt32();
                    dec.dec = (decimal) denormalized;
                    dec.flags = (dec.flags & ~DecimalScaleMask) | (scale << 16);
                    return dec.dec;
                }
            }

            throw new OverflowException(message: "Value was either too large or too small for a Decimal.");
        }

        // ----- SECTION: implicit conversions from numeric base types to BigRational  ----------------*

        [CLSCompliant(false)]
        public static implicit operator BigRational(sbyte value) => new BigRational((BigInteger) value);

        [CLSCompliant(false)]
        public static implicit operator BigRational(ushort value) => new BigRational((BigInteger) value);

        [CLSCompliant(false)]
        public static implicit operator BigRational(uint value) => new BigRational((BigInteger) value);

        [CLSCompliant(false)]
        public static implicit operator BigRational(ulong value) => new BigRational((BigInteger) value);

        public static implicit operator BigRational(byte value) => new BigRational((BigInteger) value);

        public static implicit operator BigRational(short value) => new BigRational((BigInteger) value);

        public static implicit operator BigRational(int value) => new BigRational((BigInteger) value);

        public static implicit operator BigRational(long value) => new BigRational((BigInteger) value);

        public static implicit operator BigRational(BigInteger value) => new BigRational(value);

        public static implicit operator BigRational(float value) => new BigRational(value);

        public static implicit operator BigRational(double value) => new BigRational(value);

        public static implicit operator BigRational(decimal value) => new BigRational(value);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is BigRational))
                return false;
            return Equals((BigRational) obj);
        }

        public override int GetHashCode() => (Numerator / Denominator).GetHashCode();
        
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is BigRational))
                throw new ArgumentException(message: "Argument must be of type BigRational", paramName: "obj");
            return Compare(this, (BigRational) obj);
        }
        
        public int CompareTo(BigRational other) => Compare(this, other);
        
        public bool Equals(BigRational other)
        {
            if (Denominator == other.Denominator)
                return Numerator == other.Numerator;
            return Numerator * other.Denominator == Denominator * other.Numerator;
        }
        
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            try
            {
                if (Denominator.Sign == 0 || Numerator.Sign == 0)
                {
                    Numerator = BigInteger.Zero;
                    Denominator = BigInteger.One;
                }
                else if (Denominator.Sign < 0)
                {
                    Numerator = BigInteger.Negate(Numerator);
                    Denominator = BigInteger.Negate(Denominator);
                }

                Simplify();
            }
            catch (ArgumentException e)
            {
                throw new SerializationException(message: "invalid serialization data", e);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(paramName: "info");

            info.AddValue(name: "Numerator", Numerator);
            info.AddValue(name: "Denominator", Denominator);
        }
        
        public static BigRational Abs(BigRational r) => r.Numerator.Sign < 0 ? new BigRational(BigInteger.Abs(r.Numerator), r.Denominator) : r;

        public static BigRational Negate(BigRational r) => new BigRational(BigInteger.Negate(r.Numerator), r.Denominator);

        public static BigRational Invert(BigRational r) => new BigRational(r.Denominator, r.Numerator);

        public static BigRational Add(BigRational x, BigRational y) => x + y;

        public static BigRational Subtract(BigRational x, BigRational y) => x - y;

        public static BigRational Multiply(BigRational x, BigRational y) => x * y;

        public static BigRational Divide(BigRational dividend, BigRational divisor) => dividend / divisor;

        public static BigRational Remainder(BigRational dividend, BigRational divisor) => dividend % divisor;

        public static BigRational DivRem(BigRational dividend, BigRational divisor, out BigRational remainder)
        {
            // a/b / c/d  == (ad)/(bc)
            // a/b % c/d  == (ad % bc)/bd

            // (ad) and (bc) need to be calculated for both the division and the remainder operations.
            var ad = dividend.Numerator * divisor.Denominator;
            var bc = dividend.Denominator * divisor.Numerator;
            var bd = dividend.Denominator * divisor.Denominator;

            remainder = new BigRational(ad % bc, bd);
            return new BigRational(ad, bc);
        }

        public static BigRational Pow(BigRational baseValue, BigInteger exponent)
        {
            if (exponent.Sign == 0)
            {
                // 0^0 -> 1
                // n^0 -> 1
                return One;
            }

            if (exponent.Sign < 0)
            {
                if (baseValue == Zero)
                    throw new ArgumentException(message: "cannot raise zero to a negative power", paramName: "baseValue");

                // n^(-e) -> (1/n)^e
                baseValue = Invert(baseValue);
                exponent = BigInteger.Negate(exponent);
            }

            var result = baseValue;
            while (exponent > BigInteger.One)
            {
                result = result * baseValue;
                exponent--;
            }

            return result;
        }

        // Least Common Denominator (LCD)
        //
        // The LCD is the least common multiple of the two denominators.  For instance, the LCD of
        // {1/2, 1/4} is 4 because the least common multiple of 2 and 4 is 4.  Likewise, the LCD
        // of {1/2, 1/3} is 6.
        //       
        // To find the LCD:
        //
        // 1) Find the Greatest Common Divisor (GCD) of the denominators
        // 2) Multiply the denominators together
        // 3) Divide the product of the denominators by the GCD
        public static BigInteger LeastCommonDenominator(BigRational x, BigRational y) => x.Denominator * y.Denominator / BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);

        public static int Compare(BigRational r1, BigRational r2) => BigInteger.Compare(r1.Numerator * r2.Denominator, r2.Numerator * r1.Denominator);
        
        public override string ToString()
        {
            var ret = new StringBuilder();
            ret.Append(Numerator.ToString(format: "R", CultureInfo.InvariantCulture));
            ret.Append(FractionSolidus);
            ret.Append(Denominator.ToString(format: "R", CultureInfo.InvariantCulture));
            return ret.ToString();
        }
        
        // GetWholePart() and GetFractionPart()
        // 
        // BigRational == Whole, Fraction
        //  0/2        ==     0,  0/2
        //  1/2        ==     0,  1/2
        // -1/2        ==     0, -1/2
        //  1/1        ==     1,  0/1
        // -1/1        ==    -1,  0/1
        // -3/2        ==    -1, -1/2
        //  3/2        ==     1,  1/2
        public BigInteger GetWholePart() => BigInteger.Divide(Numerator, Denominator);

        public BigRational GetFractionPart() => new BigRational(BigInteger.Remainder(Numerator, Denominator), Denominator);
        
        static bool SafeCastToDouble(BigInteger value) => DoubleMinValue <= value && value <= DoubleMaxValue;

        static bool SafeCastToDecimal(BigInteger value) => DecimalMinValue <= value && value <= DecimalMaxValue;

        static void SplitDoubleIntoParts(double dbl, out int sign, out int exp, out ulong man, out bool isFinite)
        {
            DoubleUlong du;
            du.uu = 0;
            du.dbl = dbl;

            sign = 1 - ((int) (du.uu >> 62) & 2);
            man = du.uu & 0x000FFFFFFFFFFFFF;
            exp = (int) (du.uu >> 52) & 0x7FF;
            if (exp == 0)
            {
                // Denormalized number.
                isFinite = true;
                if (man != 0)
                    exp = -1074;
            }
            else if (exp == 0x7FF)
            {
                // NaN or Infinite.
                isFinite = false;
                exp = int.MaxValue;
            }
            else
            {
                isFinite = true;
                man |= 0x0010000000000000; // mask in the implied leading 53rd significand bit
                exp -= 1075;
            }
        }

        static double GetDoubleFromParts(int sign, int exp, ulong man)
        {
            DoubleUlong du;
            du.dbl = 0;

            if (man == 0)
                du.uu = 0;
            else
            {
                // Normalize so that 0x0010 0000 0000 0000 is the highest bit set
                var cbitShift = CbitHighZero(man) - 11;
                if (cbitShift < 0)
                    man >>= -cbitShift;
                else
                    man <<= cbitShift;

                // Move the point to just behind the leading 1: 0x001.0 0000 0000 0000
                // (52 bits) and skew the exponent (by 0x3FF == 1023)
                exp += 1075;

                if (exp >= 0x7FF)
                {
                    // Infinity
                    du.uu = 0x7FF0000000000000;
                }
                else if (exp <= 0)
                {
                    // Denormalized
                    exp--;
                    if (exp < -52)
                    {
                        // Underflow to zero
                        du.uu = 0;
                    }
                    else
                        du.uu = man >> -exp;
                }
                else
                {
                    // Mask off the implicit high bit
                    du.uu = (man & 0x000FFFFFFFFFFFFF) | ((ulong) exp << 52);
                }
            }

            if (sign < 0)
                du.uu |= 0x8000000000000000;

            return du.dbl;
        }

        static int CbitHighZero(ulong uu)
        {
            if ((uu & 0xFFFFFFFF00000000) == 0)
                return 32 + CbitHighZero((uint) uu);
            return CbitHighZero((uint) (uu >> 32));
        }

        static int CbitHighZero(uint u)
        {
            if (u == 0)
                return 32;

            var cbit = 0;
            if ((u & 0xFFFF0000) == 0)
            {
                cbit += 16;
                u <<= 16;
            }

            if ((u & 0xFF000000) == 0)
            {
                cbit += 8;
                u <<= 8;
            }

            if ((u & 0xF0000000) == 0)
            {
                cbit += 4;
                u <<= 4;
            }

            if ((u & 0xC0000000) == 0)
            {
                cbit += 2;
                u <<= 2;
            }

            if ((u & 0x80000000) == 0)
                cbit += 1;
            return cbit;
        }
        
        void Simplify()
        {
            // * if the numerator is {0, +1, -1} then the fraction is already reduced
            // * if the denominator is {+1} then the fraction is already reduced
            if (Numerator == BigInteger.Zero)
                Denominator = BigInteger.One;

            var gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);
            if (gcd > BigInteger.One)
            {
                Numerator = Numerator / gcd;
                Denominator = Denominator / gcd;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DoubleUlong
        {
            [FieldOffset(0)]
            public double dbl;

            [FieldOffset(0)]
            public ulong uu;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DecimalUInt32
        {
            [FieldOffset(0)]
            public decimal dec;

            [FieldOffset(0)]
            public int flags;
        }
    }
}