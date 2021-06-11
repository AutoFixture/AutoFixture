using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit2
{
    internal class ParameterMatcherBuilder
    {
        private readonly ParameterInfo parameterInfo;

        public ParameterMatcherBuilder(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
        }

        public bool MatchExactRequest { get; set; } = true;

        public bool MatchExactType { get; set; }

        public bool MatchDirectBaseType { get; set; }

        public bool MatchImplementedInterfaces { get; set; }

        public bool MatchParameter { get; set; }

        public bool MatchProperty { get; set; }

        public bool MatchField { get; set; }

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