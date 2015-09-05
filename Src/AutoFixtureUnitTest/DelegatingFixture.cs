using System;
using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class DelegatingFixture : IFixture
    {
        public Func<ICustomization, IFixture> OnCustomize { get; set; }
        public Func<object, ISpecimenContext, object> OnCreate { get; set; }

        public DelegatingFixture()
        {
            this.OnCustomize = customization => this;
            this.OnCreate = (request, context) => new object();
        }

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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int RepeatCount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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
            return this.OnCustomize(customization);
        }

        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            throw new NotImplementedException();
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }
    }
}
