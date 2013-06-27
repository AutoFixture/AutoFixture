using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RecursiveRequestBuilder
    {
        internal Stack<RecursiveRequestItem<T>> Build<T>(
            int recursionDepth,
            Func<T> subRequest1,
            Func<T> subRequest2
            ) where T : class
        {

            var value1 = subRequest1();
            var value2 = subRequest2();
            var request = new Stack<RecursiveRequestItem<T>>();

            while (recursionDepth > 0)
            {
                request.Push(new RecursiveRequestItem<T>(value2, recursionDepth));
                request.Push(new RecursiveRequestItem<T>(value1, recursionDepth));
                recursionDepth--;
            }

            return request;
        }
    }


}
