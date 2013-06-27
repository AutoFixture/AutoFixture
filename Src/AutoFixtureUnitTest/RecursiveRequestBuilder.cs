using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RecursiveRequestBuilder
    {
        internal Stack<RecursiveRequestItem<T>> Build<T>(
            int recursionDepth,
            Func<T> request
            ) where T : class
        {

            var value = request();
            var recursiveRequest = new Stack<RecursiveRequestItem<T>>();

            while (recursionDepth > 0)
            {
                recursiveRequest.Push(new RecursiveRequestItem<T>(value, recursionDepth));
                recursionDepth--;
            }

            return recursiveRequest;
        }
    }


}
