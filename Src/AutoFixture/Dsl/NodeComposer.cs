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
#warning Refactor this bloody mess
            Expression<Action<AutoPropertiesCommand<T>, T, ISpecimenContext>> exp = (cmd, s, ctx) => cmd.Execute(s, ctx);
            var m = ((MethodCallExpression)exp.Body).Method;

            var g = this.ReplaceNodes(
                with: n => CompositeSpecimenBuilder.UnwrapIfSingle(
                    n.Compose(n.Where(b => !(b is SeedIgnoringRelay)))),
                when: n => n.OfType<SeedIgnoringRelay>().Any());

            var container = g
                .SelectNodes(n => n is FilteringSpecimenBuilder)
                .First();

            var autoProperties = container
                .SelectNodes(n =>
                {
                    var pp = n as Postprocessor<T>;
                    if (pp == null)
                        return false;
                    return m.Equals(pp.Action.Method);
                })
                .FirstOrDefault();

            if (autoProperties != null)
                container = autoProperties;            

            var g1 = (NodeComposer<T>)g.ReplaceNodes(
                with: n => ((ISpecimenBuilderNode)n).Compose(
                    new []
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            action)
                    }),
                when: container.Equals);

            var filter = g1
                .SelectNodes(n => n is FilteringSpecimenBuilder)
                .First();

            return (NodeComposer<T>)g1.ReplaceNodes(
                with: n => n.Compose(n.Concat(new [] { new SeedIgnoringRelay() })),
                when: filter.Equals);
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
#warning Argh! My eyes! Refactor!
            var g = this.ReplaceNodes(
                with: n => CompositeSpecimenBuilder.UnwrapIfSingle(
                    n.Compose(n.Where(b => !(b is SeedIgnoringRelay)))),
                when: n => n.OfType<SeedIgnoringRelay>().Any());

            var filter = g
                .SelectNodes(n => n is FilteringSpecimenBuilder)
                .First();
         
            var g1 = g.ReplaceNodes(
                with: n => ((FilteringSpecimenBuilder)n).Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new BindingCommand<T, TProperty>(propertyPicker, value).Execute,
                            CreateSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: filter.Equals);

            return (NodeComposer<T>)g1.ReplaceNodes(
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
            var g = this.ReplaceNodes(
                with: n => CompositeSpecimenBuilder.UnwrapIfSingle(
                    n.Compose(n.Where(b => !(b is SeedIgnoringRelay)))),
                when: n => n.OfType<SeedIgnoringRelay>().Any());

            var filter = g
                .SelectNodes(n => n is FilteringSpecimenBuilder)
                .First();

            return (NodeComposer<T>)g.ReplaceNodes(
                with: n => ((FilteringSpecimenBuilder)n).Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new AutoPropertiesCommand<T>().Execute,
                            CreateSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: filter.Equals);
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
}
