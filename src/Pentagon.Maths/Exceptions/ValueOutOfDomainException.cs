// -----------------------------------------------------------------------
//  <copyright file="ValueOutOfDomainException.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Maths.Exceptions
{
    using System;

    /// <summary> Represents error that occur during trying get function output value from input value, that isn't in the domain of the function. </summary>
    public class ValueOutOfDomainException : Exception
    {
        public ValueOutOfDomainException(double value) : this($"Value: {value} don't belong to function's domain.") { }

        ValueOutOfDomainException(string message) : base(message, new ArgumentOutOfRangeException()) { }
    }
}