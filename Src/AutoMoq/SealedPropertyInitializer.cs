using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// If the type of the object being mocked contains any non-virtual/sealed settable properties,
    /// this initializer will resolve them from a given context.
    /// </summary>
    public class SealedPropertyInitializer : IMockInitializer
    {
        /// <summary>
        /// If the type of the object being mocked contains any non-virtual/sealed settable properties,
        /// this initializer will resolve them from a given context.
        /// </summary>
        /// <param name="mock">The mock object.</param>
        /// <param name="context">The context.</param>
        [CLSCompliant(false)]
        public void Setup(Mock mock, ISpecimenContext context)
        {
            if (mock == null) throw new ArgumentNullException("mock");
            if (context == null) throw new ArgumentNullException("context");

            var mockType = mock.GetType();
            var mockedType = mockType.GetMockedType();
            var mockedObject = mock.Object;

            var properties = mockedType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                       .Where(p => p.CanWrite && //check if property has set accessor
                                                   p.GetSetMethod() != null && //check if the set accessor is public
                                                   p.GetSetMethod().IsSealed());

            foreach (var property in properties)
            {
                var value = context.Resolve(property.PropertyType);
                property.SetValue(mockedObject, value, null);
            }
        }
    }
}
