using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
{
    /// <summary>
    /// Aggregates an arbitrary number of
    /// <see cref="ICustomizationComposer{T}"/> instances by wrapping a
    /// <see cref="ISpecimenBuilderNode" />.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    /// <remarks>
    /// <para>
    /// This implementation finds all <see cref="NodeComposer{T}" /> sub-nodes
    /// in the wrapped graph and applies the appropriate method to all matches.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class CompositeNodeComposer<T> :
        ICustomizationComposer<T>,
        ISpecimenBuilderNode
    {
        /// <summary>Gets the encapsulated node.</summary>
        /// <value>The encapsulated node.</value>
        /// <seealso cref="CompositeNodeComposer{T}(ISpecimenBuilderNode)" />
        public ISpecimenBuilderNode Node { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeNodeComposer{T}" /> class.
        /// </summary>
        /// <param name="node">
        /// A node which may contain <see cref="NodeComposer{T}" /> sub-nodes.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// node is null.
        /// </exception>
        /// <seealso cref="Node" />
        public CompositeNodeComposer(ISpecimenBuilderNode node)
        {
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromSeed(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(
            Func<TInput1, TInput2, TInput3, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(
            Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance which can be used to
        /// produce specimens according to the behavior specified by previous
        /// method calls.
        /// </returns>
        public ISpecimenBuilder Compose() => this;

        /// <inheritdoc />
        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).Do(action),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).OmitAutoProperties(),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker, value),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker, valueFactory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker, valueFactory),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> WithAutoProperties()
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).WithAutoProperties(),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).Without(propertyPicker),
                when: n => n is NodeComposer<T>);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            return this.Node.Create(request, context);
        }

        /// <inheritdoc />
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.Node;
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
