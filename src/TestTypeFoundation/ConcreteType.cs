namespace TestTypeFoundation;

public class ConcreteType : AbstractType
{
    public ConcreteType()
    {
    }

    public ConcreteType(object obj)
    {
        Property1 = obj;
    }

    public ConcreteType(object obj1, object obj2)
    {
        Property1 = obj1;
        Property2 = obj2;
    }

    public ConcreteType(object obj1, object obj2, object obj3)
    {
        Property1 = obj1;
        Property2 = obj2;
        Property3 = obj3;
    }

    public ConcreteType(object obj1, object obj2, object obj3, object obj4)
    {
        Property1 = obj1;
        Property2 = obj2;
        Property3 = obj3;
        Property4 = obj4;
    }

    public override object Property4 { get; set; }

    public object Property5 { get; set; }
}