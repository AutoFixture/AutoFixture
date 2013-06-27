using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CapturingItemRecursionGuard : RecursionGuard
    {
        readonly int captureAtDepth;

        public CapturingItemRecursionGuard(ISpecimenBuilder builder, int recursionDepth)
            : base(builder, new DelegatingRecursionHandler())
        {
            this.captureAtDepth = recursionDepth;
        }

        public object CapturedItem { get; private set; }

        [Obsolete]
        public override object HandleRecursiveRequest(object request)
        {
            return this.OnHandleRecursiveRequest(request);
        }

        internal Func<object, object> OnHandleRecursiveRequest
        {
            get
            {
                return (obj) =>
                {
                    var recursiveItem = obj as RecursiveRequestItem<object>;
                    if (recursiveItem.RecursionDepth == this.captureAtDepth)
                        return CapturedItem = recursiveItem;
                    return null;
                };
            }
        }
    }
}
