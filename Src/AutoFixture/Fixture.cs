using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using AutoFixture.DataAnnotations;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Provides object creation services.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
        Justification = "Fixture is coupled to many other types, because it embodies rules for creating various well-known types from the base class library.")]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "The main purpose of Fixture isn't to be a collection of anything." +
                        "That it implements IEnumerable is just a coincidental side effect of how graphs are implemented in AutoFixture. " +
                        "Besides, fixing this CA error would be a breaking change.")]
    public class Fixture : IFixture, IEnumerable<ISpecimenBuilder>
    {
        private readonly MultipleRelay multiple;

        private ISpecimenBuilderNode graph;

        private SingletonSpecimenBuilderNodeStackAdapterCollection behaviors;
        private SpecimenBuilderNodeAdapterCollection customizer;
        private SpecimenBuilderNodeAdapterCollection residueCollector;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
            : this(new DefaultEngineParts())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine parts.
        /// </summary>
        public Fixture(DefaultRelays engineParts)
            : this(
                engineParts != null
                    ? new CompositeSpecimenBuilder(engineParts)
                    : throw new ArgumentNullException(nameof(engineParts)),
                new MultipleRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine
        /// and a definition of what 'many' means.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="multiple">The definition and implementation of 'many'.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification = "This is the Façade that binds everything else together, so being coupled to many other types must follow.")]
        public Fixture(ISpecimenBuilder engine, MultipleRelay multiple)
        {
            this.Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            this.multiple = multiple ?? throw new ArgumentNullException(nameof(multiple));

            ISpecimenBuilderNode newGraph =
                new BehaviorRoot(
                    new TerminatingWithPathSpecimenBuilder(new CompositeSpecimenBuilder(
                        new CustomizationNode(
                            new CompositeSpecimenBuilder(
                                new FilteringSpecimenBuilder(
                                    new FixedBuilder(
                                        this),
                                    new OrRequestSpecification(
                                        new ExactTypeSpecification(
                                            typeof(Fixture)),
                                        new ExactTypeSpecification(
                                            typeof(IFixture)),
                                        new ExactTypeSpecification(
                                            typeof(ISpecimenBuilder)))),
                                new StableFiniteSequenceRelay(),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(Dictionary<,>),
                                    new ModestConstructorQuery(),
                                    new DictionaryFiller()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(SortedDictionary<,>),
                                    new ModestConstructorQuery(),
                                    new DictionaryFiller()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(SortedList<,>),
                                    new ModestConstructorQuery(),
                                    new DictionaryFiller()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(Collection<>),
                                    new ListFavoringConstructorQuery()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(List<>),
                                    new EnumerableFavoringConstructorQuery()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(HashSet<>),
                                    new EnumerableFavoringConstructorQuery()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(SortedSet<>),
                                    new EnumerableFavoringConstructorQuery()),
                                MakeQueryBasedBuilderForMatchingType(
                                    typeof(ObservableCollection<>),
                                    new EnumerableFavoringConstructorQuery()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new NullableEnumRequestSpecification()),
                                new EnumGenerator(),
                                new LambdaExpressionGenerator(),
                                MakeFixedBuilder(
                                    CultureInfo.InvariantCulture),
                                MakeFixedBuilder(
                                    Encoding.UTF8),
                                MakeFixedBuilder(
                                    IPAddress.Loopback),
                                new DataAnnotationsSupportNode(
                                    new CompositeSpecimenBuilder(
                                        new RangeAttributeRelay(),
                                        new NumericRangedRequestRelay(),
                                        new EnumRangedRequestRelay(),
                                        new TimeSpanRangedRequestRelay(),
                                        new StringLengthAttributeRelay(),
                                        new MinAndMaxLengthAttributeRelay(),
                                        new RegularExpressionAttributeRelay())))),
                        new AutoPropertiesTarget(
                            new Postprocessor(
                                new CompositeSpecimenBuilder(
                                    engine,
                                    multiple),
                                new AutoPropertiesCommand(),
                                new AnyTypeSpecification())),
                        new ResidueCollectorNode(
                            new CompositeSpecimenBuilder(
                                new TypeRelay(
                                    typeof(IDictionary<,>),
                                    typeof(Dictionary<,>)),
                                new TypeRelay(
                                    typeof(IReadOnlyDictionary<,>),
                                    typeof(ReadOnlyDictionary<,>)),
                                new TypeRelay(
                                    typeof(ICollection<>),
                                    typeof(List<>)),
                                new TypeRelay(
                                    typeof(IReadOnlyCollection<>),
                                    typeof(ReadOnlyCollection<>)),
                                new TypeRelay(
                                    typeof(IList<>),
                                    typeof(List<>)),
                                new TypeRelay(
                                    typeof(IReadOnlyList<>),
                                    typeof(ReadOnlyCollection<>)),
                                new TypeRelay(
                                    typeof(ISet<>),
                                    typeof(HashSet<>)),
                                new EnumerableRelay(),
                                new EnumeratorRelay())),
                        new FilteringSpecimenBuilder(
                            new MutableValueTypeWarningThrower(),
                            new AndRequestSpecification(
                                new ValueTypeSpecification(),
                                new NoConstructorsSpecification())))));

            this.UpdateGraphAndSetupAdapters(newGraph, Enumerable.Empty<ISpecimenBuilderTransformation>());

            this.Behaviors.Add(new ThrowingRecursionBehavior());
        }

        /// <inheritdoc />
        public IList<ISpecimenBuilderTransformation> Behaviors => this.behaviors;

        /// <inheritdoc />
        public IList<ISpecimenBuilder> Customizations => this.customizer;

        /// <summary>
        /// Gets the core engine of the <see cref="Fixture"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the core engine that drives a <see cref="Fixture"/> instance. Even with no
        /// <see cref="Customizations"/> or <see cref="ResidueCollectors"/>, the
        /// <see cref="Engine"/> should be capably of resolving a wide range of different requests,
        /// based on conventions.
        /// </para>
        /// </remarks>
        /// <see cref="Customizations"/>
        /// <see cref="ResidueCollectors"/>
        public ISpecimenBuilder Engine { get; }

        /// <inheritdoc />
        public bool OmitAutoProperties
        {
            get => this.FindAutoPropertiesPostProcessor().Specification is FalseRequestSpecification;
            set
            {
                IRequestSpecification newSpecification = value
                    ? (IRequestSpecification)new FalseRequestSpecification()
                    : new AnyTypeSpecification();

                var existingPostProcessor = this.FindAutoPropertiesPostProcessor();

                // Optimization. Do nothing if no change is required (i.e. you set property to its current value).
                if (existingPostProcessor.Specification.GetType() == newSpecification.GetType())
                    return;

                var updatedPostProcessor = new Postprocessor(
                    existingPostProcessor.Builder,
                    existingPostProcessor.Command,
                    newSpecification);

                var updatedGraph = this.graph.ReplaceNodes(
                    with: updatedPostProcessor,
                    when: existingPostProcessor.Equals);

                this.UpdateGraphAndSetupAdapters(updatedGraph);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a number that controls how many objects are created when a
        /// <see cref="Fixture"/> creates more than one anonymous objects.
        /// </summary>
        public int RepeatCount
        {
            get => this.multiple.Count;
            set => this.multiple.Count = value;
        }

        /// <inheritdoc />
        public IList<ISpecimenBuilder> ResidueCollectors => this.residueCollector;

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This method is required to be generic by design, so there is no way to refactor it.")]
        public ICustomizationComposer<T> Build<T>()
        {
            var g = this.graph.ReplaceNodes(
                with: n => new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<T>().WithAutoProperties(this.EnableAutoProperties),
                    n),
                when: n => n is BehaviorRoot);

            return new CompositeNodeComposer<T>(g);
        }

        /// <inheritdoc />
        public IFixture Customize(ICustomization customization)
        {
            if (customization == null) throw new ArgumentNullException(nameof(customization));

            customization.Customize(this);
            return this;
        }

        /// <inheritdoc />
        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (composerTransformation == null) throw new ArgumentNullException(nameof(composerTransformation));

            var c = composerTransformation(SpecimenBuilderNodeFactory.CreateComposer<T>().WithAutoProperties(this.EnableAutoProperties));
            this.customizer.Insert(0, c);
        }

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (context == null) throw new ArgumentNullException(nameof(context));

            return this.graph.Create(request, context);
        }

        /// <summary>
        /// Gets an enumerator over the internal specimen builders used to create objects.
        /// </summary>
        /// <returns>
        /// An enumerator over the internal specimen builders used to create objects.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.graph;
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

        private bool EnableAutoProperties => !this.OmitAutoProperties;

        private void UpdateGraphAndSetupAdapters(ISpecimenBuilderNode newGraph)
        {
            if (this.Behaviors == null)
            {
                throw new InvalidOperationException(
                    "Behaviors should be already initialized. Please verify the method invocation logic to fix that.");
            }

            this.UpdateGraphAndSetupAdapters(newGraph, this.Behaviors);
        }

        private void UpdateGraphAndSetupAdapters(
            ISpecimenBuilderNode newGraph, IEnumerable<ISpecimenBuilderTransformation> existingBehaviors)
        {
            this.graph = newGraph;

            this.UpdateCustomizer();
            this.UpdateResidueCollector();
            this.UpdateBehaviors(existingBehaviors.ToArray());
        }

        private void UpdateCustomizer()
        {
            this.customizer =
                new SpecimenBuilderNodeAdapterCollection(
                    this.graph,
                    n => n is CustomizationNode);
            this.customizer.GraphChanged += (_, args) => this.UpdateGraphAndSetupAdapters(args.Graph);
        }

        private void UpdateResidueCollector()
        {
            this.residueCollector =
                new SpecimenBuilderNodeAdapterCollection(
                    this.graph,
                    n => n is ResidueCollectorNode);
            this.residueCollector.GraphChanged += (_, args) => this.UpdateGraphAndSetupAdapters(args.Graph);
        }

        private void UpdateBehaviors(ISpecimenBuilderTransformation[] existingTransformations)
        {
            this.behaviors =
                new SingletonSpecimenBuilderNodeStackAdapterCollection(
                    this.graph,
                    n => n is BehaviorRoot,
                    existingTransformations);
            this.behaviors.GraphChanged += (_, args) => this.UpdateGraphAndSetupAdapters(args.Graph);
        }

        private Postprocessor FindAutoPropertiesPostProcessor()
        {
            var postprocessorHolder = (AutoPropertiesTarget)this.graph.FindFirstNode(b => b is AutoPropertiesTarget);
            return (Postprocessor)postprocessorHolder.Builder;
        }

        private static ISpecimenBuilder MakeFixedBuilder<T>(T value)
        {
            return new FilteringSpecimenBuilder(
                new FixedBuilder(
                    value),
                new ExactTypeSpecification(
                    typeof(T)));
        }

        private static ISpecimenBuilder MakeQueryBasedBuilderForMatchingType(Type matchingType, IMethodQuery query)
        {
            return new FilteringSpecimenBuilder(
                new MethodInvoker(
                    query),
                new ExactTypeSpecification(
                    matchingType));
        }

        private static ISpecimenBuilder MakeQueryBasedBuilderForMatchingType(Type matchingType, IMethodQuery query,
            ISpecimenCommand command)
        {
            return new FilteringSpecimenBuilder(
                new Postprocessor(
                    new MethodInvoker(
                        query),
                    command),
                new ExactTypeSpecification(
                    matchingType));
        }
    }
}
