// -----------------------------------------------------------------------
//  <copyright file="BinaryNumber.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Represents a binary number.
    /// </summary>
    public struct BinaryNumber : IValueDataType<BinaryNumber>
    {
        /// <summary>
        ///     The number.
        /// </summary>
        readonly double _number;

        /// <summary>
        ///     The bits.
        /// </summary>
        IReadOnlyList<Bit> _bits;

        /// <summary>
        ///     The bytes.
        /// </summary>
        IReadOnlyList<byte> _bytes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryNumber" /> struct.
        /// </summary>
        /// <param name="number"> The number. </param>
        public BinaryNumber(double number)
        {
            _number = number;
            HasValue = true;
            _bits = null;
            _bytes = null;
        }

        /// <summary>
        ///     Gets the bits of this binary number.
        /// </summary>
        /// <value>
        ///     The read-only list of <see cref="Bit" />.
        /// </value>
        public IReadOnlyList<Bit> Bits => _bits ?? (_bits = GetBits());

        /// <summary>
        ///     Gets the bytes of this binary number.
        /// </summary>
        /// <value>
        ///     The read-only list of <see cref="byte" />.
        /// </value>
        public IReadOnlyList<byte> Bytes => _bytes ?? (_bytes = GetBytes());

        /// <inheritdoc />
        public bool HasValue { get; }

        #region Operators

        /// <inheritdoc />
        public static bool operator <(BinaryNumber left, BinaryNumber right) => left.CompareTo(right) < 0;

        /// <inheritdoc />
        public static bool operator >(BinaryNumber left, BinaryNumber right) => left.CompareTo(right) > 0;

        /// <inheritdoc />
        public static bool operator <=(BinaryNumber left, BinaryNumber right) => left.CompareTo(right) <= 0;

        /// <inheritdoc />
        public static bool operator >=(BinaryNumber left, BinaryNumber right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public static bool operator ==(BinaryNumber left, BinaryNumber right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(BinaryNumber left, BinaryNumber right) => !left.Equals(right);

        #endregion

        /// <inheritdoc />
        public int CompareTo(BinaryNumber other) => _number.CompareTo(other._number);

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is BinaryNumber))
                throw new ArgumentException($"Object must be of type {nameof(BinaryNumber)}");
            return CompareTo((BinaryNumber) obj);
        }

        /// <inheritdoc />
        public bool Equals(BinaryNumber other) => _number.Equals(other._number);

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is BinaryNumber && Equals((BinaryNumber) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _number.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => Bits.Select(b => b.ToString()).Aggregate((s, s1) => $"{s}{s1}");

        /// <summary>
        ///     Gets the bits.
        /// </summary>
        /// <returns>
        ///     A read-only list of <see cref="Bit" />.
        /// </returns>
        IReadOnlyList<Bit> GetBits()
        {
            var bin = new List<Bit>();
            var number = _number;

            while (number >= 1)
            {
                var a = (int) (number % 2);
                number = number / 2;
                bin.Add(Convert.ToBoolean(a));
            }

            bin.Reverse();
            if (bin.Count == 0)
                bin.Add(Bit.Zero);
            return bin;
        }

        /// <summary>
        ///     Gets the bytes.
        /// </summary>
        /// <returns>
        ///     A read-only list of <see cref="byte" />.
        /// </returns>
        IReadOnlyList<byte> GetBytes()
        {
            var bytes = new List<byte>();
            var bits = Bits.ToList();
            var bytesCount = bits.Count / 8;
            for (var i = 0; i < bytesCount - 1; i++)
            {
                List<Bit> one;
                if (bits.Count < 8)
                    one = bits;
                else
                {
                    one = bits.GetRange(i, 8);
                    bits.RemoveRange(i, 8);
                }
                byte b = 0;
                foreach (var item in one)
                    b += Convert.ToByte(item.Value);
                bytes.Add(b);
            }

            return bytes;
        }
    }
}