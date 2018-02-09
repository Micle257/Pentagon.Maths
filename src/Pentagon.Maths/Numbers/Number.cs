// -----------------------------------------------------------------------
//  <copyright file="Number.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Numbers
{
    public abstract class Number : INumber
    {
        public abstract NumberSet NumberSet { get; }

        #region Operators

        public static INumber operator +(Number left, INumber right) => left.Add(right);
        public static INumber operator *(Number left, INumber right) => left.Multiple(right);

        #endregion

        public INumber Add(INumber second) => NumberCalculateHelper.Add(this, second);
        public INumber Multiple(INumber second) => NumberCalculateHelper.Multiple(this, second);
    }
}