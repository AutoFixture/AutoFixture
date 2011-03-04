using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class CompositeMemberContext : IMemberContext
    {
        private readonly IEnumerable<IMemberContext> memberContexts;

        public CompositeMemberContext(params IMemberContext[] memberContexts)
        {
            if (memberContexts == null)
            {
                throw new ArgumentNullException("memberContexts");
            }

            this.memberContexts = memberContexts;
        }

        public CompositeMemberContext(IEnumerable<IMemberContext> memberContexts)
            : this(memberContexts.ToArray())
        {
        }

        public IEnumerable<IMemberContext> MemberContexts
        {
            get { return this.memberContexts; }
        }

        #region IMemberContext Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            foreach (var ctx in this.memberContexts)
            {
                ctx.VerifyBoundaries(convention);
            }
        }

        #endregion
    }
}
