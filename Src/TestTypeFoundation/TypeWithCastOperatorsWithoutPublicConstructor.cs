namespace TestTypeFoundation
{
    public class TypeWithCastOperatorsWithoutPublicConstructor
    {
        private TypeWithCastOperatorsWithoutPublicConstructor()
        {
        }

        public static implicit operator TypeWithCastOperatorsWithoutPublicConstructor(int ignored)
        {
            return new TypeWithCastOperatorsWithoutPublicConstructor();
        }

        public static explicit operator TypeWithCastOperatorsWithoutPublicConstructor(string ignored)
        {
            return new TypeWithCastOperatorsWithoutPublicConstructor();
        }
    }
}