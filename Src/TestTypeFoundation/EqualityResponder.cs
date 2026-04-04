namespace TestTypeFoundation;

public class EqualityResponder
{
    private readonly bool _equals;

    public EqualityResponder(bool equals)
    {
        _equals = equals;
    }

    public override bool Equals(object obj)
    {
        return _equals;
    }

    public override int GetHashCode()
    {
        return _equals.GetHashCode();
    }
}