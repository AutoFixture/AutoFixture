using System;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    public class DelegatingComposer : DelegatingComposer<object>
    {
    }

    public class DelegatingComposer<T> : ICustomizationComposer<T>
    {
        public DelegatingComposer()
        {
            this.OnFromSeed = f => new DelegatingComposer<T>();
            this.OnFromBuilder = f => new DelegatingComposer<T>();
            this.OnFromFactory = f => new DelegatingComposer<T>();
            this.OnFromOverloadeFactory = f => new DelegatingComposer<T>();
            this.OnDo = f => new DelegatingComposer<T>();
            this.OnOmitAutoProperties = () => new DelegatingComposer<T>();
            this.OnAnonymousWith = f => new DelegatingComposer<T>();
            this.OnWith = (f, v) => new DelegatingComposer<T>();
            this.OnWithAutoProperties = () => new DelegatingComposer<T>();
            this.OnWithout = f => new DelegatingComposer<T>();
            this.OnCreate = (r, c) => new object();
        }

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this.OnFromSeed(factory);
        }

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return this.OnFromBuilder(factory);
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this.OnFromFactory(factory);
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this.OnFromOverloadeFactory(factory);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this.OnFromOverloadeFactory(factory);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this.OnFromOverloadeFactory(factory);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this.OnFromOverloadeFactory(factory);
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return this.OnDo(action);
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return this.OnOmitAutoProperties();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this.OnAnonymousWith(propertyPicker);
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return this.OnWith(propertyPicker, value);
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            return this.OnWithAutoProperties();
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return this.OnWithout(propertyPicker);
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.OnCreate(request, context);
        }

        internal Func<Func<T, T>, IPostprocessComposer<T>> OnFromSeed { get; set; }
        internal Func<ISpecimenBuilder, IPostprocessComposer<T>> OnFromBuilder { get; set; }
        internal Func<Func<T>, IPostprocessComposer<T>> OnFromFactory { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnFromOverloadeFactory { get; set; }
        internal Func<Action<T>, IPostprocessComposer<T>> OnDo { get; set; }
        internal Func<IPostprocessComposer<T>> OnOmitAutoProperties { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnAnonymousWith { get; set; }
        internal Func<object, object, IPostprocessComposer<T>> OnWith { get; set; }
        internal Func<IPostprocessComposer<T>> OnWithAutoProperties { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnWithout { get; set; }
        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
        internal Func<IMatchComposer<T>> OnMatch { get; set; }
    }
}
