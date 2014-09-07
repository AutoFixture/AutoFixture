using System.Collections.Generic;
namespace Ploeh.TestTypeFoundation
{
    public class TypeWithFactoryMethod
    {
        public TypeWithFactoryMethod InstanceCreate()
        {
            return new TypeWithFactoryMethod();
        }

        public static TypeWithFactoryMethod Create()
        {
            return new TypeWithFactoryMethod();
        }

        public TypeWithFactoryMethod InstanceCreate(object argument)
        {
            return new TypeWithFactoryMethod();
        }

        public static TypeWithFactoryMethod Create(object argument)
        {
            return new TypeWithFactoryMethod();
        }

        public TypeWithFactoryMethod InstanceCreate(IEnumerable<object> arguments)
        {
            return new TypeWithFactoryMethod();
        }

        public static TypeWithFactoryMethod Create(IEnumerable<object> arguments)
        {
            return new TypeWithFactoryMethod();
        }
    }
}