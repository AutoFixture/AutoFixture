using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    public class NodeComposer<T> : 
        ICustomizationComposer<T>,
        ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilder builder;

        public NodeComposer(ISpecimenBuilder builder)
        {
            this.builder = builder;
        }

        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this.WithFactory(new SeededFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return this.WithFactory(factory);
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this.WithFactory(new SpecimenFactory<T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, T>(factory));
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T>(factory));
        }

        public ISpecimenBuilder Compose()
        {
            return this;
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            var targetToDecorate = this
                .SelectNodes(n =>
                    n is NoSpecimenOutputGuard ||
                    n is Postprocessor<T>)
                .First();

            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => new Postprocessor<T>(n, action),
                when: targetToDecorate.Equals);
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            var targetToRemove = this
                .SelectNodes(n => n is Postprocessor<T>)
                .FirstOrDefault();

            if (targetToRemove == null)
                return this;

            var p = this.Parents(targetToRemove.Equals).First();

            return (NodeComposer<T>)this.ReplaceNodes(
                with: targetToRemove.Concat(p.Where(b => targetToRemove != b)),
                when: p.Equals);
        }

        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            var targetToDecorate = this
                .SelectNodes(n => n is NoSpecimenOutputGuard)
                .First();

            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    n,
                    new BindingCommand<T, TProperty>(propertyPicker).Execute,
                    CreateSpecification()),
                when: targetToDecorate.Equals);
        }

        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            var targetToDecorate = this
                .SelectNodes(n => n is NoSpecimenOutputGuard)
                .First();

            var g = this.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    n,
                    new BindingCommand<T, TProperty>(propertyPicker, value).Execute,
                    CreateSpecification()),
                when: targetToDecorate.Equals);

            return (NodeComposer<T>)g.ReplaceNodes(
                with: n => ((NodeComposer<T>)n).Compose(
                    new []
                    {
                        new Omitter(
                            new EqualRequestSpecification(
                                propertyPicker.GetWritableMember().Member,
                                new MemberInfoEqualityComparer()))
                    }
                    .Concat(n)),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            var targetToDecorate = this
                .SelectNodes(n => n is NoSpecimenOutputGuard)
                .First();

            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    n,
                    new AutoPropertiesCommand<T>().Execute,
                    CreateSpecification()),
                when: targetToDecorate.Equals);
        }

        public IPostprocessComposer<T> Without<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => ((NodeComposer<T>)n).Compose(
                    new[]
                    {
                        new Omitter(
                            new EqualRequestSpecification(
                                propertyPicker.GetWritableMember().Member,
                                new MemberInfoEqualityComparer()))
                    }.Concat(n)),
                when: n => n is NodeComposer<T>);
        }

        public NodeComposer<T> WithAutoProperties(bool enable)
        {
            if (!enable)
                return (NodeComposer<T>)this.OmitAutoProperties();

            return (NodeComposer<T>)this.WithAutoProperties();
        }

        private NodeComposer<T> WithFactory(ISpecimenBuilder builder)
        {
            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => ((NoSpecimenOutputGuard)n).Compose(new[] { builder }),
                when: n => n is NoSpecimenOutputGuard);
        }        

        private static IRequestSpecification CreateSpecification()
        {
            return new OrRequestSpecification(
                new SeedRequestSpecification(typeof(T)),
                new ExactTypeSpecification(typeof(T)));
        }

        public ISpecimenBuilderNode Compose(
            IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder =
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NodeComposer<T>(composedBuilder);
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.builder.Create(request, context);
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.builder;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }
    }

    public static class NodeComposer
    {
        public static NodeComposer<T> Create<T>()
        {
            return new NodeComposer<T>(
                new FilteringSpecimenBuilder(
                    DecorateFactory<T>(
                        new MethodInvoker(
                            new ModestConstructorQuery())),
                    CreateSpecification<T>()));
        }

        private static ISpecimenBuilder DecorateFactory<T>(
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

        private static IRequestSpecification CreateSpecification<T>()
        {
            return new OrRequestSpecification(
                new SeedRequestSpecification(typeof(T)),
                new ExactTypeSpecification(typeof(T)));
        }
    }
}
