using System;
using System.Collections.Generic;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture.NUnit3.UnitTest
{
    /// <summary>
    /// A <see cref="IFixture"/> for the benefit of creating stubs of <see cref="AutoDataAttribute"/> 
    /// and <see cref="InlineAutoDataAttribute"/> for unit testing
    /// </summary>
    public class ThrowingStubFixture : IFixture
    {
        public IList<ISpecimenBuilderTransformation> Behaviors
        {
            get { throw new NotImplementedException(); }
        }

        public IList<ISpecimenBuilder> Customizations
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

        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { throw new NotImplementedException(); }
        }

        public ICustomizationComposer<T> Build<T>()
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

        /// <summary>
        /// This throws a unique <see cref="DummyException"/> for testing purpose
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Create(object request, ISpecimenContext context)
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
