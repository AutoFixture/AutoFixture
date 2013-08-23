using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit.org
{
    /// <summary>
    /// Abstract attribute which represents a data source for a data theory.
    ///             Data source providers derive from this attribute and implement GetData
    ///             to return the data for the theory.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]   
    public abstract class DataAttribute : Attribute
    {
        private readonly object _typeId = new object();

        /// <inheritdoc/>
        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        /// <summary>
        /// Returns the composition of data to be used to test the theory. Favors the data returned
        /// by DataAttributes in ascending order. Data already returned is ignored on next
        /// DataAttribute returned data.
        /// </summary>
        /// <param name="methodUnderTest">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the theory data.
        /// </returns>
        /// <remarks>
        /// The number of test cases is set from the first DataAttribute theory length.
        /// </remarks>
        public abstract IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes);
    }
}