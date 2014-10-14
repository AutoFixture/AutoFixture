using System;
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
        private IRequestSpecification matcher;
        private Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute"/> class.
        /// </summary>
        public FrozenAttribute()
        {
            this.By = Matching.ExactType;
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> that the frozen parameter value
        /// should be mapped to in the <see cref="IFixture"/>.
        /// </summary>
        public Type As { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </summary>
        /// <remarks>
        /// If not specified, requests will be matched by exact type.
        /// </remarks>
        public Matching By { get; set; }

        /// <summary>
        /// Gets or sets the identifier used to determine which requests
        /// for a class member will be satisfied by the frozen parameter value.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Gets a <see cref="FreezeOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> of the parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter for which the customization is requested.
        /// </param>
        /// <returns>
        /// A <see cref="FreezeOnMatchCustomization"/> configured
        /// to match requests based on the <see cref="Type"/> of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            this.targetType = parameter.ParameterType;

            return ShouldMatchBySpecificType()
                ? FreezeAsType()
                : FreezeByCriteria();
        }

        private bool ShouldMatchBySpecificType()
        {
            return this.As != null;
        }

        private ICustomization FreezeAsType()
        {
            return new FreezingCustomization(
                this.targetType,
                this.As ?? targetType);
        }

        private ICustomization FreezeByCriteria()
        {
            MatchByType();
            MatchByName();
            return new FreezeOnMatchCustomization(
                this.targetType,
                this.matcher);
        }

        private void MatchByType()
        {
            AlwaysMatchByExactType();
            MatchByBaseType();
            MatchByImplementedInterfaces();
        }

        private void MatchByName()
        {
            MatchByPropertyName();
            MatchByParameterName();
            MatchByFieldName();
        }

        private void AlwaysMatchByExactType()
        {
            MatchBy(
                new OrRequestSpecification(
                    new ExactTypeSpecification(targetType),
                    new SeedRequestSpecification(targetType)));
        }

        private void MatchByBaseType()
        {
            if (ShouldMatchBy(Matching.DirectBaseType))
            {
                MatchBy(new DirectBaseTypeSpecification(this.targetType));
            }
        }

        private void MatchByImplementedInterfaces()
        {
            if (ShouldMatchBy(Matching.ImplementedInterfaces))
            {
                MatchBy(new ImplementedInterfaceSpecification(this.targetType));
            }
        }

        private void MatchByParameterName()
        {
            if (ShouldMatchBy(Matching.ParameterName))
            {
                MatchBy(
                    new ParameterSpecification(
                        this.targetType,
                        this.TargetName));
            }
        }

        private void MatchByPropertyName()
        {
            if (ShouldMatchBy(Matching.PropertyName))
            {
                MatchBy(
                    new PropertySpecification(
                        this.targetType,
                        this.TargetName));
            }
        }

        private void MatchByFieldName()
        {
            if (ShouldMatchBy(Matching.FieldName))
            {
                MatchBy(
                    new FieldSpecification(
                        this.targetType,
                        this.TargetName));
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
    }
}
