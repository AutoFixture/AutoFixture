using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public class NodeComposer<T> : FilteringSpecimenBuilder, ICustomizationComposer<T>
    {
        public NodeComposer()
            : this(new MethodInvoker(new ModestConstructorQuery()))
        {
        }

        public NodeComposer(ISpecimenBuilder factory)
            : base(
                DecorateFactory(factory),
                CreateSpecification())
        {
        }

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return new NodeComposer<T>(new SeededFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return new NodeComposer<T>(factory);
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return new NodeComposer<T>(new SpecimenFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            throw new NotImplementedException();
        }

        public Kernel.ISpecimenBuilder Compose()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        private static ISpecimenBuilder DecorateFactory(
            ISpecimenBuilder factory)
        {
            return new CompositeSpecimenBuilder(
                new NoSpecimenOutputGuard(
                    factory,
                    new InverseRequestSpecification(
                        new SeedRequestSpecification(
                            typeof(T)))),
                new SeedIgnoringRelay());
        }

        private static IRequestSpecification CreateSpecification()
        {
            return new OrRequestSpecification(
                new SeedRequestSpecification(typeof(T)),
                new ExactTypeSpecification(typeof(T)));
        }
    }
}
