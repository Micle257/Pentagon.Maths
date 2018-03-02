// -----------------------------------------------------------------------
//  <copyright file="ComposeUnit.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Units
{
    using System.Collections.Generic;
    using System.Linq;

    public class ComposeUnit : IPhysicalUnit
    {
        public ComposeUnit(IEnumerable<IPhysicalUnit> numerator, IEnumerable<IPhysicalUnit> denumerator)
        {
            Numerator = numerator as IList<IPhysicalUnit> ?? numerator.ToList();
            Denumerator = denumerator as IList<IPhysicalUnit> ?? denumerator.ToList();
        }

        public IList<IPhysicalUnit> Numerator { get; }

        public IList<IPhysicalUnit> Denumerator { get; }

        /// <inheritdoc />
        public string Symbol { get; }

        #region IEquatable members

        /// <inheritdoc />
        public bool Equals(IPhysicalUnit other)
        {
            if (!(other is ComposeUnit cu))
                return false;

            if (Numerator.Count != cu.Numerator.Count || Denumerator.Count != cu.Denumerator.Count)
                return false;

            for (var i = 0; i < Numerator.Count; i++)
            {
                if (!Numerator[i].IsEqual(cu.Numerator[i]))
                    return false;
            }

            for (var i = 0; i < Denumerator.Count; i++)
            {
                if (!Denumerator[i].IsEqual(cu.Denumerator[i]))
                    return false;
            }

            return true;
        }

        #endregion
    }
}