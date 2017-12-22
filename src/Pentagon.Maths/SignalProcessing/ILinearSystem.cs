namespace Pentagon.Maths.SignalProcessing {
    using Functions;
    using Quantities;

    public interface ILinearSystem
    {
        IDiscreteFunction GetImpulseResponse(Frequency samplingFrequency);
    }
}