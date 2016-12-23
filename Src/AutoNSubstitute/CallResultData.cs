using System;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Storage for a single resolved call result.
    /// </summary>
    public class CallResultData
    {
        /// <summary>
        /// Initializes instance of the <see cref="CachedCallResult"/>
        /// </summary>
        public CallResultData(Maybe<object> returnValue, Tuple<int, object>[] argumentValues)
        {
            ReturnValue = returnValue;
            ArgumentValues = argumentValues;
        }

        /// <summary>
        /// Value that should be returned.
        /// </summary>
        public Maybe<object> ReturnValue { get; }

        /// <summary>
        /// Ref/Out argument values. Each tuple store argument index and resolved value.
        /// </summary>
        public Tuple<int, object>[] ArgumentValues { get; }
    }
}