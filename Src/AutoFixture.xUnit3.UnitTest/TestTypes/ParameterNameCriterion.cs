using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Xunit3.UnitTest.TestTypes
{
    internal class ParameterNameCriterion : IEquatable<ParameterInfo>
    {
        public ParameterNameCriterion(string name)
            : this(new Criterion<string>(name, StringComparer.Ordinal))
        {
        }

        public ParameterNameCriterion(IEquatable<string> nameCriterion)
        {
            this.NameCriterion = nameCriterion ?? throw new ArgumentNullException(nameof(nameCriterion));
        }

        /// <summary>
        /// The name criterion originally passed in via the class' constructor.
        /// </summary>
        public IEquatable<string> NameCriterion { get; }

        public bool Equals(ParameterInfo other)
        {
            return other is not null
                   && this.NameCriterion.Equals(other.Name);
        }
    }
}