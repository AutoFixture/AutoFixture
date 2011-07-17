using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class CompositeMethodQuery : IMethodQuery
    {
        private readonly IEnumerable<IMethodQuery> queries;

        public CompositeMethodQuery(IEnumerable<IMethodQuery> queries)
            : this(queries.ToArray())
        {
        }

        public CompositeMethodQuery(params IMethodQuery[] queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException("parameterToCheck");
            }
            
            this.queries = queries;
        }

        public IEnumerable<IMethodQuery> Queries
        {
            get { return this.queries; }
        }

        #region IMethodQuery Members

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return from q in this.queries
                   from m in q.SelectMethods(type)
                   select m;
        }

        #endregion
    }
}
