using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace AutoFixtureUnitTest.Kernel;

internal class NodeComparer : IEqualityComparer<ISpecimenBuilder>
{
    private readonly IEqualityComparer<IRequestSpecification> _specificationComparer;
    private readonly IEqualityComparer<IMethodQuery> _queryComparer;
    private readonly IEqualityComparer<ISpecimenCommand> _commandComparer;

    public NodeComparer()
    {
        _specificationComparer = new SpecificationComparer();
        _queryComparer = new QueryComparer();
        _commandComparer = new CommandComparer(_specificationComparer);
    }

    public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
    {
        if (x is FilteringSpecimenBuilder fx &&
            y is FilteringSpecimenBuilder fy &&
            _specificationComparer.Equals(fx.Specification, fy.Specification))
            return true;

        if (x is CompositeSpecimenBuilder && y is CompositeSpecimenBuilder)
            return true;

        if (x is NoSpecimenOutputGuard gx &&
            y is NoSpecimenOutputGuard gy &&
            _specificationComparer.Equals(gx.Specification, gy.Specification))
            return true;

        if (x is SeedIgnoringRelay && y is SeedIgnoringRelay)
        {
            return true;
        }

        if (x is MethodInvoker mix &&
            y is MethodInvoker miy &&
            _queryComparer.Equals(mix.Query, miy.Query))
            return true;

        if (x is Omitter omx &&
            y is Omitter omy &&
            _specificationComparer.Equals(omx.Specification, omy.Specification))
            return true;

        if (x is DelegatingSpecimenBuilder dx &&
            y is DelegatingSpecimenBuilder dy &&
            object.Equals(dx.OnCreate, dy.OnCreate))
            return true;

        if (x is Postprocessor px &&
            y is Postprocessor py &&
            _commandComparer.Equals(px.Command, py.Command) &&
            _specificationComparer.Equals(px.Specification, py.Specification))
            return true;

        if (GenericComparer.CreateFromTemplate(x).Equals(y))
            return true;

        return EqualityComparer<ISpecimenBuilder>.Default.Equals(x, y);
    }

    public int GetHashCode(ISpecimenBuilder obj)
    {
        return EqualityComparer<ISpecimenBuilder>.Default.GetHashCode(obj);
    }

    private class SpecificationComparer : IEqualityComparer<IRequestSpecification>
    {
        private readonly IEqualityComparer<IEqualityComparer> _comparerComparer;

        public SpecificationComparer()
        {
            _comparerComparer = new ComparerComparer();
        }

        public bool Equals(IRequestSpecification x, IRequestSpecification y)
        {
            if (x is InverseRequestSpecification invx &&
                y is InverseRequestSpecification invy &&
                Equals(invx.Specification, invy.Specification))
                return true;

            if (x is OrRequestSpecification ox &&
                y is OrRequestSpecification oy &&
                ox.Specifications.SequenceEqual(oy.Specifications, this))
                return true;

            if (x is AndRequestSpecification ax &&
                y is AndRequestSpecification ay &&
                ax.Specifications.SequenceEqual(ay.Specifications, this))
                return true;

            if (x is SeedRequestSpecification sx &&
                y is SeedRequestSpecification sy &&
                sx.TargetType == sy.TargetType)
                return true;

            if (x is ExactTypeSpecification ex &&
                y is ExactTypeSpecification ey &&
                ex.TargetType == ey.TargetType)
                return true;

            if (x is TrueRequestSpecification && y is TrueRequestSpecification)
                return true;

            if (x is FalseRequestSpecification && y is FalseRequestSpecification)
                return true;

            if (x is EqualRequestSpecification eqx &&
                y is EqualRequestSpecification eqy &&
                object.Equals(eqx.Target, eqy.Target) &&
                _comparerComparer.Equals(eqx.Comparer, eqy.Comparer))
                return true;

            return EqualityComparer<IRequestSpecification>.Default.Equals(x, y);
        }

        public int GetHashCode(IRequestSpecification obj)
        {
            return EqualityComparer<IRequestSpecification>.Default.GetHashCode(obj);
        }

        private class ComparerComparer : IEqualityComparer<IEqualityComparer>
        {
            public bool Equals(IEqualityComparer x, IEqualityComparer y)
            {
                if (x is MemberInfoEqualityComparer &&
                    y is MemberInfoEqualityComparer)
                    return true;

                return EqualityComparer<IEqualityComparer>.Default.Equals(x, y);
            }

            public int GetHashCode(IEqualityComparer obj)
            {
                return 0;
            }
        }
    }

    private class QueryComparer : IEqualityComparer<IMethodQuery>
    {
        public bool Equals(IMethodQuery x, IMethodQuery y)
        {
            if (x is ModestConstructorQuery &&
                y is ModestConstructorQuery)
                return true;

            return EqualityComparer<IMethodQuery>.Default.Equals(x, y);
        }

        public int GetHashCode(IMethodQuery obj)
        {
            return EqualityComparer<IMethodQuery>.Default.GetHashCode(obj);
        }
    }

    private class CommandComparer : IEqualityComparer<ISpecimenCommand>
    {
        private readonly IEqualityComparer<IRequestSpecification> _specificationComparer;

        public CommandComparer(IEqualityComparer<IRequestSpecification> specificationComparer)
        {
            _specificationComparer = specificationComparer;
        }

        public bool Equals(ISpecimenCommand x, ISpecimenCommand y)
        {
            if (x is AutoPropertiesCommand apx &&
                y is AutoPropertiesCommand apy)
            {
                return
                    _specificationComparer.Equals(apx.Specification, apy.Specification) &&
                    apx.ExplicitSpecimenType == apy.ExplicitSpecimenType;
            }

            return x.GetType() == y.GetType();
        }

        public int GetHashCode(ISpecimenCommand obj)
        {
            return EqualityComparer<ISpecimenCommand>.Default.GetHashCode(obj);
        }
    }

    private static class GenericComparer
    {
        private static readonly Dictionary<Type, Type> s_equatables =
            new Dictionary<Type, Type>
            {
                { typeof(SeededFactory<>), typeof(SeededFactoryEquatable<>) },
                { typeof(SpecimenFactory<>), typeof(SpecimenFactoryEquatable<>) },
                { typeof(SpecimenFactory<,>), typeof(SpecimenFactoryEquatable<,>) },
                { typeof(SpecimenFactory<,,>), typeof(SpecimenFactoryEquatable<,,>) },
                { typeof(SpecimenFactory<,,,>), typeof(SpecimenFactoryEquatable<,,,>) },
                { typeof(SpecimenFactory<,,,,>), typeof(SpecimenFactoryEquatable<,,,,>) },
                { typeof(NodeComposer<>), typeof(NodeComposerEquatable<>) },
                { typeof(CompositeNodeComposer<>), typeof(CompositeNodeComposerEquatable<>) },
                { typeof(BindingCommand<,>), typeof(BindingCommandEquatable<,>) }
            };

        public static object CreateFromTemplate(object obj)
        {
            var t = obj.GetType();
            if (!t.IsGenericType)
                return new object();

            if (s_equatables.TryGetValue(t.GetGenericTypeDefinition(), out Type equatableType))
            {
                var typeArguments = t.GetGenericArguments();
                return equatableType.MakeGenericType(typeArguments)
                    .GetConstructors().Single()
                    .Invoke(new[] { obj });
            }

            return new object();
        }
    }

    private class SeededFactoryEquatable<T> : GenericEquatable<SeededFactory<T>>
    {
        public SeededFactoryEquatable(SeededFactory<T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SeededFactory<T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class SpecimenFactoryEquatable<T> : GenericEquatable<SpecimenFactory<T>>
    {
        public SpecimenFactoryEquatable(SpecimenFactory<T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SpecimenFactory<T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class SpecimenFactoryEquatable<TInput, T> : GenericEquatable<SpecimenFactory<TInput, T>>
    {
        public SpecimenFactoryEquatable(SpecimenFactory<TInput, T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SpecimenFactory<TInput, T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class SpecimenFactoryEquatable<TInput1, TInput2, T> : GenericEquatable<SpecimenFactory<TInput1, TInput2, T>>
    {
        public SpecimenFactoryEquatable(SpecimenFactory<TInput1, TInput2, T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SpecimenFactory<TInput1, TInput2, T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class SpecimenFactoryEquatable<TInput1, TInput2, TInput3, T> : GenericEquatable<SpecimenFactory<TInput1, TInput2, TInput3, T>>
    {
        public SpecimenFactoryEquatable(SpecimenFactory<TInput1, TInput2, TInput3, T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SpecimenFactory<TInput1, TInput2, TInput3, T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class SpecimenFactoryEquatable<TInput1, TInput2, TInput3, TInput4, T> : GenericEquatable<SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T>>
    {
        public SpecimenFactoryEquatable(SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T> sf)
            : base(sf)
        {
        }

        protected override bool EqualsInstance(SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T> other)
        {
            return Item.Factory.Equals(other.Factory);
        }
    }

    private class NodeComposerEquatable<T> : GenericEquatable<NodeComposer<T>>
    {
        public NodeComposerEquatable(NodeComposer<T> item)
            : base(item)
        {
        }

        protected override bool EqualsInstance(NodeComposer<T> other)
        {
            return true;
        }
    }

    private class CompositeNodeComposerEquatable<T> : GenericEquatable<CompositeNodeComposer<T>>
    {
        public CompositeNodeComposerEquatable(CompositeNodeComposer<T> item)
            : base(item)
        {
        }

        protected override bool EqualsInstance(CompositeNodeComposer<T> other)
        {
            return true;
        }
    }

    private class BindingCommandEquatable<T, TProperty> : GenericEquatable<BindingCommand<T, TProperty>>
    {
        public BindingCommandEquatable(BindingCommand<T, TProperty> item)
            : base(item)
        {
        }

        protected override bool EqualsInstance(BindingCommand<T, TProperty> other)
        {
            return Item.Member == other.Member;
        }
    }

    private abstract class GenericEquatable<T> : IEquatable<T>
        where T : class
    {
        public GenericEquatable(T item)
        {
            Item = item;
        }

        public T Item { get; }

        public override bool Equals(object obj)
        {
            if (obj is T other)
                return Equals(other);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

        public bool Equals(T other)
        {
            if (other == null)
                return false;

            return EqualsInstance(other);
        }

        protected abstract bool EqualsInstance(T other);
    }
}