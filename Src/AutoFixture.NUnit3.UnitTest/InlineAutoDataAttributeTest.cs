using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class InlineAutoDataAttributeTest
    {
        [Test]
        public void InlineAutoDataInheritsAutoData()
        {
            Assert.That(new InlineAutoDataAttribute(), 
                Is.AssignableTo(typeof(AutoDataAttribute)));
        }

        [Test]
        public void InlineAutoDataCanBeExtendedWithImplementationOfIFixture()
        {
            var extended = new InlineAutoDataAttributeStub();

            Assert.That(extended, Is.AssignableTo(typeof(InlineAutoDataAttribute)));
        }

        private class InlineAutoDataAttributeStub : InlineAutoDataAttribute
        {
            /// <summary>
            /// Here we can use any implementation of <see cref="IFixture"/>, 
            /// the use of <see cref="ThrowingStubFixture"/> is pure coincidence and inconsequential
            /// </summary>
            /// <param name="arguments"></param>
            public InlineAutoDataAttributeStub(params object[] arguments) 
                : base(new ThrowingStubFixture(), arguments)
            {
            }
        }
    }
}
