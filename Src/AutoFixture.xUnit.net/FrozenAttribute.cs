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
        private readonly Matching by;
        private readonly string targetName;
        private IRequestSpecification matcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute"/> class.
        /// </summary>
        /// <param name="by">
        /// The <see cref="Matching"/> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// The default value is <see cref="F:Matching.ExactType"/>.
        /// </param>
        /// <param name="targetName">
        /// The identifier used to determine which requests
        /// for a class member will be satisfied by the frozen parameter value.
        /// </param>
        public FrozenAttribute(
            Matching by = Matching.ExactType,
            string targetName = null)
        {
            this.by = by;
            this.targetName = targetName;
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
        /// Gets the identifier used to determine which requests
        /// for a class member will be satisfied by the frozen parameter value.
        /// </summary>
        public string TargetName
        {
            get { return targetName; }
        }

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

            var type = parameter.ParameterType;

            return ShouldMatchBySpecificType()
                ? FreezeAsType(type)
                : FreezeByCriteria(type);
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

        private ICustomization FreezeByCriteria(Type type)
        {
            MatchByType(type);
            MatchByName(type);
            return new FreezeOnMatchCustomization(type, this.matcher);
        }

        private void MatchByType(Type type)
        {
            AlwaysMatchByExactType(type);
            MatchByBaseType(type);
            MatchByImplementedInterfaces(type);
        }

        private void MatchByName(Type type)
        {
            MatchByPropertyName(type);
            MatchByParameterName(type);
            MatchByFieldName(type);
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

        private void MatchByParameterName(Type type)
        {
            if (ShouldMatchBy(Matching.ParameterName))
            {
                MatchBy(new ParameterSpecification(type, this.targetName));
            }
        }

        private void MatchByPropertyName(Type type)
        {
            if (ShouldMatchBy(Matching.PropertyName))
            {
                MatchBy(new PropertySpecification(type, this.targetName));
            }
        }

        private void MatchByFieldName(Type type)
        {
            if (ShouldMatchBy(Matching.FieldName))
            {
                MatchBy(new FieldSpecification(type, this.targetName));
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
