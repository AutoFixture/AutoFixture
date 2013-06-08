using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Checks if two value objects are equal. Uses eiter <see cref="IEquatable{T}.Equals(T)"/> 
    /// or <see cref="Object.Equals"/> depending which one is implemented.
    /// </summary>
    internal class ValueObjectEqualityChecker
    {
        private readonly bool expectedResult;

        public ValueObjectEqualityChecker(bool expectedResult)
        {
            this.expectedResult = expectedResult;
        }

        public void CheckEquality(List<List<Tuple<object, object>>> listOfLists, Type type)
        {
            var iequatableInterface = GetIEquatableInterface(type);
            if (iequatableInterface != null)
            {
                listOfLists.ForEach(x => x.ForEach(y => this.CheckEqualityForIEquatable(y.Item1, y.Item2, iequatableInterface)));
            }
            else
            {
                listOfLists.ForEach(x => x.ForEach(y => this.CheckEquality(y.Item1, y.Item2)));
            }
        }

        private void CheckEqualityForIEquatable(object firstObject, object secondObject, Type iequatableInterface)
        {
            bool result;
            var objectResult = iequatableInterface.InvokeMember("Equals", BindingFlags.InvokeMethod,
                                                                null, firstObject,
                                                                new object[] { secondObject });

            result = objectResult is bool && (bool)objectResult;
            
            if (result != this.expectedResult)
                throw new ValueObjectEqualityException();
        }

        private void CheckEquality(object firstObject, object secondObject)
        {
            bool result;
            result = firstObject.Equals(secondObject);
            if (result != this.expectedResult)
                throw new ValueObjectEqualityException();
        }

        private static Type GetIEquatableInterface(Type type)
        {
            var iequatableTypeName = typeof(IEquatable<>).Name;
            var iequatableInterface = type.GetInterface(iequatableTypeName);
            return iequatableInterface;
        }
    }
}