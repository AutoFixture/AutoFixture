using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitSpecimen : IEquatable<OmitSpecimen>
    {
        public override bool Equals(object obj)
        {
            var other = obj as OmitSpecimen;
            if (other != null)
                return this.Equals(other);
            return base.Equals(obj);
        }

        public bool Equals(OmitSpecimen other)
        {
            return other != null;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
