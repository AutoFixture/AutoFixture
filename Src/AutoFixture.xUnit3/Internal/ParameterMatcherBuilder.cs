using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    /// A builder type that creates a <see cref="IRequestSpecification"/> instance,
    /// for a <see cref="ParameterInfo"/> instance, based on the builder's matching configuration.
    /// </summary>
    public class ParameterMatcherBuilder
    {
        private readonly ParameterInfo parameterInfo;

        /// <summary>
        /// Creates an instance of type <see cref="ParameterMatcherBuilder"/>.
        /// </summary>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="parameterInfo"/> is <see langword="null"/>.
        /// </exception>
        public ParameterMatcherBuilder(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo
                ?? throw new ArgumentNullException(nameof(parameterInfo));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the exact parameter request should be matched.
        /// Default is <see langword="true"/>.
        /// </summary>
        public bool MatchExactRequest { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the exact parameter type should be matched.
        /// </summary>
        public bool MatchExactType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the direct base type should be matched.
        /// </summary>
        public bool MatchDirectBaseType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interfaces implemented
        /// by the parameter type should be matched.
        /// </summary>
        public bool MatchImplementedInterfaces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter type and name should be matched.
        /// The name comparison is case-insensitive.
        /// </summary>
        public bool MatchParameter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property type and name should be matched.
        /// The name comparison is case-insensitive.
        /// </summary>
        public bool MatchProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field type and name should be matched.
        /// The name comparison is case-insensitive.
        /// </summary>
        public bool MatchField { get; set; }

        /// <summary>
        /// Sets the matching flags.
        /// </summary>
        /// <param name="flags">The matching flags.</param>
        /// <returns>The current <see cref="ParameterMatcherBuilder"/> instance.</returns>
        public ParameterMatcherBuilder SetFlags(Matching flags)
        {
            this.MatchExactType = flags.HasFlag(Matching.ExactType);
            this.MatchDirectBaseType = flags.HasFlag(Matching.DirectBaseType);
            this.MatchImplementedInterfaces = flags.HasFlag(Matching.ImplementedInterfaces);
            this.MatchParameter = flags.HasFlag(Matching.ParameterName);
            this.MatchProperty = flags.HasFlag(Matching.PropertyName);
            this.MatchField = flags.HasFlag(Matching.FieldName);
            return this;
        }

        /// <summary>
        /// Builds the <see cref="IRequestSpecification"/> instance.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="IRequestSpecification"/>, matching the configuration.
        /// </returns>
        public IRequestSpecification Build()
        {
            var specifications = new List<IRequestSpecification>(7);
            if (this.MatchExactRequest)
                specifications.Add(this.AsExactRequest());

            if (this.MatchExactType)
                specifications.Add(this.AsExactType());

            if (this.MatchDirectBaseType)
                specifications.Add(this.AsDirectBaseType());

            if (this.MatchImplementedInterfaces)
                specifications.Add(this.AsImplementedInterfaces());

            if (this.MatchProperty)
                specifications.Add(this.AsProperty());

            if (this.MatchParameter)
                specifications.Add(this.AsParameter());

            if (this.MatchField)
                specifications.Add(this.AsField());

            return specifications.Count == 1
                ? specifications[0]
                : new OrRequestSpecification(specifications);
        }

        private IRequestSpecification AsExactRequest()
        {
            return new EqualRequestSpecification(this.parameterInfo);
        }

        private IRequestSpecification AsExactType()
        {
            return new OrRequestSpecification(
                new ExactTypeSpecification(this.parameterInfo.ParameterType),
                new SeedRequestSpecification(this.parameterInfo.ParameterType));
        }

        private IRequestSpecification AsDirectBaseType()
        {
            return new AndRequestSpecification(
                new InverseRequestSpecification(
                    new ExactTypeSpecification(this.parameterInfo.ParameterType)),
                new DirectBaseTypeSpecification(this.parameterInfo.ParameterType));
        }

        private IRequestSpecification AsImplementedInterfaces()
        {
            return new AndRequestSpecification(
                new InverseRequestSpecification(
                    new ExactTypeSpecification(this.parameterInfo.ParameterType)),
                new ImplementedInterfaceSpecification(this.parameterInfo.ParameterType));
        }

        private IRequestSpecification AsParameter()
        {
            return new ParameterSpecification(
                new ParameterTypeAndNameCriterion(
                    new Criterion<Type>(this.parameterInfo.ParameterType, new DerivesFromTypeComparer()),
                    new Criterion<string>(this.parameterInfo.Name, StringComparer.OrdinalIgnoreCase)));
        }

        private IRequestSpecification AsProperty()
        {
            return new PropertySpecification(
                new PropertyTypeAndNameCriterion(
                    new Criterion<Type>(this.parameterInfo.ParameterType, new DerivesFromTypeComparer()),
                    new Criterion<string>(this.parameterInfo.Name, StringComparer.OrdinalIgnoreCase)));
        }

        private IRequestSpecification AsField()
        {
            return new FieldSpecification(
                new FieldTypeAndNameCriterion(
                    new Criterion<Type>(this.parameterInfo.ParameterType, new DerivesFromTypeComparer()),
                    new Criterion<string>(this.parameterInfo.Name, StringComparer.OrdinalIgnoreCase)));
        }

        private class DerivesFromTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return y switch
                {
                    null when x is null => true,
                    null => false,
                    _ => y.GetTypeInfo().IsAssignableFrom(x)
                };
            }

            public int GetHashCode(Type obj) => 0;
        }
    }
}