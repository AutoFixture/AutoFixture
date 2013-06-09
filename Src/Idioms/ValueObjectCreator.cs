using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

        internal abstract List<List<Tuple<object, object, StringBuilder>>> BuildType(Type type, ISpecimenContext context);
    }
}