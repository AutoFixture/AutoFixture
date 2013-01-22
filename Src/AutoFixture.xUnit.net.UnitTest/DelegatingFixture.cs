using System;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    internal class DelegatingFixture : IFixture
    {
        private readonly List<ISpecimenBuilder> customizations;
        private readonly List<ISpecimenBuilder> residueCollectors;

        public DelegatingFixture()
        {
            this.customizations = new List<ISpecimenBuilder>();
            this.residueCollectors = new List<ISpecimenBuilder>();
            this.OnCreate = (r, s) => new object();
        }

        public IList<ISpecimenBuilderTransformation> Behaviors
        {
            get { throw new NotImplementedException(); }
        }

        public IList<ISpecimenBuilder> Customizations
        {
            get { return this.customizations; }
        }

        public bool OmitAutoProperties { get; set; }

        public int RepeatCount { get; set; }

        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return this.residueCollectors; }
        }

        public void AddManyTo<T>(ICollection<T> collection, Func<T> creator)
        {
            throw new NotImplementedException();
        }

        public void AddManyTo<T>(ICollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public void AddManyTo<T>(ICollection<T> collection, int repeatCount)
        {
            throw new NotImplementedException();
        }

        public Ploeh.AutoFixture.Dsl.ICustomizationComposer<T> Build<T>()
        {
            throw new NotImplementedException();
        }

        public IFixture Customize(ICustomization customization)
        {
            throw new NotImplementedException();
        }

        public void Customize<T>(Func<Ploeh.AutoFixture.Dsl.ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            throw new NotImplementedException();
        }

        public void Inject<T>(T item)
        {
            throw new NotImplementedException();
        }

        public void Register<T>(Func<T> creator)
        {
            throw new NotImplementedException();
        }

        public void Register<TInput, T>(Func<TInput, T> creator)
        {
            throw new NotImplementedException();
        }

        public void Register<TInput1, TInput2, T>(Func<TInput1, TInput2, T> creator)
        {
            throw new NotImplementedException();
        }

        public void Register<TInput1, TInput2, TInput3, T>(Func<TInput1, TInput2, TInput3, T> creator)
        {
            throw new NotImplementedException();
        }

        public void Register<TInput1, TInput2, TInput3, TInput4, T>(Func<TInput1, TInput2, TInput3, TInput4, T> creator)
        {
            throw new NotImplementedException();
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
    }
}
