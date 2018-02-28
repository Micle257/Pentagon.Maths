namespace Pentagon.Maths.Functions {
    using System;

    public class EquationNotComputedException : Exception
    {
        public EquationNotComputedException() : base($"The equation is not computed.") { }
    }
}