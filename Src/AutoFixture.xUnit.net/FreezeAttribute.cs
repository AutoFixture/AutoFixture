using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Xunit
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FreezeAttribute : CustomizeAttribute
    {
        public FreezeAttribute()
        {
            this.By = Matching.ExactType;
        }

        public Matching By { get; set; }

        public string TargetName { get; set; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            Require.IsNotNull(parameter, "parameter");

            return FreezeTypeWithMatchingRules(parameter.ParameterType);
        }

        private ICustomization FreezeTypeWithMatchingRules(Type type)
        {
            return (ICustomization)Activator.CreateInstance(
                typeof(FreezeOnMatchCustomization<>).MakeGenericType(type),
                this.GetMatchingStrategy(),
                this.GetTargetNames());
        }

        private Matching GetMatchingStrategy()
        {
            return this.By | Matching.ExactType;
        }

        private string[] GetTargetNames()
        {
            return this.TargetName != null ?
                new[] { this.TargetName }
                : new string[0];
        }
    }
}
