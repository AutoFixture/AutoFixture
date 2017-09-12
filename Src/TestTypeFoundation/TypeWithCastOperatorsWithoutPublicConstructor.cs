namespace Ploeh.TestTypeFoundation
{
    public class TypeWithCastOperatorsWithoutPublicConstructor
    {
        private TypeWithCastOperatorsWithoutPublicConstructor()
        {
        }

        public static implicit operator TypeWithCastOperatorsWithoutPublicConstructor(int _)
        {
            return new TypeWithCastOperatorsWithoutPublicConstructor();
        }

        public static explicit operator TypeWithCastOperatorsWithoutPublicConstructor(string _)
        {
            return new TypeWithCastOperatorsWithoutPublicConstructor();
        }
    }
}