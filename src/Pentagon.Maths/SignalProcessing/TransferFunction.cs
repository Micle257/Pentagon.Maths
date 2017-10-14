// -----------------------------------------------------------------------
//  <copyright file="TransferFunction.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.SignalProcessing
{
    public class TransferFunction
    {
        public TransferFunction(ZTranform output, ZTranform input)
        {
            Input = input;
            Output = output;
        }

        public TransferFunction(double[] output, double[] input) : this(new ZTranform(output), new ZTranform(input)) { }

        public ZTranform Input { get; }
        public ZTranform Output { get; }

        #region Operators

        public static TransferFunction operator +(TransferFunction a, TransferFunction b) => new TransferFunction(a.Output + b.Output, a.Input + b.Input);
        public static TransferFunction operator *(TransferFunction a, TransferFunction b) => new TransferFunction(a.Output * b.Output, a.Input * b.Input);

        public static TransferFunction operator *(TransferFunction a, double value)
        {
            var s = a.Output.Parameters;
            for (var index = 0; index < s.Length; index++)
                s[index] *= value;
            return new TransferFunction(new ZTranform(s), a.Input);
        }

        public static TransferFunction operator ^(TransferFunction a, int exp)
        {
            if (exp > 1)
            {
                for (var i = 1; i < exp; i++)
                    a *= a;
                return a;
            }
            return a;
            //return new TransferFunction((MathExpression)a.Numerator ^ exp, (MathExpression)a.Denominator ^ exp);
        }

        #endregion

        //public static double[] Convolution(double[] u, double[] v)
        //{
        //    var m = u.Length;
        //    var n = v.Length;
        //    var w = new double[m + n - 1];
        //    for (var k = 0; k < w.Length; k++)
        //    {
        //        for (var j = new[] {0, k - n + 1}.Max(); j < new[] {k + 1, m}.Min(); j++) // var j = max(1,k+1-n); j < min(k,m); j++
        //            w[k] += u[j] * v[k - j];
        //    }
        //    return w;
        //}

        public TransferFunction AddOne()
        {
            var input = Input.Parameters;
            var output = Output.Parameters;
            var newOutput = input + output;
            return new TransferFunction(new ZTranform(newOutput), Input);
        }

        public TransferFunction Invert() => new TransferFunction(Input, Output);
    }
}