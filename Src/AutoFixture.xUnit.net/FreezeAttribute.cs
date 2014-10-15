using System;
using System.Reflection;
using Ploeh.AutoFixture.Dsl;

namespace Ploeh.AutoFixture.Xunit
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FreezeAttribute : CustomizeAttribute
    {
        private Type parameterType;
        private string parameterName;

        public FreezeAttribute()
        {
            this.By = Matching.ExactType;
        }

        public Matching By { get; set; }

        public string TargetName { get; set; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            this.parameterType = parameter.ParameterType;
            this.parameterName = parameter.Name;

            return FreezeTypeWithMatchingCriteria();
        }

        private ICustomization FreezeTypeWithMatchingCriteria()
        {
            return (ICustomization)Activator.CreateInstance(
                typeof(FreezeOnMatchCustomization<>).MakeGenericType(this.parameterType),
                BuildMatchingCriteria());
        }

        private Func<IMatchComposer, IMatchComposer> BuildMatchingCriteria()
        {
            return match =>
            {
                match = MatchDecoratedParameter(match);

                if (ShouldMatchBy(Matching.ExactType))
                {
                    match = match.ByExactType(this.parameterType);
                }

                if (ShouldMatchBy(Matching.BaseType))
                {
                    match = match.Or.ByBaseType(this.parameterType);
                }

                if (ShouldMatchBy(Matching.ImplementedInterfaces))
                {
                    match = match.Or.ByInterfaces(this.parameterType);
                }

                if (ShouldMatchBy(Matching.ParameterName))
                {
                    match = match.Or.ByParameterName(this.parameterType, this.TargetName);
                }

                if (ShouldMatchBy(Matching.PropertyName))
                {
                    match = match.Or.ByPropertyName(this.parameterType, this.TargetName);
                }

                if (ShouldMatchBy(Matching.FieldName))
                {
                    match = match.Or.ByFieldName(this.parameterType, this.TargetName);
                }

                return match;
            };
        }

        private IMatchComposer MatchDecoratedParameter(IMatchComposer match)
        {
            match = match.ByParameterName(this.parameterType, this.parameterName);
            return match;
        }

        private bool ShouldMatchBy(Matching criteria)
        {
            return this.By.HasFlag(criteria);
        }
    }
}
