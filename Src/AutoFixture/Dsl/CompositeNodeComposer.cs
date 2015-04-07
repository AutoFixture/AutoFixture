using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Dsl
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
        private readonly ISpecimenBuilderNode node;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CompositeNodeComposer{T}" /> class.
        /// </summary>
        /// <param name="node">
        /// A node which may contain <see cref="NodeComposer{T}" /> sub-nodes.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// node is null
        /// </exception>
        /// <seealso cref="Node" />
        public CompositeNodeComposer(ISpecimenBuilderNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            this.node = node;
        }

        /// <summary>
        /// Specifies a function that defines how to create a specimen from a
        /// seed.
        /// </summary>
        /// <param name="factory">
        /// The factory used to create specimens from seeds.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromSeed(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies an <see cref="ISpecimenBuilder"/> that can create
        /// specimens of the appropriate type. Mostly for advanced scenarios.
        /// </summary>
        /// <param name="factory">
        /// An <see cref="ISpecimenBuilder"/> that can create specimens of the
        /// appropriate type.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies that an anonymous object should be created in a
        /// particular way; often by using a constructor.
        /// </summary>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often
        /// be a constructor.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way,
        /// using a single input parameter for the factory.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often
        /// be a constructor that takes a single constructor argument of type
        /// <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput>(
            Func<TInput, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way,
        /// using two input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often
        /// be a constructor that takes two constructor arguments of type
        /// <typeparamref name="TInput1"/> and <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(
            Func<TInput1, TInput2, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way,
        /// using three input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often
        /// be a constructor that takes three constructor arguments of type
        /// <typeparamref name="TInput1"/>, <typeparamref name="TInput2"/> and
        /// <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(
            Func<TInput1, TInput2, TInput3, T> factory)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).FromFactory(factory),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way,
        /// using four input parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter to use when invoking
        /// <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often
        /// be a constructor that takes three constructor arguments of type
        /// <typeparamref name="TInput1"/>, <typeparamref name="TInput2"/>,
        /// <typeparamref name="TInput3"/> and <typeparamref name="TInput4"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
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
        public ISpecimenBuilder Compose()
        {
            return this;
        }

        /// <summary>
        /// Performs the specified action on a specimen.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> Do(Action<T> action)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).Do(action),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Disables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> OmitAutoProperties()
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).OmitAutoProperties(),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Registers that a writable property or field should be assigned an
        /// anonymous value as part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property of field.
        /// </typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will
        /// should have a value
        /// assigned.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Registers that a writable property or field should be assigned a
        /// specific value as part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property of field.
        /// </typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field that will have
        /// <paramref name="value"/> assigned.
        /// </param>
        /// <param name="value">
        /// The value to assign to the property or field identified by
        /// <paramref name="propertyPicker"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).With(propertyPicker, value),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Enables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Although the default behavior of <see cref="Fixture" /> may make
        /// this method seem redundant, the purpose of this method is to enable
        /// an explicit opt-in for certain types, in the case where a Fixture
        /// instance has been configured to <em>not</em> auto-fill properties
        /// by default (e.g. if <see cref="Fixture.OmitAutoProperties" /> is
        /// set to <see langword="true" />).
        /// </para>
        /// </remarks>
        /// <example>
        /// In this example, result.Property will be assigned a value, even
        /// though this isn't the default behavior of the Fixture instance.
        /// <code>
        /// var fixture = new Fixture { OmitAutoProperties = true };
        /// PropertyHolder&lt;object&gt; result = fixture
        ///    .Build&lt;PropertyHolder&lt;object&gt;&gt;()
        ///    .WithAutoProperties()
        ///    .Create();
        /// </code>
        /// </example>
        public IPostprocessComposer<T> WithAutoProperties()
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).WithAutoProperties(),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>
        /// Withouts the specified property picker.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyPicker">The property picker.</param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> Without<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            return (CompositeNodeComposer<T>)this.ReplaceNodes(
                with: n =>
                    (NodeComposer<T>)((NodeComposer<T>)n).Without(propertyPicker),
                when: n => n is NodeComposer<T>);
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
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

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// The requested specimen if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <paramref name="request" /> can be any object, but will often
        /// be a <see cref="Type" /> or other
        /// <see cref="System.Reflection.MemberInfo" /> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            return this.node.Create(request, context);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{ISpecimenBuilder}" /> that can be used to
        /// iterate through the collection.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.node;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can
        /// be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Gets the encapsulated node.</summary>
        /// <value>The encapsulated node.</value>
        /// <seealso cref="CompositeNodeComposer{T}(ISpecimenBuilderNode)" />
        public ISpecimenBuilderNode Node
        {
            get { return this.node; }
        }
    }
}
