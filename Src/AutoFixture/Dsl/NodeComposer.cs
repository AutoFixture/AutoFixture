﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Linq.Expressions;

namespace Ploeh.AutoFixture.Dsl
{
    /// <summary>
    /// Enables composition customization of a single type of specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class NodeComposer<T> : 
        ICustomizationComposer<T>,
        ISpecimenBuilderNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeComposer{T}" />
        /// class.
        /// </summary>
        /// <param name="builder">
        /// A builder to which specimen creation responsibilities are
        /// delegated.
        /// </param>
        /// <remarks>
        /// A new <see cref="NodeComposer{T}" /> instance with an appropriate
        /// initial <paramref name="builder" /> can be easily produced by
        /// <see cref="SpecimenBuilderNodeFactory.CreateComposer{T}()" />.
        /// </remarks>
        /// <seealso cref="SpecimenBuilderNodeFactory.CreateComposer{T}()" />
        /// <seealso cref="Builder" />
        public NodeComposer(ISpecimenBuilder builder)
        {
            this.Builder = builder;
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
            return this.WithFactory(new SeededFactory<T>(factory));
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
            return this.WithFactory(factory);
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
            return this.WithFactory(new SpecimenFactory<T>(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using a single input
        /// parameter for the factory.
        /// </summary>
        /// <typeparam name="TInput">
        /// The type of input parameter to use when invoking <paramref name="factory"/>
        /// .</typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes a single constructor argument of type <typeparamref name="TInput"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput, T>(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using two input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes two constructor arguments of type <typeparamref name="TInput1"/> and
        /// <typeparamref name="TInput2"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, T>(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using three input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/> and <typeparamref name="TInput3"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, T>(factory));
        }

        /// <summary>
        /// Specifies that a specimen should be created in a particular way, using four input
        /// parameters for the construction.
        /// </summary>
        /// <typeparam name="TInput1">
        /// The type of the first input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput2">
        /// The type of the second input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput3">
        /// The type of the third input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <typeparam name="TInput4">
        /// The type of the fourth input parameter to use when invoking <paramref name="factory"/>.
        /// </typeparam>
        /// <param name="factory">
        /// A function that will be used to create the object. This will often be a constructor
        /// that takes three constructor arguments of type <typeparamref name="TInput1"/>,
        /// <typeparamref name="TInput2"/>, <typeparamref name="TInput3"/> and
        /// <typeparamref name="TInput4"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to further customize the
        /// post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3, TInput4>(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T>(factory));
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
            var graphWithoutSeedIgnoringRelay = WithoutSeedIgnoringRelay(this);

            var container = FindContainer(graphWithoutSeedIgnoringRelay);
            var autoPropertiesNode = FindAutoPropertiesNode(container);
            if (autoPropertiesNode != null)
                container = autoPropertiesNode;

            var graphWithDoNode = WithDoNode(
                action,
                graphWithoutSeedIgnoringRelay,
                container);

            var filter = FindContainer(graphWithDoNode);
            return (NodeComposer<T>)graphWithDoNode.ReplaceNodes(
                with: n => n.Compose(n.Concat(new [] { new SeedIgnoringRelay() })),
                when: filter.Equals);
        }

        private static NodeComposer<T> WithDoNode(
            Action<T> action,
            ISpecimenBuilderNode graph,
            ISpecimenBuilderNode container)
        {
            return (NodeComposer<T>)graph.ReplaceNodes(
                with: n => n.Compose(
                    new[]
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new ActionSpecimenCommand<T>(action))
                    }),
                when: container.Equals);
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
            var autoPropertiesNode = FindAutoPropertiesNode(this);

            if (autoPropertiesNode == null)
                return this;

            // We disable postprocessor rather than delete it.
            // The reason is that it's the only holder of which properties/fields should not be populated.
            // If user later decide to enable properties population, we'll need this information 
            // (e.g. if user does smth like fixture.Build().Without(x => x.P).OmitAutoProperties().WithAutoProperties()
            // the information from "Without" should not be missed)
            return (NodeComposer<T>) this.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    autoPropertiesNode.Builder,
                    autoPropertiesNode.Command,
                    new FalseRequestSpecification()),
                when: autoPropertiesNode.Equals);
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
            ExpressionReflector.VerifyIsNonNestedWritableMemberExpression(propertyPicker);
            
            var targetToDecorate = this.FindFirstNode(n => n is NoSpecimenOutputGuard);

            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    n,
                    new BindingCommand<T, TProperty>(propertyPicker),
                    CreateSpecification()),
                when: targetToDecorate.Equals);
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
            ExpressionReflector.VerifyIsNonNestedWritableMemberExpression(propertyPicker);
            
            var graphWithAutoPropertiesNode = this.GetGraphWithAutoPropertiesNode();
            var graphWithoutSeedIgnoringRelay = WithoutSeedIgnoringRelay(graphWithAutoPropertiesNode);

            var container = FindContainer(graphWithoutSeedIgnoringRelay);
         
            var graphWithProperty = graphWithoutSeedIgnoringRelay.ReplaceNodes(
                with: n => n.Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new BindingCommand<T, TProperty>(propertyPicker, value),
                            CreateSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: container.Equals);

            var member = propertyPicker.GetWritableMember().Member;
            return (NodeComposer<T>) ExcludeMemberFromAutoProperties(member, graphWithProperty);
        }

        /// <summary>
        /// Enables auto-properties for a type of specimen.
        /// </summary>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> WithAutoProperties()
        {
            var g = GetGraphWithAutoPropertiesNode();
            var autoProperties = FindAutoPropertiesNode(g);

            return (NodeComposer<T>) g.ReplaceNodes(
                with: n => new Postprocessor<T>(
                    autoProperties.Builder,
                    autoProperties.Command,
                    new TrueRequestSpecification()),
                when: autoProperties.Equals);
        }

        /// <summary>
        /// Registers that a writable property should not be assigned an
        /// automatic value as part of specimen post-processing.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property or field to ignore.
        /// </typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field to be ignored.
        /// </param>
        /// <returns>
        /// An <see cref="IPostprocessComposer{T}"/> which can be used to
        /// further customize the post-processing of created specimens.
        /// </returns>
        public IPostprocessComposer<T> Without<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            ExpressionReflector.VerifyIsNonNestedWritableMemberExpression(propertyPicker);
            
            var member = propertyPicker.GetWritableMember().Member;
            var graphWithAutoPropertiesNode = GetGraphWithAutoPropertiesNode();

            return (NodeComposer<T>) ExcludeMemberFromAutoProperties(member, graphWithAutoPropertiesNode);
        }

        /// <summary>
        /// Controls whether auto-properties will be enabled or not.
        /// </summary>
        /// <param name="enable">
        /// Set to <see langword="true"/> to enable auto-properties.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="NodeComposer{T}" /> where
        /// auto-properties are either enabled or disabled according to
        /// <paramref name="enable" />.
        /// </returns>
        public NodeComposer<T> WithAutoProperties(bool enable)
        {
            if (!enable)
                return (NodeComposer<T>)this.OmitAutoProperties();

            return (NodeComposer<T>)this.WithAutoProperties();
        }

        private NodeComposer<T> WithFactory(ISpecimenBuilder factory)
        {
            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => n.Compose(new[] { factory }),
                when: n => n is NoSpecimenOutputGuard);
        }        

        private static IRequestSpecification CreateSpecification()
        {
            return new OrRequestSpecification(
                new SeedRequestSpecification(typeof(T)),
                new ExactTypeSpecification(typeof(T)));
        }

        /// <summary>Composes the supplied builders.</summary>
        /// <param name="builders">The builders to compose.</param>
        /// <returns>
        /// A new <see cref="ISpecimenBuilderNode" /> instance containing
        /// <paramref name="builders" /> as child nodes.
        /// </returns>
        public ISpecimenBuilderNode Compose(
            IEnumerable<ISpecimenBuilder> builders)
        {
            var composedBuilder =
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NodeComposer<T>(composedBuilder);
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
            return this.Builder.Create(request, context);
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
            yield return this.Builder;
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

        /// <summary>Gets the encapsulated builder.</summary>
        /// <value>The encapsulated builder.</value>
        /// <seealso cref="NodeComposer{T}(ISpecimenBuilder)" />
        public ISpecimenBuilder Builder { get; }

        /// <summary>
        /// Looks for the AutoProperties postprocessor in the current graph.
        /// If postprocessor is missing, it's created in inactive state.
        /// </summary>
        private NodeComposer<T> GetGraphWithAutoPropertiesNode()
        {
            var existingNode = FindAutoPropertiesNode(this);
            if(existingNode != null) return this;
            
            var g = WithoutSeedIgnoringRelay(this);
            var filter = FindContainer(g);

            //Create AutoProperties node in inactive state
            return (NodeComposer<T>)g.ReplaceNodes(
                with: n => n.Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor<T>(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new AutoPropertiesCommand<T>(),
                            new FalseRequestSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: filter.Equals);
        }

        private static Postprocessor<T> FindAutoPropertiesNode(ISpecimenBuilderNode graph)
        {
            return (Postprocessor<T>) graph
                .FindFirstNodeOrDefault(n => n is Postprocessor<T> postprocessor &&
                                             postprocessor.Command is AutoPropertiesCommand<T>);
        }
        
        /// <summary>
        /// Adjusts the AutoProperties postprocessor and changes rule to avoid the specified member population.
        /// If AutoProperties node is missing, nothing is done. 
        /// </summary>
        private static ISpecimenBuilderNode ExcludeMemberFromAutoProperties(MemberInfo member, 
            ISpecimenBuilderNode graph)
        {
            var autoPropertiesNode = FindAutoPropertiesNode(graph);
            if (autoPropertiesNode == null) return graph;

            var currentSpecification = ((AutoPropertiesCommand<T>) autoPropertiesNode.Command).Specification;
            var newRule = new InverseRequestSpecification(
                new EqualRequestSpecification(
                    member,
                    new MemberInfoEqualityComparer()));

            // Try to make specification list flat if possible
            IRequestSpecification specification;
            if (currentSpecification is TrueRequestSpecification)
            {
                specification = newRule;
            }
            else if (currentSpecification is AndRequestSpecification andSpec)
            {
                specification = new AndRequestSpecification(andSpec.Specifications.Concat(new[] {newRule}));
            }
            else
            {
                specification = new AndRequestSpecification(currentSpecification, newRule);
            }

            return graph.ReplaceNodes(
                with: _ => new Postprocessor<T>(
                    autoPropertiesNode.Builder,
                    new AutoPropertiesCommand<T>(specification),
                    autoPropertiesNode.Specification),
                when: autoPropertiesNode.Equals);
        }
        
        private static ISpecimenBuilderNode WithoutSeedIgnoringRelay(ISpecimenBuilderNode graph)
        {
            var g = graph.ReplaceNodes(
                with: n => CompositeSpecimenBuilder.UnwrapIfSingle(
                    n.Compose(n.Where(b => !(b is SeedIgnoringRelay)))),
                when: n => n.OfType<SeedIgnoringRelay>().Any());
            return g;
        }

        private static ISpecimenBuilderNode FindContainer(ISpecimenBuilderNode graph)
        {
            return graph.FindFirstNode(n => n is FilteringSpecimenBuilder);
        }
    }
}
