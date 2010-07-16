using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    public class MockPostprocessor : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        public MockPostprocessor(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        #region ISpecimenBuilder Members

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (!t.IsMock())
            {
                return new NoSpecimen(request);
            }            

            var m = this.builder.Create(request, context) as Mock;
            if (m == null)
            {
                return new NoSpecimen(request);
            }

            var mockType = t.GetMockedType();
            if (m.GetType().GetMockedType() != mockType)
            {
                return new NoSpecimen(request);
            }

            var configurator = (IMockConfigurator)Activator.CreateInstance(typeof(MockConfigurator<>).MakeGenericType(mockType));
            configurator.Configure(m);

            return m;
        }

        #endregion

        private class MockConfigurator<T> : IMockConfigurator where T : class
        {
            #region IMockConfigurator Members

            public void Configure(Mock mock)
            {
                this.Configure((Mock<T>)mock);
            }

            #endregion

            private void Configure(Mock<T> mock)
            {
                mock.CallBase = true;
                mock.DefaultValue = DefaultValue.Mock;
            }
        }

        private interface IMockConfigurator
        {
            void Configure(Mock mock);
        }
    }
}
