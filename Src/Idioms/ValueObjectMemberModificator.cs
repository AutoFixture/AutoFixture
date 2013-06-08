using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    internal abstract class ValueObjectMemberModificator
    {
        private readonly IFixture fixture;

        public ValueObjectMemberModificator(IFixture fixture)
        {
            this.fixture = fixture;
        }

        public IFixture Fixture
        {
            get { return this.fixture; }
        }

        internal static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where pi.GetSetMethod() != null
                   && pi.GetIndexParameters().Length == 0
                   select pi;
        }

        internal static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return from fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                   where !fi.IsInitOnly
                   select fi;
        }

        internal abstract void ChangeMembers(List<Tuple<object, object>> listOfObjectsToModify, Type type, ISpecimenContext context);
    }
}