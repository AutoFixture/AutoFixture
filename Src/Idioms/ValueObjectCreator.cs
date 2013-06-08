using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal abstract class ValueObjectCreator
    {
        private readonly IFixture fixture;
        private readonly IMethodQuery query;

        public ValueObjectCreator(IFixture fixture, IMethodQuery query)
        {
            this.fixture = fixture;
            this.query = query;
        }

        public IFixture Fixture {
            get { return this.fixture; }
        }

        public IMethodQuery Query {
            get { return this.query; }
        }

        protected IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where pi.GetSetMethod() != null
                         && pi.GetIndexParameters().Length == 0
                   select pi;
        }

        internal abstract List<List<Tuple<object, object>>> BuildType(Type type, ISpecimenContext context);
    }
}