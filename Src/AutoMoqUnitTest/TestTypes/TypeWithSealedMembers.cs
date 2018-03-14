namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public abstract class TypeWithSealedMembersTemp
    {
        public abstract string ExplicitlySealedProperty { get; set; }
        public abstract string ExplicitlySealedMethod();
    }

    public class TypeWithSealedMembers : TypeWithSealedMembersTemp
    {
        public sealed override string ExplicitlySealedProperty { get; set; }
        public string ImplicitlySealedProperty { get; set; }

        public sealed override string ExplicitlySealedMethod()
        {
            return "Awesome string";
        }

        public string ImplicitlySealedMethod()
        {
            return "Awesome string";
        }
    }
}
