using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        public void CheckEquality(List<List<Tuple<object, object, StringBuilder>>> listOfLists, Type type)
        {
            var iequatableInterface = GetIEquatableInterface(type);
            if (iequatableInterface != null)
            {
                listOfLists.ForEach(x => x.ForEach(y => this.CheckEqualityForIEquatable(y.Item1, y.Item2, y.Item3.ToString(), iequatableInterface)));
            }
            else
            {
                listOfLists.ForEach(x => x.ForEach(y => this.CheckEquality(y.Item1, y.Item2, y.Item3.ToString())));
            }
        }

        private void CheckEqualityForIEquatable(object firstObject, object secondObject, string message, Type iequatableInterface)
        {
            bool result;
            var objectResult = iequatableInterface.InvokeMember("Equals", BindingFlags.InvokeMethod,
                                                                null, firstObject,
                                                                new object[] { secondObject });

            result = objectResult is bool && (bool)objectResult;

            if (result != this.expectedResult)
                throw new ValueObjectEqualityException(string.Concat(message,
                                                                     string.Format(
                                                                         "For thoose values IEquatable.Equals method was called and returned: {0} while expected was {1}",
                                                                         result, this.expectedResult)));
        }

        private void CheckEquality(object firstObject, object secondObject, string message)
        {
            bool result;
            result = firstObject.Equals(secondObject);
            if (result != this.expectedResult)
                throw new ValueObjectEqualityException(string.Concat(message,
                                                                     string.Format(
                                                                         "For thoose values Equals method was called and returned: {0} wile expected was {1}",
                                                                         result, this.expectedResult)));
        }

        private static Type GetIEquatableInterface(Type type)
        {
            var iequatableTypeName = typeof(IEquatable<>).Name;
            var iequatableInterface = type.GetInterface(iequatableTypeName);
            return iequatableInterface;
        }
    }
}