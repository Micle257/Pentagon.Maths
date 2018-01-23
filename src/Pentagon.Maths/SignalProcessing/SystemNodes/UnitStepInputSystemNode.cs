namespace Pentagon.Maths.SignalProcessing.SystemNodes {
    public class UnitStepInputSystemNode : IInputSystemNode
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public double GetValue(int index) => index >= 0 ? 1 : 0;
    }
}