namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    public class StepImpulseInputSystemNode : IInputSystemNode
    {
        /// <inheritdoc />
        public double GetValue(int index) => index == 0 ? 1 : 0;
    }
}