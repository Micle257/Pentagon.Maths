namespace Pentagon.Maths.Functions {
    using Quantities;

    public interface IDiscreteFunction
    {
        Frequency SamplingFrequency { get; }

        double this[int sample] { get; }

        double EvaluateSample(int sample);

        double EvaluateTime(double time);
    }
}