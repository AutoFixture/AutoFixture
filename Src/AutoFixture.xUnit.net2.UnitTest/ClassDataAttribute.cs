using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace AutoFixture.Xunit2.UnitTest
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from a class
    /// which must implement IEnumerable&lt;object[]&gt;.
    /// </summary>
    /// <remarks>
    /// This is essentially the ClassDataAttribute class from xUnit 1, adjusted for changes in the DataAttribute API.
    /// It has been put here to make the existing tests run with xUnit 2 with the least possible effort.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClassDataAttribute : DataAttribute
    {
        private readonly Type @class;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDataAttribute"/> class.
        /// </summary>
        /// <param name="class">The class that provides the data.</param>
        public ClassDataAttribute(Type @class)
        {
            this.@class = @class;
        }

        /// <summary>
        /// Gets the type of the class that provides the data.
        /// </summary>
        public Type Class
        {
            get { return this.@class; }
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            return (IEnumerable<object[]>)Activator.CreateInstance(this.@class);
        }
    }
}