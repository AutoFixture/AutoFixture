using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.Albedo;
using Ploeh.Albedo.Refraction;

namespace Ploeh.AutoFixtureUnitTest
{
    /// <summary>
    /// Collects values from fields and properties on any object.
    /// </summary>
    class ValueCollectingVisitor : ReflectionVisitor<IEnumerable>
    {
        private readonly object target;
        private readonly object[] values;

        public ValueCollectingVisitor(object target, params object[] values)
        {
            this.target = target;
            this.values = values;
        }

        public override IEnumerable Value
        {
            get { return this.values; }
        }

        public override IReflectionVisitor<IEnumerable> Visit(
            FieldInfoElement fieldInfoElement)
        {
            var value = fieldInfoElement.FieldInfo.GetValue(this.target);
            return new ValueCollectingVisitor(
                this.target,
                this.values.Concat(new[] { value }).ToArray());
        }

        public override IReflectionVisitor<IEnumerable> Visit(
            PropertyInfoElement propertyInfoElement)
        {
            var value =
                propertyInfoElement.PropertyInfo.GetValue(this.target, null);
            return new ValueCollectingVisitor(
                this.target,
                this.values.Concat(new[] { value }).ToArray());
        }

        public static IEnumerable<object> GetAllValues<TSpecimen>(
            TSpecimen specimen, BindingFlags flags)
        {
            var allPropertiesAndFields = typeof(TSpecimen)
               .GetProperties(flags).Select(p => p.ToReflectionElement())
               .Concat(typeof(TSpecimen)
                   .GetFields(flags).Select(f => f.ToReflectionElement()))
               .ToArray();

            return new CompositeReflectionElement(allPropertiesAndFields)
                .Accept(new ValueCollectingVisitor(specimen))
                .Value
                .Cast<object>();
        } 

        public static IEnumerable<object> GetAllInstanceValues<TSpecimen>(TSpecimen specimen)
        {
            return GetAllValues(specimen,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}