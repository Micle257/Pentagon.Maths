namespace Pentagon.Maths.Numbers {
    using System;

    public interface INumber
    {
        NumberSet NumberSet { get; }

        INumber Add(INumber second);

        INumber Multiple(INumber second);
    }
}