using System;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// Theory to indicate that the parameter value should be frozen so that the same instance is
    /// returned every time the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FrozenAttribute : CustomizeAttribute
    {
        private readonly Matching by;
        private IRequestSpecification matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value
        /// is <see cref="F:Matching.ExactType"/>.
        /// </remarks>
        public FrozenAttribute()
            : this(Matching.ExactType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute"/> class.
        /// </summary>
        /// <param name="by">
        /// The <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </param>
        public FrozenAttribute(Matching by)
        {
            this.by = by;
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> that the frozen parameter value
        /// should be mapped to in the <see cref="IFixture"/>.
        /// </summary>
        [Obsolete("The ability to map a frozen object to a specific type is deprecated and will likely be removed in the future. If you wish to map a frozen type to its implemented interfaces, use [Frozen(By = Matching.ImplementedInterfaces)].")]
        public Type As { get; set; }

        /// <summary>
        /// Gets the <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </summary>
        public Matching By
        {
            get { return by; }
        }

        /// <summary>
        /// Gets a <see cref="FreezeOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> and optionally
        /// the name of the parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter for which the customization is requested.
        /// </param>
        /// <returns>
        /// A <see cref="FreezeOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> and optionally
        /// the name of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            var type = parameter.ParameterType;
            var name = parameter.Name;

            return ShouldMatchBySpecificType()
                ? FreezeAsType(type)
                : FreezeByCriteria(type, name);
        }

        private bool ShouldMatchBySpecificType()
        {
#pragma warning disable 0618
            return this.As != null;
#pragma warning restore 0618
        }

        private ICustomization FreezeAsType(Type type)
        {
            return new FreezingCustomization(
                type,
#pragma warning disable 0618
                this.As ?? type);
#pragma warning restore 0618
        }

        private ICustomization FreezeByCriteria(Type type, string name)
        {
            MatchByType(type);
            MatchByName(type, name);
            return new FreezeOnMatchCustomization(type, this.matcher);
        }

        private void MatchByType(Type type)
        {
            AlwaysMatchByExactType(type);
            MatchByBaseType(type);
            MatchByImplementedInterfaces(type);
        }

        private void MatchByName(Type type, string name)
        {
            MatchByPropertyName(type, name);
            MatchByParameterName(type, name);
            MatchByFieldName(type, name);
        }

        private void AlwaysMatchByExactType(Type type)
        {
            MatchBy(
                new OrRequestSpecification(
                    new ExactTypeSpecification(type),
                    new SeedRequestSpecification(type)));
        }

        private void MatchByBaseType(Type type)
        {
            if (ShouldMatchBy(Matching.DirectBaseType))
            {
                MatchBy(new DirectBaseTypeSpecification(type));
            }
        }

        private void MatchByImplementedInterfaces(Type type)
        {
            if (ShouldMatchBy(Matching.ImplementedInterfaces))
            {
                MatchBy(new ImplementedInterfaceSpecification(type));
            }
        }

        private void MatchByParameterName(Type type, string name)
        {
            if (ShouldMatchBy(Matching.ParameterName))
            {
                MatchBy(
                    new ParameterSpecification(
                        new ParameterTypeAndNameCriterion(
                            DerivesFrom(type),
                            IsNamed(name))));
            }
        }

        private void MatchByPropertyName(Type type, string name)
        {
            if (ShouldMatchBy(Matching.PropertyName))
            {
                MatchBy(
                    new PropertySpecification(
                        new PropertyTypeAndNameCriterion(
                            DerivesFrom(type),
                            IsNamed(name))));
            }
        }

        private void MatchByFieldName(Type type, string name)
        {
            if (ShouldMatchBy(Matching.FieldName))
            {
                MatchBy(
                    new FieldSpecification(
                        new FieldTypeAndNameCriterion(
                            DerivesFrom(type),
                            IsNamed(name))));
            }
        }

        private bool ShouldMatchBy(Matching criteria)
        {
            return this.By.HasFlag(criteria);
        }

        private void MatchBy(IRequestSpecification criteria)
        {
            this.matcher = this.matcher == null
                ? criteria
                : new OrRequestSpecification(this.matcher, criteria);
        }

        private static Criterion<Type> DerivesFrom(Type type)
        {
            return new Criterion<Type>(
                type,
                new DerivesFromTypeComparer());
        }

        private static Criterion<string> IsNamed(string name)
        {
            return new Criterion<string>(
                name,
                StringComparer.OrdinalIgnoreCase);
        }

        private class DerivesFromTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                if (y == null && x == null)
                    return true;
                if (y == null)
                    return false;
                return y.IsAssignableFrom(x);
            }

            public int GetHashCode(Type obj)
            {
                return 0;
            }
        }
    }
}
