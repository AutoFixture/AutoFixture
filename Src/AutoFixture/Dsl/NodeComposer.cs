using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Dsl
{
    /// <summary>
    /// Enables composition customization of a single type of specimen.
    /// </summary>
    /// <typeparam name="T">The type of specimen.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main responsibility of this class isn't to be a 'collection' (which, by the way, it isn't - it's just an Iterator).")]
    public class NodeComposer<T> :
        ICustomizationComposer<T>,
        ISpecimenBuilderNode
    {
        /// <summary>Gets the encapsulated builder.</summary>
        /// <value>The encapsulated builder.</value>
        /// <seealso cref="NodeComposer{T}(ISpecimenBuilder)" />
        public ISpecimenBuilder Builder { get; }

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

        /// <inheritdoc />
        public IPostprocessComposer<T> FromSeed(Func<T, T> factory)
        {
            return this.WithFactory(new SeededFactory<T>(factory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(ISpecimenBuilder factory)
        {
            return this.WithFactory(factory);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory(Func<T> factory)
        {
            return this.WithFactory(new SpecimenFactory<T>(factory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput>(Func<TInput, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput, T>(factory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2>(Func<TInput1, TInput2, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, T>(factory));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> FromFactory<TInput1, TInput2, TInput3>(Func<TInput1, TInput2, TInput3, T> factory)
        {
            return this.WithFactory(new SpecimenFactory<TInput1, TInput2, TInput3, T>(factory));
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
                with: n => n.Compose(n.Concat(new[] { new SeedIgnoringRelay() })),
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
                        new Postprocessor(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new ActionSpecimenCommand<T>(action))
                    }),
                when: container.Equals);
        }

        /// <inheritdoc />
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
            return (NodeComposer<T>)this.ReplaceNodes(
                with: n => new Postprocessor(
                    autoPropertiesNode.Builder,
                    autoPropertiesNode.Command,
                    new FalseRequestSpecification()),
                when: autoPropertiesNode.Equals);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker)
        {
            return this.WithCommand(
                propertyPicker,
                new BindingCommand<T, TProperty>(propertyPicker));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(
            Expression<Func<T, TProperty>> propertyPicker, TProperty value)
        {
            return this.WithCommand(
                propertyPicker,
                new BindingCommand<T, TProperty>(propertyPicker, value));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty>(Expression<Func<T, TProperty>> propertyPicker, Func<TProperty> valueFactory)
        {
            return this.WithCommand(
                propertyPicker,
                new BindingCommand<T, TProperty>(propertyPicker, _ => valueFactory.Invoke()));
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> With<TProperty, TInput>(Expression<Func<T, TProperty>> propertyPicker, Func<TInput, TProperty> valueFactory)
        {
            return this.WithCommand(
                propertyPicker,
                new BindingCommand<T, TProperty>(propertyPicker, context =>
                {
                    var arg = (TInput)context.Resolve(typeof(TInput));
                    return valueFactory.Invoke(arg);
                }));
        }

        private IPostprocessComposer<T> WithCommand<TProperty>(Expression<Func<T, TProperty>> propertyPicker, ISpecimenCommand command)
        {
            ExpressionReflector.VerifyIsNonNestedWritableMemberExpression(propertyPicker);

            var graphWithAutoPropertiesNode = this.GetGraphWithAutoPropertiesNode();
            var graphWithoutSeedIgnoringRelay = WithoutSeedIgnoringRelay(graphWithAutoPropertiesNode);

            var container = FindContainer(graphWithoutSeedIgnoringRelay);

            var graphWithProperty = graphWithoutSeedIgnoringRelay.ReplaceNodes(
                with: n => n.Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            command,
                            CreateSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: container.Equals);

            var member = propertyPicker.GetWritableMember().Member;
            return (NodeComposer<T>)ExcludeMemberFromAutoProperties(member, graphWithProperty);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> WithAutoProperties()
        {
            var g = this.GetGraphWithAutoPropertiesNode();
            var autoProperties = FindAutoPropertiesNode(g);

            return (NodeComposer<T>)g.ReplaceNodes(
                with: n => new Postprocessor(
                    autoProperties.Builder,
                    autoProperties.Command,
                    new TrueRequestSpecification()),
                when: autoProperties.Equals);
        }

        /// <inheritdoc />
        public IPostprocessComposer<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            if (propertyPicker == null) throw new ArgumentNullException(nameof(propertyPicker));

            ExpressionReflector.VerifyIsNonNestedWritableMemberExpression(propertyPicker);

            var member = propertyPicker.GetWritableMember().Member;
            var graphWithAutoPropertiesNode = this.GetGraphWithAutoPropertiesNode();

            return (NodeComposer<T>)ExcludeMemberFromAutoProperties(member, graphWithAutoPropertiesNode);
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

        /// <inheritdoc />
        public ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            if (builders == null) throw new ArgumentNullException(nameof(builders));

            var composedBuilder =
                CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new NodeComposer<T>(composedBuilder);
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            return this.Builder.Create(request, context);
        }

        /// <inheritdoc />
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.Builder;
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Looks for the AutoProperties postprocessor in the current graph.
        /// If postprocessor is missing, it's created in inactive state.
        /// </summary>
        private NodeComposer<T> GetGraphWithAutoPropertiesNode()
        {
            var existingNode = FindAutoPropertiesNode(this);
            if (existingNode != null) return this;

            var g = WithoutSeedIgnoringRelay(this);
            var filter = FindContainer(g);

            // Create AutoProperties node in inactive state
            return (NodeComposer<T>)g.ReplaceNodes(
                with: n => n.Compose(
                    new ISpecimenBuilder[]
                    {
                        new Postprocessor(
                            CompositeSpecimenBuilder.ComposeIfMultiple(n),
                            new AutoPropertiesCommand(typeof(T)),
                            new FalseRequestSpecification()),
                        new SeedIgnoringRelay()
                    }),
                when: filter.Equals);
        }

        private static Postprocessor FindAutoPropertiesNode(ISpecimenBuilderNode graph)
        {
            return (Postprocessor)graph
                .FindFirstNodeOrDefault(n => n is Postprocessor postprocessor &&
                                             postprocessor.Command is AutoPropertiesCommand);
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

            var currentSpecification = ((AutoPropertiesCommand)autoPropertiesNode.Command).Specification;
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
                specification = new AndRequestSpecification(andSpec.Specifications.Concat(new[] { newRule }));
            }
            else
            {
                specification = new AndRequestSpecification(currentSpecification, newRule);
            }

            return graph.ReplaceNodes(
                with: _ => new Postprocessor(
                    autoPropertiesNode.Builder,
                    new AutoPropertiesCommand(typeof(T), specification),
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
