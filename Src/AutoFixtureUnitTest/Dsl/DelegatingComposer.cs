using System;
using System.Linq.Expressions;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Dsl;

public class DelegatingComposer : DelegatingComposer<object>
{
}

public class DelegatingComposer<T> : ICustomizationComposer<T>
{
    public DelegatingComposer()
    {
        OnFromSeed = f => new DelegatingComposer<T>();
        OnFromBuilder = f => new DelegatingComposer<T>();
        OnFromFactory = f => new DelegatingComposer<T>();
        OnFromOverloadFactory = f => new DelegatingComposer<T>();
        OnDo = f => new DelegatingComposer<T>();
        OnOmitAutoProperties = () => new DelegatingComposer<T>();
        OnAnonymousWith = f => new DelegatingComposer<T>();
        OnWithOverloadValue = (f, v) => new DelegatingComposer<T>();
        OnWithOverloadFactory = (f, vf) => new DelegatingComposer<T>();
        OnWithAutoProperties = () => new DelegatingComposer<T>();
        OnWithout = f => new DelegatingComposer<T>();
        OnCreate = (r, c) => new object();
    }

    public IPostprocessComposer<T> FromSeed(Func<T, T> factory) => OnFromSeed(factory);

    public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory) => OnFromBuilder(factory);

    public IPostprocessComposer<T> FromFactory(Func<T> factory) => OnFromFactory(factory);

    public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory) =>
        OnFromOverloadFactory(factory);

    public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory) =>
        OnFromOverloadFactory(factory);

    public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory) =>
        OnFromOverloadFactory(factory);

    public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory) =>
        OnFromOverloadFactory(factory);

    public IPostprocessComposer<T> Do(Action<T> action) => OnDo(action);

    public IPostprocessComposer<T> OmitAutoProperties() => OnOmitAutoProperties();

    public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker) =>
        OnAnonymousWith(propertyPicker);

    public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value) =>
        OnWithOverloadValue(propertyPicker, value);

    public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory) =>
        OnWithOverloadFactory(propertyPicker, valueFactory);

    public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory) =>
        OnWithOverloadFactory(propertyPicker, valueFactory);

    public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, ISpecimenBuilder builder) =>
        OnWithOverloadFactory(propertyPicker, builder);

    public IPostprocessComposer<T> WithAutoProperties() => OnWithAutoProperties();

    public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker) =>
        OnWithout(propertyPicker);

    public object Create(object request, ISpecimenContext context) => OnCreate(request, context);

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