namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public class ConcreteTypeWithVirtualMembers : TypeWithVirtualMembers
    {
    }

    public abstract class TypeWithVirtualMembers
    {
        public virtual string VirtualProperty { get; set; }

        public virtual string VirtualMethod()
        {
            return "Awesome string";
        }
    }
}
