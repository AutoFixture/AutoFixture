using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture.Kernel
{
    public class ConstrainedMinLengthStringRequest : IEquatable<ConstrainedMinLengthStringRequest>
    {

        public int MinLength { get; }

        public ConstrainedMinLengthStringRequest(int minLength)
        {
            if (minLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length must be equal or greater than 0.");
            }

            this.MinLength = minLength;
        }

        public override bool Equals(object obj)
        {
            if (obj is ConstrainedMinLengthStringRequest other)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.MinLength.GetHashCode();
        }

        public bool Equals(ConstrainedMinLengthStringRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.MinLength == other.MinLength;
        }
    }
}
