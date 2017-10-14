// -----------------------------------------------------------------------
//  <copyright file="ZTranform.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    using System;
    using System.Numerics;

    public class ZTranform
    {
        public ZTranform(double[] signal)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));
            if (signal.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(signal));
            Parameters = new Vector(signal);
        }

        public ZTranform(Vector signla)
        {
            if (signla == null)
                throw new ArgumentNullException(nameof(signla));
            Parameters = signla;
        }

        public static ZTranform FirstOrder => FromOrder(1);

        public Vector Parameters { get; }

        #region Operators

        public static ZTranform operator +(ZTranform a, ZTranform b) => new ZTranform(a.Parameters + b.Parameters);

        public static ZTranform operator *(ZTranform a, ZTranform b) => new ZTranform(a.Parameters.Convolution(b.Parameters));

        #endregion

        public static ZTranform FromOrder(int order)
        {
            var coif = new double[order];
            coif[order - 1] = 1;
            return new ZTranform(coif);
        }

        //public IMathExpression GetExpression() // TODO MathExprs with Complex
        //{
        //    var first = (MathExpression) Signal[0];
        //    var muls = new List<MathExpression>();
        //    for (var i = 1; i < Signal.Length; i++)
        //    {
        //        muls.Add(Signal[i] * Complex.Pow());
        //    }
        //}

        public Complex Evaluate(Complex z)
        {
            var sum = default(Complex);
            for (var n = 0; n < Parameters.Length; n++)
                sum += Parameters[n] * Complex.Pow(z, -n);
            return sum;
        }
    }
}