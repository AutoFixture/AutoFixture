using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.DataAnnotations;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;
using System.Globalization;
using System.Text;
using System.Net;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation services.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Fixture is coupled to many other types, because it embodies rules for creating various well-known types from the base class library.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The main purpose of Fixture isn't to be a collection of anything. That it implements IEnumerable is just a coincidental side effect of how graphs are implemented in AutoFixture. Besides, fixing this CA error would be a breaking change.")]
    public class Fixture : IFixture, IEnumerable<ISpecimenBuilder>
    {
        private SingletonSpecimenBuilderNodeStackAdapterCollection behaviors;
        private SpecimenBuilderNodeAdapterCollection customizer;
        private SpecimenBuilderNodeAdapterCollection residueCollector;
        private readonly MultipleRelay multiple;

        private ISpecimenBuilderNode graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
            : this(new DefaultEngineParts())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine
        /// parts.
        /// </summary>
        /// <param name="engineParts">The engine parts.</param>
        public Fixture(DefaultRelays engineParts)
            : this(new CompositeSpecimenBuilder(engineParts), new MultipleRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine
        /// and a definition of what 'many' means.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="multiple">The definition and implementation of 'many'.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is the Façade that binds everything else together, so being coupled to many other types must follow.")]
        public Fixture(ISpecimenBuilder engine, MultipleRelay multiple)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
            if (multiple == null)
            {
                throw new ArgumentNullException(nameof(multiple));
            }

            this.Engine = engine;
            this.multiple = multiple;

            this.graph =
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
                                new FilteringSpecimenBuilder(
                                    new Postprocessor(
                                        new MethodInvoker(
                                            new ModestConstructorQuery()),
                                        new DictionaryFiller()),
                                    new DictionarySpecification()),
                                new FilteringSpecimenBuilder(
                                    new Postprocessor(
                                        new MethodInvoker(
                                            new ModestConstructorQuery()),
                                        new DictionaryFiller()),
                                    new SortedDictionarySpecification()),
                                new FilteringSpecimenBuilder(
                                    new Postprocessor(
                                        new MethodInvoker(
                                            new ModestConstructorQuery()),
                                        new DictionaryFiller()),
                                    new SortedListSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new EnumerableFavoringConstructorQuery()),
                                    new ObservableCollectionSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new ListFavoringConstructorQuery()),
                                    new CollectionSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new EnumerableFavoringConstructorQuery()),
                                    new HashSetSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new EnumerableFavoringConstructorQuery()),
                                    new SortedSetSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new EnumerableFavoringConstructorQuery()),
                                    new ListSpecification()),
                                new FilteringSpecimenBuilder(
                                    new MethodInvoker(
                                        new ModestConstructorQuery()),
                                    new NullableEnumRequestSpecification()),
                                new RangeAttributeRelay(),
                                new StringLengthAttributeRelay(),
                                new RegularExpressionAttributeRelay(),
                                new EnumGenerator(),
                                new LambdaExpressionGenerator(),
                                CreateDefaultValueBuilder(CultureInfo.InvariantCulture),
                                CreateDefaultValueBuilder(Encoding.UTF8),
                                CreateDefaultValueBuilder(IPAddress.Loopback))),
                        new Postprocessor(
                            new AutoPropertiesTarget(
                                new CompositeSpecimenBuilder(
                                    engine,
                                    multiple)),
                            new AutoPropertiesCommand(),
                            new AnyTypeSpecification()),
                        new ResidueCollectorNode(
                            new CompositeSpecimenBuilder(
                                new DictionaryRelay(),
                                new CollectionRelay(),
                                new ListRelay(),
                                new EnumerableRelay(),
                                new EnumeratorRelay())),
                        new FilteringSpecimenBuilder(
                            new MutableValueTypeWarningThrower(),
                            new AndRequestSpecification(
                                new ValueTypeSpecification(),
                                new NoConstructorsSpecification())))));

            this.UpdateCustomizer();
            this.UpdateResidueCollector();
            this.UpdateBehaviors(new ISpecimenBuilderTransformation[0]);

            this.behaviors.Add(new ThrowingRecursionBehavior());
        }

        /// <summary>
        /// Gets the behaviors that wrap around the rest of the graph.
        /// </summary>
        public IList<ISpecimenBuilderTransformation> Behaviors
        {
            get { return this.behaviors; }
        }

        /// <summary>
        /// Gets the customizations that intercept the <see cref="Engine"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any <see cref="ISpecimenBuilder"/> in this list are invoked before
        /// <see cref="Engine"/>, giving them a chance to intercept a request and resolve it before
        /// the Engine.
        /// </para>
        /// <para>
        /// <see cref="Customize{T}"/> places resulting customizations in this list.
        /// </para>
        /// </remarks>
        /// <seealso cref="Engine"/>
        /// <seealso cref="ResidueCollectors"/>
        public IList<ISpecimenBuilder> Customizations
        {
            get { return this.customizer; }
        }

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

        /// <summary>
        /// Gets or sets if writable properties should generally be assigned a value when 
        /// generating an anonymous object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is false.
        /// </para>
        /// </remarks>
        public bool OmitAutoProperties
        {
            get
            {
                return !this.graph.Parents(b => b is AutoPropertiesTarget).Any(n => n is Postprocessor);
            }
            set
            {
                if (value == this.OmitAutoProperties)
                    return;

                var g = this.graph;
                if (value)
                {
                    foreach (var p in this.graph.Parents(b => b is AutoPropertiesTarget))
                    {
                        foreach (var b in p)
                        {
                            var aptn = b as AutoPropertiesTarget;
                            if (aptn != null)
                                g = g.ReplaceNodes(with: aptn, when: p.Equals);
                        }
                    }
                }
                else
                {
                    foreach (var p in this.graph.Parents(b => b is AutoPropertiesTarget))
                    {
                        var pps = p
                            .Select(b => new Postprocessor(
                                b,
                                new AutoPropertiesCommand(),
                                new AnyTypeSpecification()))
                            .Cast<ISpecimenBuilder>();
                        g = g.ReplaceNodes(with: pps, when: p.Equals);
                    }
                }
                this.OnGraphChanged(this, new SpecimenBuilderNodeEventArgs(g));
            }
        }

        /// <summary>
        /// Gets or sets a number that controls how many objects are created when a
        /// <see cref="Fixture"/> creates more than one anonymous objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is 3.
        /// </para>
        /// </remarks>
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T})" />
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T}, Func{T})" />
        /// <seealso cref="FixtureRepeater.Repeat"/>
        public int RepeatCount
        {
            get { return this.multiple.Count; }
            set { this.multiple.Count = value; }
        }

        /// <summary>
        /// Gets the residue collectors that can be used to handle requests that neither the
        /// <see cref="Customizations"/> nor <see cref="Engine"/> could handle.
        /// </summary>
        /// <remarks>
        /// <para>
        /// These <see cref="ISpecimenBuilder"/> instances will be invoked if no previous builder
        /// could resolve a request. This gives you the opportunity to define fallback strategies
        /// to deal with unresolved requests.
        /// </para>
        /// </remarks>
        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return this.residueCollector; }
        }

        /// <summary>
        /// Customizes the creation algorithm for a single object, effectively turning off all
        /// Customizations on the <see cref="IFixture"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Build method kicks off a Fluent API which is usually completed by invoking
        /// <see cref="SpecimenFactory.Create{T}(IPostprocessComposer{T})"/> on the method
        /// chain.
        /// </para>
        /// <para>
        /// Note that the Build method chain is best understood as a one-off Customization. It
        /// bypasses all Customizations on the <see cref="Fixture"/> instance. Instead, it allows
        /// fine-grained control when building a specific specimen. However, in most cases, adding
        /// a convention-based <see cref="ICustomization"/> is a better, more flexible option.
        /// </para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public ICustomizationComposer<T> Build<T>()
        {
            var g = this.graph.ReplaceNodes(
                with: n => new CompositeSpecimenBuilder(
                    SpecimenBuilderNodeFactory.CreateComposer<T>().WithAutoProperties(this.EnableAutoProperties),
                    n),
                when: n => n is BehaviorRoot);

            return new CompositeNodeComposer<T>(g);
        }

        /// <summary>
        /// Applies a customization.
        /// </summary>
        /// <param name="customization">The customization to apply.</param>
        /// <returns>
        /// The current instance.
        /// </returns>
        public IFixture Customize(ICustomization customization)
        {
            if (customization == null)
            {
                throw new ArgumentNullException(nameof(customization));
            }

            customization.Customize(this);
            return this;
        }

        /// <summary>
        /// Customizes the creation algorithm for all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of object to customize.</typeparam>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        /// <remarks>
        /// <para>
        /// The resulting <see cref="ISpecimenBuilder"/> is added to <see cref="Customizations"/>.
        /// </para>
        /// </remarks>
        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (composerTransformation == null)
            {
                throw new ArgumentNullException(nameof(composerTransformation));
            }

            var c = composerTransformation(SpecimenBuilderNodeFactory.CreateComposer<T>().WithAutoProperties(this.EnableAutoProperties));
            this.customizer.Insert(0, c);
        }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// The requested specimen if possible; otherwise an exception is
        /// thrown.
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
            return this.graph.Create(request, context);
        }

        /// <summary>
        /// Gets an enumerator over the internal specimen builders used to 
        /// create objects.
        /// </summary>
        /// <returns>
        /// An enumerator over the internal specimen builders used to create
        /// objects.
        /// </returns>
        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            yield return this.graph;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <seealso cref="GetEnumerator()" />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private bool EnableAutoProperties
        {
            get { return !this.OmitAutoProperties; }
        }

        private void OnGraphChanged(object sender, SpecimenBuilderNodeEventArgs e)
        {
            var transformations = this.Behaviors.ToArray();

            this.graph = e.Graph;

            this.UpdateCustomizer();
            this.UpdateResidueCollector();
            this.UpdateBehaviors(transformations);
        }

        private void UpdateCustomizer()
        {
            this.customizer =
                new SpecimenBuilderNodeAdapterCollection(
                    this.graph,
                    n => n is CustomizationNode);
            this.customizer.GraphChanged += this.OnGraphChanged;
        }

        private void UpdateResidueCollector()
        {
            this.residueCollector =
                new SpecimenBuilderNodeAdapterCollection(
                    this.graph,
                    n => n is ResidueCollectorNode);
            this.residueCollector.GraphChanged += this.OnGraphChanged;
        }

        private void UpdateBehaviors(ISpecimenBuilderTransformation[] transformations)
        {
            this.behaviors = new SingletonSpecimenBuilderNodeStackAdapterCollection(this.graph, n => n is BehaviorRoot, transformations);
            this.behaviors.GraphChanged += this.OnGraphChanged;
        }

        private static ISpecimenBuilder CreateDefaultValueBuilder<T>(T value)
        {
            return new FilteringSpecimenBuilder(
                new FixedBuilder(value),
                new ExactTypeSpecification(typeof(T)));
        }
    }
}
