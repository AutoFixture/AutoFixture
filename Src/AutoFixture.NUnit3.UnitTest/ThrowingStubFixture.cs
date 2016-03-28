using System;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    /// <summary>
    /// A <see cref="IFixture"/> for the benefit of creating stubs of <see cref="AutoDataAttribute"/> for unit testing
    /// </summary>
    public class ThrowingStubFixture : IFixture
    {
        public System.Collections.Generic.IList<Kernel.ISpecimenBuilderTransformation> Behaviors
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Generic.IList<Kernel.ISpecimenBuilder> Customizations
        {
            get { throw new NotImplementedException(); }
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

        public System.Collections.Generic.IList<Kernel.ISpecimenBuilder> ResidueCollectors
        {
            get { throw new NotImplementedException(); }
        }

        public Dsl.ICustomizationComposer<T> Build<T>()
        {
            throw new NotImplementedException();
        }

        public IFixture Customize(ICustomization customization)
        {
            throw new NotImplementedException();
        }

        public void Customize<T>(Func<Dsl.ICustomizationComposer<T>, Kernel.ISpecimenBuilder> composerTransformation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This throws a unique <see cref="DummyException"/> for testing purpose
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Create(object request, Kernel.ISpecimenContext context)
        {
            throw new DummyException();
        }

        /// <summary>
        /// A unique exception for the benefit of unit testing
        /// </summary>
        public class DummyException : Exception
        {
        }
    }
}
