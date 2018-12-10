using System;
using System.Linq.Expressions;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Dsl
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
            this.OnFromOverloadFactory = f => new DelegatingComposer<T>();
            this.OnDo = f => new DelegatingComposer<T>();
            this.OnOmitAutoProperties = () => new DelegatingComposer<T>();
            this.OnAnonymousWith = f => new DelegatingComposer<T>();
            this.OnWithOverloadValue = (f, v) => new DelegatingComposer<T>();
            this.OnWithOverloadFactory = (f, vf) => new DelegatingComposer<T>();
            this.OnWithAutoProperties = () => new DelegatingComposer<T>();
            this.OnWithout = f => new DelegatingComposer<T>();
            this.OnCreate = (r, c) => new object();
        }

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory) => this.OnFromSeed(factory);

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory) => this.OnFromBuilder(factory);

        public IPostprocessComposer<T> FromFactory(Func<T> factory) => this.OnFromFactory(factory);

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory) =>
            this.OnFromOverloadFactory(factory);

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory) =>
            this.OnFromOverloadFactory(factory);

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory) =>
            this.OnFromOverloadFactory(factory);

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory) =>
            this.OnFromOverloadFactory(factory);

        public IPostprocessComposer<T> Do(Action<T> action) => this.OnDo(action);

        public IPostprocessComposer<T> OmitAutoProperties() => this.OnOmitAutoProperties();

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker) =>
            this.OnAnonymousWith(propertyPicker);

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value) =>
            this.OnWithOverloadValue(propertyPicker, value);

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory) =>
            this.OnWithOverloadFactory(propertyPicker, valueFactory);

        public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory) =>
            this.OnWithOverloadFactory(propertyPicker, valueFactory);

        public IPostprocessComposer<T> WithAutoProperties() => this.OnWithAutoProperties();

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker) =>
            this.OnWithout(propertyPicker);

        public object Create(object request, ISpecimenContext context) => this.OnCreate(request, context);

        internal Func<Func<T, T>, IPostprocessComposer<T>> OnFromSeed { get; set; }
        internal Func<ISpecimenBuilder, IPostprocessComposer<T>> OnFromBuilder { get; set; }
        internal Func<Func<T>, IPostprocessComposer<T>> OnFromFactory { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnFromOverloadFactory { get; set; }
        internal Func<Action<T>, IPostprocessComposer<T>> OnDo { get; set; }
        internal Func<IPostprocessComposer<T>> OnOmitAutoProperties { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnAnonymousWith { get; set; }
        internal Func<object, object, IPostprocessComposer<T>> OnWithOverloadValue { get; set; }
        internal Func<object, object, IPostprocessComposer<T>> OnWithOverloadFactory { get; set; }
        internal Func<IPostprocessComposer<T>> OnWithAutoProperties { get; set; }
        internal Func<object, IPostprocessComposer<T>> OnWithout { get; set; }
        internal Func<object, ISpecimenContext, object> OnCreate { get; set; }
    }
}
