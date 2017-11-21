namespace Pentagon.Maths.Numbers {
    public class WholeNumber : Number
    {
        public int Value { get; }

        public WholeNumber(int value)
        {
            Value = value;
        }
        public override NumberSet NumberSet => NumberSet.Whole;
    }
}