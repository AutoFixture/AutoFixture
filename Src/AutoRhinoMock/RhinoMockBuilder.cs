using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public class RhinoMockBuilder : ISpecimenBuilder
    {
        private readonly Func<Type, bool> shouldBeMocked;

        public RhinoMockBuilder()
            : this(RhinoMockBuilder.ShouldBeMocked)
        {
        }

        public RhinoMockBuilder(Func<Type, bool> mockableSpecification)
        {
            if (mockableSpecification == null)
            {
                throw new ArgumentNullException("mockableSpecification");
            }

            this.shouldBeMocked = mockableSpecification;
        }

        public Func<Type, bool> MockableSpecification
        {
            get { return this.shouldBeMocked; }
        }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var t = request as Type;
            if (!this.shouldBeMocked(t))
            {
                return new NoSpecimen(request);
            }
            if (t == null)
            {
                return new NoSpecimen(request);
            }

            if (t.IsInterface)
            {
                return MockRepository.GenerateMock(t, Enumerable.Empty<Type>().ToArray());
            }

            var resolvedParameters = new List<object>();

            var ctor = t.GetPublicAndProtectedConstructors().First();
            var parameters = ctor.GetParameters().ToList();
            parameters.ForEach(p => resolvedParameters.Add(context.Resolve(p.ParameterType)));

            return MockRepository.GenerateMock(t, Enumerable.Empty<Type>().ToArray(), resolvedParameters.ToArray());
        }

        #endregion

        private static bool ShouldBeMocked(Type t)
        {
            return t.IsMockable();
        }
    }
}
