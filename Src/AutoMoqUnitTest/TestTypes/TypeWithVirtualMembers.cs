namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithVirtualMembers
    {
        public virtual string VirtualProperty { get; set; }

        public virtual string VirtualMethod()
        {
            return "Awesome string";
        }
    }
}
