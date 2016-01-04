namespace Ploeh.TestTypeFoundation
{
    public class TypeWithFactoryProperty
    {
        private TypeWithFactoryProperty()
        {
        }

        public static TypeWithFactoryProperty Factory => new TypeWithFactoryProperty();
    }
}
