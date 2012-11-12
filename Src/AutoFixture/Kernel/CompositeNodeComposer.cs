using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture.Kernel
{
    public class CompositeNodeComposer<T> : 
        ICustomizationComposer<T>,
        ISpecimenBuilderNode
    {
        private readonly ISpecimenBuilderNode node;

        public CompositeNodeComposer(ISpecimenBuilderNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            this.node = node;
        }
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromSeed(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory<TInput>(
            Func<TInput, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(
            Func<TInput1, TInput2, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(
            Func<TInput1, TInput2, TInput3, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(
            Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        public ISpecimenBuilder Compose()
        {
            return this;
        }

        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).Do(action),
                when: n => n is NodeComposer<T>);
        }

        public IPostprocessComposer<T> OmitAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> With<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> WithAutoProperties()
        {
            throw new NotImplementedException();
        }

        public IPostprocessComposer<T> Without<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyPicker)
        {
            throw new NotImplementedException();
        }

        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var isSingle = builders.Take(2).Count() == 1;
            if (isSingle)
            {
                var single = builders.Single() as ISpecimenBuilderNode;
                if (single != null)
                    return new CompositeNodeComposer<T>(single);
            }

            return new CompositeNodeComposer<T>(
                new CompositeSpecimenBuilder(
                    builders));
        }

        public object Create(object request, ISpecimenContext context)
        {
            return this.node.Create(request, context);
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.node;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ISpecimenBuilderNode Node
        {
            get { return this.node; }
        }
    }
}
