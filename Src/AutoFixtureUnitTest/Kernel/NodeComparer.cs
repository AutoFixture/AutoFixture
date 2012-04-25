using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                return EqualityComparer<IRequestSpecification>.Default.Equals(x, y);
            }

            public int GetHashCode(IRequestSpecification obj)
            {
                return EqualityComparer<IRequestSpecification>.Default.GetHashCode(obj);
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
                    { typeof(SpecimenFactory<,,>), typeof(SpecimenFactoryEquatable<,,>) }
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
