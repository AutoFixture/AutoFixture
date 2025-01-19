using System;
using System.Collections.Generic;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class DelegatingFixture : IFixture
    {
        private readonly List<ISpecimenBuilder> customizations = new();
        private readonly List<ISpecimenBuilder> residueCollectors = new();

        public IList<ISpecimenBuilderTransformation> Behaviors => throw new InvalidOperationException();

        public IList<ISpecimenBuilder> Customizations => this.customizations;

        public bool OmitAutoProperties { get; set; }

        public int RepeatCount { get; set; }

        public IList<ISpecimenBuilder> ResidueCollectors => this.residueCollectors;

        public void AddManyTo<T>(ICollection<T> collection, Func<T> creator)
        {
            throw new InvalidOperationException();
        }

        public void AddManyTo<T>(ICollection<T> collection)
        {
            throw new InvalidOperationException();
        }

        public void AddManyTo<T>(ICollection<T> collection, int repeatCount)
        {
            throw new InvalidOperationException();
        }

        public ICustomizationComposer<T> Build<T>()
        {
            throw new InvalidOperationException();
        }

        public IFixture Customize(ICustomization customization)
        {
            this.OnCustomize?.Invoke(customization);
            return this;
        }

        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            throw new InvalidOperationException();
        }

        public void Inject<T>(T item)
        {
            throw new InvalidOperationException();
        }

        public void Register<T>(Func<T> creator)
        {
            throw new InvalidOperationException();
        }

        public void Register<TInput, T>(Func<TInput, T> creator)
        {
            throw new InvalidOperationException();
        }

        public void Register<TInput1, TInput2, T>(Func<TInput1, TInput2, T> creator)
        {
            throw new InvalidOperationException();
        }

        public void Register<TInput1, TInput2, TInput3, T>(Func<TInput1, TInput2, TInput3, T> creator)
        {
            throw new InvalidOperationException();
        }

        public void Register<TInput1, TInput2, TInput3, TInput4, T>(Func<TInput1, TInput2, TInput3, TInput4, T> creator)
        {
            throw new InvalidOperationException();
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        internal Func<object, ISpecimenContext, object> OnCreate { get; set; } = (_, _) => new object();

        internal Action<ICustomization> OnCustomize { get; set; }
    }
}
