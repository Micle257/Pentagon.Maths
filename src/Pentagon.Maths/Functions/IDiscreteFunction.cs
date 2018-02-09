namespace Pentagon.Maths.Functions {
    using System.Collections.Generic;
    using Helpers;

    public interface IDiscreteFunction
    {
        double SamplingFrequency { get; }

        double this[int sample] { get; }

        double EvaluateSample(int sample);

        double EvaluateTime(double time);
        IEnumerable<double> EvaluateSamples(IRange<int> range);
    }
}