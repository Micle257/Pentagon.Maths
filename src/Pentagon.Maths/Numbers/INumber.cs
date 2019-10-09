// -----------------------------------------------------------------------
//  <copyright file="INumber.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Numbers
{
    public interface INumber
    {
        NumberSet NumberSet { get; }

        INumber Add(INumber second);

        INumber Multiple(INumber second);
    }
}