using System.Collections.Generic;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    /// Storage for a single resolved call result.
    /// </summary>
    public class CallResultData
    {
        /// <summary>
        /// Value that should be returned.
        /// </summary>
        public Maybe<object> ReturnValue { get; }

        /// <summary>
        /// Ref/Out argument values.
        /// </summary>
        public IEnumerable<ArgumentValue> ArgumentValues { get; }

        /// <summary>
        /// Initializes instance of the <see cref="CallResultData"/>.
        /// </summary>
        public CallResultData(Maybe<object> returnValue, IEnumerable<ArgumentValue> argumentValues)
        {
            this.ReturnValue = returnValue;
            this.ArgumentValues = argumentValues;
        }

        /// <summary>
        /// Intance represents resolved argument value by index.
        /// </summary>
        public class ArgumentValue
        {
            /// <summary>
            /// Argument index starting from zero.
            /// </summary>
            public int Index { get; }

            /// <summary>
            /// Argument value.
            /// </summary>
            public object Value { get; }

            /// <summary>
            /// Creates an instance of <see cref="ArgumentValue"/>.
            /// </summary>
            public ArgumentValue(int index, object value)
            {
                this.Index = index;
                this.Value = value;
            }
        }
    }
}