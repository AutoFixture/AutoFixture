using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    public static class IFixtureExtensions
    {
        public static IEnumerable<T> Repeat<T>(this IFixture fixture, Func<T> function)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }
            
            throw new NotImplementedException();
        }
    }
}
