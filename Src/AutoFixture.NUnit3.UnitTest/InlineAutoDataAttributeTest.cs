using NUnit.Framework;
using System;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

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
            var extended = new DeriviedInlineAutoDataAttribute();

            Assert.That(extended, Is.AssignableTo(typeof(AutoDataAttribute)));
        }

        private class DeriviedInlineAutoDataAttribute : InlineAutoDataAttribute
        {
            public DeriviedInlineAutoDataAttribute(params object[] arguments) 
                : base(new StubFixture(), arguments)
            {
            }
        }

        private class StubFixture : IFixture
        {
            public IList<ISpecimenBuilderTransformation> Behaviors
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IList<ISpecimenBuilder> Customizations
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool OmitAutoProperties
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public int RepeatCount
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public IList<ISpecimenBuilder> ResidueCollectors
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public ICustomizationComposer<T> Build<T>()
            {
                throw new NotImplementedException();
            }

            public object Create(object request, ISpecimenContext context)
            {
                throw new NotImplementedException();
            }

            public IFixture Customize(ICustomization customization)
            {
                throw new NotImplementedException();
            }

            public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
            {
                throw new NotImplementedException();
            }
        }
    }
}
