namespace TestTypeFoundation
{
    /// <summary>
    /// This type contains a method that returns an instance of the same type
    /// so that it looks like a Factory Method. In fact, it is not because it
    /// receives a parameter of the same type, so using it as a factory inevitably
    /// leads to circular dependencies.
    /// </summary>
    public class TypeWithPseudoFactoryMethod
    {
        private TypeWithPseudoFactoryMethod()
        {
        }

        public static TypeWithPseudoFactoryMethod DoSomething(
            TypeWithPseudoFactoryMethod argument)
        {
            return new TypeWithPseudoFactoryMethod();
        }
    }
}