using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class NodeComparer : IEqualityComparer<ISpecimenBuilder>
    {
        private readonly IEqualityComparer<IRequestSpecification> specificationComparer;
        private readonly IEqualityComparer<IMethodQuery> queryComparer;

        public NodeComparer()
        {
            this.specificationComparer = new SpecificationComparer();
            this.queryComparer = new QueryComparer();
        }

        public bool Equals(ISpecimenBuilder x, ISpecimenBuilder y)
        {
            var fx = x as FilteringSpecimenBuilder;
            var fy = y as FilteringSpecimenBuilder;
            if (fx != null &&
                fy != null &&
                this.specificationComparer.Equals(fx.Specification, fy.Specification))
                return true;

            if (x is CompositeSpecimenBuilder && y is CompositeSpecimenBuilder)
                return true;

            var gx = x as NoSpecimenOutputGuard;
            var gy = y as NoSpecimenOutputGuard;
            if (gx != null &&
                gy != null &&
                this.specificationComparer.Equals(gx.Specification, gy.Specification))
                return true;

            var sirx = x as SeedIgnoringRelay;
            var siry = y as SeedIgnoringRelay;
            if (sirx != null && siry != null)
            {
                return true;
            }

            var mix = x as MethodInvoker;
            var miy = y as MethodInvoker;
            if (mix != null &&
                miy != null &&
                this.queryComparer.Equals(mix.Query, miy.Query))
                return true;

            var omx = x as Omitter;
            var omy = y as Omitter;
            if (omx != null &&
                omy != null &&
                this.specificationComparer.Equals(omx.Specification, omy.Specification))
                return true;

            var dx = x as DelegatingSpecimenBuilder;
            var dy = y as DelegatingSpecimenBuilder;
            if (dx != null &&
                dy != null &&
                object.Equals(dx.OnCreate, dy.OnCreate))
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
            private readonly IEqualityComparer<IEqualityComparer> comparerComparer;

            public SpecificationComparer()
            {
                this.comparerComparer = new ComparerComparer();
            }

            public bool Equals(IRequestSpecification x, IRequestSpecification y)
            {
                var invx = x as InverseRequestSpecification;
                var invy = y as InverseRequestSpecification;
                if (invx != null &&
                    invy != null &&
                    this.Equals(invx.Specification, invy.Specification))
                    return true;

                var ox = x as OrRequestSpecification;
                var oy = y as OrRequestSpecification;
                if (ox != null &&
                    oy != null &&
                    ox.Specifications.SequenceEqual(oy.Specifications, this))
                    return true;

                var sx = x as SeedRequestSpecification;
                var sy = y as SeedRequestSpecification;
                if (sx != null && sy != null)
                {
                    if (sx.TargetType == sy.TargetType)
                        return true;
                }

                var ex = x as ExactTypeSpecification;
                var ey = y as ExactTypeSpecification;
                if (ex != null && ey != null)
                {
                    if (ex.TargetType == ey.TargetType)
                        return true;
                }

                var tx = x as TrueRequestSpecification;
                var ty = x as TrueRequestSpecification;
                if (tx != null && ty != null)
                    return true;

                var eqx = x as EqualRequestSpecification;
                var eqy = y as EqualRequestSpecification;
                if (eqx != null &&
                    eqy != null &&
                    object.Equals(eqx.Target, eqy.Target) &&
                    this.comparerComparer.Equals(eqx.Comparer, eqy.Comparer))
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
                    var micx = x as MemberInfoEqualityComparer;
                    var micy = y as MemberInfoEqualityComparer;
                    if (micx != null &&
                        micy != null)
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
                var mqx = x as ModestConstructorQuery;
                var mqy = y as ModestConstructorQuery;
                if (mqx != null &&
                    mqy != null)
                    return true;

                return EqualityComparer<IMethodQuery>.Default.Equals(x, y);
            }

            public int GetHashCode(IMethodQuery obj)
            {
                return EqualityComparer<IMethodQuery>.Default.GetHashCode(obj);
            }
        }

        private static class GenericComparer
        {
            private static readonly Dictionary<Type, Type> equatables =
                new Dictionary<Type, Type>
                {
                    { typeof(SeededFactory<>), typeof(SeededFactoryEquatable<>) },
                    { typeof(SpecimenFactory<>), typeof(SpecimenFactoryEquatable<>) },
                    { typeof(SpecimenFactory<,>), typeof(SpecimenFactoryEquatable<,>) },
                    { typeof(SpecimenFactory<,,>), typeof(SpecimenFactoryEquatable<,,>) },
                    { typeof(SpecimenFactory<,,,>), typeof(SpecimenFactoryEquatable<,,,>) },
                    { typeof(SpecimenFactory<,,,,>), typeof(SpecimenFactoryEquatable<,,,,>) },
                    { typeof(Postprocessor<>), typeof(PostprocessorEquatable<>) },
                    { typeof(NodeComposer<>), typeof(NodeComposerEquatable<>) },
                    { typeof(CompositeNodeComposer<>), typeof(CompositeNodeComposerEquatable<>) }
                };

            public static object CreateFromTemplate(object obj)
            {
                var t = obj.GetType();
                if (!t.IsGenericType)
                    return new object();

                Type equatableType;
                if (equatables.TryGetValue(t.GetGenericTypeDefinition(),
                    out equatableType))
                {
                    var typeArguments = t.GetGenericArguments();
                    return equatableType.MakeGenericType(typeArguments)
                        .GetConstructors().Single()
                        .Invoke(new[] { obj });
                }

                return new object();
            }
        }

        private class PostprocessorEquatable<T> : GenericEquatable<Postprocessor<T>>
        {
            private readonly IEqualityComparer<IRequestSpecification> specificationComparer;

            public PostprocessorEquatable(Postprocessor<T> pp)
                : base(pp)
            {
                this.specificationComparer = new SpecificationComparer();
            }

            protected override bool EqualsInstance(Postprocessor<T> other)
            {
                var thisActionMethod = GetActionMethod(this.Item.Action);
                var otherActionMethod = GetActionMethod(other.Action);

                return this.Item.Action.Method.Equals(other.Action.Method)
                    && thisActionMethod.Equals(otherActionMethod)
                    && this.specificationComparer.Equals(this.Item.Specification, other.Specification);
            }

            /// <summary>
            /// Gets the underlying method implementing the target of an action.
            /// </summary>
            /// <param name="obj">The Target of an Action.</param>
            /// <returns>
            /// The underlying <see cref="MethodInfo" /> object.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The simple constructor of <see cref="Postprocessor{T}" />, which is the most
            /// commonly used constructor overload, wraps the injected <see cref="Action{T}" />
            /// into another <see cref="Action{T, ISpecimenContext}" />. Thus, the
            /// <see cref="Action{T, ISpecimenContext}.Method" /> property is always the same. What
            /// really matters is the Target of the Action. However, the immediate Target is alway
            /// the closure belonging to the Postprocessor instance itself. This closure is really
            /// a compiler-generated class with a single public field called "action". When the
            /// action wraps another action, that field is itself an Action with a Method
            /// parameter, which is what is being returned from this method.
            /// </para>
            /// <para>
            /// However, if there's no "action" field then this is taken as an indication that
            /// another constructor overload of PostProcesser{T} was used - one which doesn't wrap
            /// one action in another. In this case, the fallback is the method itself.
            /// </para>
            /// </remarks>
            private static object GetActionMethod(Delegate del)
            {
                var obj = del.Target;
                var actionField = obj.GetType().GetField("action");
                if (actionField == null)
                    return del.Method;
                var action = actionField.GetValue(obj);
                var methodProperty = action.GetType().GetProperty("Method");
                return methodProperty.GetValue(action, null);
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
                return this.Item.Factory.Equals(other.Factory);
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
                return this.Item.Factory.Equals(other.Factory);
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
                return this.Item.Factory.Equals(other.Factory);
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
                return this.Item.Factory.Equals(other.Factory);
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
                return this.Item.Factory.Equals(other.Factory);
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
                return this.Item.Factory.Equals(other.Factory);
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

        private abstract class GenericEquatable<T> : IEquatable<T> where T : class
        {
            private readonly T item;

            public GenericEquatable(T item)
            {
                this.item = item;
            }

            public T Item
            {
                get { return this.item; }
            }

            public override bool Equals(object obj)
            {
                var other = obj as T;
                if (other != null)
                    return this.Equals(other);
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.item.GetHashCode();
            }

            public bool Equals(T other)
            {
                if (other == null)
                    return false;

                return this.EqualsInstance(other);
            }

            protected abstract bool EqualsInstance(T other);
        }
    }
}
