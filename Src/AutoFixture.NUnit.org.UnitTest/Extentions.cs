using NUnit.Framework;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    public static class Extentions
    {
        /// <summary>
        /// Verifies that an object is of the given type or a derived type.
        /// 
        /// </summary>
        /// <typeparam name="T">The type the object should be</typeparam><param name="object">The object to be evaluated</param>
        /// <returns>
        /// The object, casted to type T when successful
        /// </returns>
        /// <exception cref="T:Xunit.Sdk.IsAssignableFromException">Thrown when the object is not the given type</exception>
        public static T IsAssignableFrom<T>(this Assert assert, object @object)
        {
            Assert.IsAssignableFrom(typeof(T), @object);
            return (T) @object; 
        }
    }
}