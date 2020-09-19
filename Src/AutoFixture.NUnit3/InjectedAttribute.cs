﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// TestCase to indicate that the parameter value should be injected so that the same instance is
    /// returned every time the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class InjectedAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectedAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// <param name="valueToInject">The value to inject.</param>
        /// The <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the injected parameter value
        /// is <see cref="Matching.ExactType"/>.
        /// </remarks>
        public InjectedAttribute(object valueToInject)
            : this(valueToInject, Matching.ExactType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectedAttribute"/> class.
        /// </summary>
        /// <param name="valueToInject">The value to inject.</param>
        /// <param name="by">
        /// The <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the injected parameter value.
        /// </param>
        public InjectedAttribute(object valueToInject, Matching by)
        {
            this.ValueToInject = valueToInject;
            this.By = by;
        }

        /// <summary>
        /// The value to inject.
        /// </summary>
        public object ValueToInject { get; }

        /// <summary>
        /// Gets the <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </summary>
        public Matching By { get; }

        /// <summary>
        /// Gets a <see cref="InjectOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> and optionally
        /// the name of the parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter for which the customization is requested.
        /// </param>
        /// <returns>
        /// A <see cref="InjectOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> and optionally
        /// the name of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            return this.InjectByCriteria(parameter);
        }

        private ICustomization InjectByCriteria(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            var name = parameter.Name;

            var filter = new Filter(ByEqual(parameter))
                         .Or(this.ByExactType(type))
                         .Or(this.ByBaseType(type))
                         .Or(this.ByImplementedInterfaces(type))
                         .Or(this.ByPropertyName(type, name))
                         .Or(this.ByParameterName(type, name))
                         .Or(this.ByFieldName(type, name));

            return new InjectOnMatchCustomization(this.ValueToInject, parameter, filter);
        }

        private static IRequestSpecification ByEqual(object target)
        {
            return new EqualRequestSpecification(target);
        }

        private IRequestSpecification ByExactType(Type type)
        {
            return this.ShouldMatchBy(Matching.ExactType)
                ? new OrRequestSpecification(
                    new ExactTypeSpecification(type),
                    new SeedRequestSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByBaseType(Type type)
        {
            return this.ShouldMatchBy(Matching.DirectBaseType)
                ? new AndRequestSpecification(
                    new InverseRequestSpecification(
                        new ExactTypeSpecification(type)),
                    new DirectBaseTypeSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByImplementedInterfaces(Type type)
        {
            return this.ShouldMatchBy(Matching.ImplementedInterfaces)
                ? new AndRequestSpecification(
                    new InverseRequestSpecification(
                        new ExactTypeSpecification(type)),
                    new ImplementedInterfaceSpecification(type))
                : NoMatch();
        }

        private IRequestSpecification ByParameterName(Type type, string name)
        {
            return this.ShouldMatchBy(Matching.ParameterName)
                ? new ParameterSpecification(
                    new ParameterTypeAndNameCriterion(
                        DerivesFrom(type),
                        IsNamed(name)))
                : NoMatch();
        }

        private IRequestSpecification ByPropertyName(Type type, string name)
        {
            return this.ShouldMatchBy(Matching.PropertyName)
                ? new PropertySpecification(
                    new PropertyTypeAndNameCriterion(
                        DerivesFrom(type),
                        IsNamed(name)))
                : NoMatch();
        }

        private IRequestSpecification ByFieldName(Type type, string name)
        {
            return this.ShouldMatchBy(Matching.FieldName)
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
                return y.GetTypeInfo().IsAssignableFrom(x);
            }

            public int GetHashCode(Type obj)
            {
                return 0;
            }
        }
    }
}