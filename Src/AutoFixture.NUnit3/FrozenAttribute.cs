using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// TestCase to indicate that the parameter value should be frozen so that the same instance is
    /// returned every time the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FrozenAttribute : CustomizeAttribute
    {
        private readonly Matching by;

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

            return FreezeByCriteria(parameter);
        }
        
        private ICustomization FreezeByCriteria(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            var name = parameter.Name;

            var filter = new Filter(ByEqual(parameter))
                .Or(ByExactType(type))
                .Or(ByBaseType(type))
                .Or(ByImplementedInterfaces(type))
                .Or(ByPropertyName(type, name))
                .Or(ByParameterName(type, name))
                .Or(ByFieldName(type, name));

            return new FreezeOnMatchCustomization(type, filter);
        }

        private static IRequestSpecification ByEqual(object target)
        {
            return new EqualRequestSpecification(target);
        }

        private IRequestSpecification ByExactType(Type type)
        {
            return ShouldMatchBy(Matching.ExactType)
                ? new OrRequestSpecification(
                    new ExactTypeSpecification(type),
                    new SeedRequestSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByBaseType(Type type)
        {
            return ShouldMatchBy(Matching.DirectBaseType)
                ? new AndRequestSpecification(
                    new InverseRequestSpecification(
                        new ExactTypeSpecification(type)),
                    new DirectBaseTypeSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByImplementedInterfaces(Type type)
        {
            return ShouldMatchBy(Matching.ImplementedInterfaces)
                ? new AndRequestSpecification(
                    new InverseRequestSpecification(
                        new ExactTypeSpecification(type)),
                    new ImplementedInterfaceSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByParameterName(Type type, string name)
        {
            return ShouldMatchBy(Matching.ParameterName)
                ? new ParameterSpecification(
                    new ParameterTypeAndNameCriterion(
                        DerivesFrom(type),
                        IsNamed(name)))
                : NoMatch();
        }

        private IRequestSpecification ByPropertyName(Type type, string name)
        {
            return ShouldMatchBy(Matching.PropertyName)
                ? new PropertySpecification(
                    new PropertyTypeAndNameCriterion(
                        DerivesFrom(type),
                        IsNamed(name)))
                : NoMatch();
        }

        private IRequestSpecification ByFieldName(Type type, string name)
        {
            return ShouldMatchBy(Matching.FieldName)
                ? new FieldSpecification(
                    new FieldTypeAndNameCriterion(
                        DerivesFrom(type),
                        IsNamed(name)))
                : NoMatch();
        }

        private bool ShouldMatchBy(Matching criteria)
        {
            return this.By.HasFlag(criteria);
        }

        private static IRequestSpecification NoMatch()
        {
            return new FalseRequestSpecification();
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

        private class Filter : IRequestSpecification
        {
            private readonly IRequestSpecification criteria;

            public Filter(IRequestSpecification criteria)
            {
                this.criteria = criteria;
            }

            public Filter Or(IRequestSpecification condition)
            {
                return new Filter(
                    new OrRequestSpecification(
                        this.criteria,
                        condition));
            }

            public bool IsSatisfiedBy(object request)
            {
                return this.criteria.IsSatisfiedBy(request);
            }
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
