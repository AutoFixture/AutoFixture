using System;
using System.Collections;
using System.Linq;
using Ploeh.Albedo;

namespace Ploeh.AutoFixture.Idioms
{
    internal class SemanticReflectionVisitor : ReflectionVisitor<IEnumerable>
    {
        private readonly object[] values;

        public SemanticReflectionVisitor(
            params object[] values)
        {
            this.values = values;
        }

        public override IEnumerable Value
        {
            get { return this.values; }
        }

        public override IReflectionVisitor<IEnumerable> Visit(
            FieldInfoElement fieldInfoElement)
        {
            if (fieldInfoElement == null) throw new ArgumentNullException("fieldInfoElement");
            var v = new SemanticComparisonValue(
                fieldInfoElement.FieldInfo.Name,
                fieldInfoElement.FieldInfo.FieldType);
            return new SemanticReflectionVisitor(
                this.values.Concat(new[] {v}).ToArray());
        }

        public override IReflectionVisitor<IEnumerable> Visit(
            ParameterInfoElement parameterInfoElement)
        {
            if (parameterInfoElement == null) throw new ArgumentNullException("parameterInfoElement");
            var v = new SemanticComparisonValue(
                parameterInfoElement.ParameterInfo.Name,
                parameterInfoElement.ParameterInfo.ParameterType);
            return new SemanticReflectionVisitor(
                this.values.Concat(new[] {v}).ToArray());
        }

        public override IReflectionVisitor<IEnumerable> Visit(
            PropertyInfoElement propertyInfoElement)
        {
            if (propertyInfoElement == null) throw new ArgumentNullException("propertyInfoElement");
            var v = new SemanticComparisonValue(
                propertyInfoElement.PropertyInfo.Name,
                propertyInfoElement.PropertyInfo.PropertyType);
            return new SemanticReflectionVisitor(
                this.values.Concat(new[] {v}).ToArray());
        }
    }
}