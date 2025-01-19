using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture.Xunit3.Internal;

namespace AutoFixture.Xunit3
{
    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute" />-driven
    /// Theory to indicate that the parameter value should be frozen so that the same instance is
    /// returned every time the <see cref="IFixture" /> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes",
        Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class FrozenAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="Matching" /> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value
        /// is <see cref="Matching.ExactType" />.
        /// </remarks>
        public FrozenAttribute()
            : this(Matching.ExactType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrozenAttribute" /> class.
        /// </summary>
        /// <param name="by">
        /// The <see cref="Matching" /> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </param>
        public FrozenAttribute(Matching by)
        {
            this.By = by;
        }

        /// <summary>
        /// Gets the <see cref="Matching" /> criteria used to determine
        /// which requests will be satisfied by the frozen parameter value.
        /// </summary>
        public Matching By { get; }

        /// <summary>
        /// Gets a <see cref="FreezeOnMatchCustomization" /> configured
        /// to match requests based on the <see cref="Type" /> and optionally
        /// the name of the parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter for which the customization is requested.
        /// </param>
        /// <returns>
        /// A <see cref="FreezeOnMatchCustomization" /> configured
        /// to match requests based on the <see cref="Type" /> and optionally
        /// the name of the parameter.
        /// </returns>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter is null) throw new ArgumentNullException(nameof(parameter));

            var matcher = new ParameterMatcherBuilder(parameter).SetFlags(this.By).Build();
            return new FreezeOnMatchCustomization(parameter, matcher);
        }
    }
}