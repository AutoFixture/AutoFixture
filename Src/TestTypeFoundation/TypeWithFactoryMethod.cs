using System.Collections.Generic;
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

        public static TypeWithFactoryMethod Create(IEnumerable<object> arguments)
        {
            return new TypeWithFactoryMethod();
        }
    }
}