namespace Pentagon.Maths.Numbers {
    public interface INumber
    {
        INumber Add(INumber second);

        INumber Multiple(INumber second);
    }
}