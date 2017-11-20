namespace Pentagon.Maths.SignalProcessing {
    public abstract class Fraction<TValue>
    {
        public TValue Numerator { get; }
        public TValue Denumerator { get; }

        public Fraction(TValue numerator, TValue denumerator)
        {
            Numerator = numerator;
            Denumerator = denumerator;
        }
        public static Fraction<TValue> operator +(Fraction<TValue> first, Fraction<TValue> second) => first.Add(second);

        public abstract Fraction<TValue> Add(Fraction<TValue> second);

        public static Fraction<TValue> operator *(Fraction<TValue> first, Fraction<TValue> second) => first.Multiple(second);

        public abstract Fraction<TValue> Multiple(Fraction<TValue> second);

        public abstract Fraction<TValue> InTermsOf(Fraction<TValue> second);
    }
}