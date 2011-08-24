namespace Ploeh.TestTypeFoundation
{
    public class TypeWithFactoryMethod
    {
        private TypeWithFactoryMethod() 
        {
        }

        public static TypeWithFactoryMethod Create()
        {
            return new TypeWithFactoryMethod();
        }

        public static TypeWithFactoryMethod Create(object argument)
        {
            return new TypeWithFactoryMethod();
        }
    }
}