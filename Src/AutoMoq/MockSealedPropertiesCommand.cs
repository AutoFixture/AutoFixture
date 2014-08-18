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
    public class MockSealedPropertiesCommand : ISpecimenCommand
    {
        private readonly ISpecimenCommand autoPropertiesCommand =
            new AutoPropertiesCommand(new SealedPropertySpecification());

        /// <summary>
        /// If the type of the object being mocked contains any non-virtual/sealed settable properties,
        /// this initializer will resolve them from a given context.
        /// </summary>
        /// <param name="specimen">The mock object.</param>
        /// <param name="context">The context.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            var mock = specimen as Mock;
            if (mock == null)
                return;

            autoPropertiesCommand.Execute(mock.Object, context);
        }

        private class SealedPropertySpecification : IRequestSpecification
        {
            /// <summary>
            /// Satisfied by any non-virtual/sealed properties.
            /// </summary>
            public bool IsSatisfiedBy(object request)
            {
                var pi = request as PropertyInfo;
                if (pi != null)
                    return pi.GetSetMethod().IsSealed();

                return false;
            }
        }
    }
}
