using System;

namespace Ploeh.AutoFixtureUnitTest
{
    public class QueryMock<T, TResult>
    {
        public QueryMock()
        {
            this.OnQuery = x => default(TResult);
        }

        public Func<T, TResult> OnQuery { get; set; }

        public TResult Query(T x)
        {
            return this.OnQuery(x);
        }
    }

    public class QueryMock<T1, T2, TResult>
    {
        public QueryMock()
        {
            this.OnQuery = (x1, x2) => default(TResult);
        }

        public Func<T1, T2, TResult> OnQuery { get; set; }

        public TResult Query(T1 x1, T2 x2)
        {
            return this.OnQuery(x1, x2);
        }
    }

    public class QueryMock<T1, T2, T3, TResult>
    {
        public QueryMock()
        {
            this.OnQuery = (x1, x2, x3) => default(TResult);
        }

        public Func<T1, T2, T3, TResult> OnQuery { get; set; }

        public TResult Query(T1 x1, T2 x2, T3 x3)
        {
            return this.OnQuery(x1, x2, x3);
        }
    }

    public class QueryMock<T1, T2, T3, T4, TResult>
    {
        public QueryMock()
        {
            this.OnQuery = (x1, x2, x3, x4) => default(TResult);
        }

        public Func<T1, T2, T3, T4, TResult> OnQuery { get; set; }

        public TResult Query(T1 x1, T2 x2, T3 x3, T4 x4)
        {
            return this.OnQuery(x1, x2, x3, x4);
        }
    }
}
