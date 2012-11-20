using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public static class SpecimenQuery
    {
        public static TResult Get<T, TResult>(
            this ISpecimenBuilder builder,
            Func<T, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(builder.Create<T>());
        }

        public static TResult Get<T1, T2, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(builder.Create<T1>(), builder.Create<T2>());
        }

        public static TResult Get<T1, T2, T3, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>());
        }

        public static TResult Get<T1, T2, T3, T4, TResult>(
            this ISpecimenBuilder builder,
            Func<T1, T2, T3, T4, TResult> function)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");
            if (function == null)
                throw new ArgumentNullException("function");

            return function(
                builder.Create<T1>(),
                builder.Create<T2>(),
                builder.Create<T3>(),
                builder.Create<T4>());
        }
    }
}
