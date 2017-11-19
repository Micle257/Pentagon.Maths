namespace Pentagon.Maths.SignalProcessing {
    using System.Numerics;

    public class Delay
    {
        readonly Complex _complex;

        public Delay(Complex complex)
        {
            _complex = complex;
        }

        public Complex this[int delay] => Complex.Pow(_complex, delay);
    }
}