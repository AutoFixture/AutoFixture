using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixture.Kernel
{
    public class ConstrainedMaxLengthStringRequest : IEquatable<ConstrainedMaxLengthStringRequest>
    {

        public int MaxLength { get; }

        public ConstrainedMaxLengthStringRequest(int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length must be equal or greater than 0.");
            }

            this.MaxLength = maxLength;
        }

        public override bool Equals(object obj)
        {
            if (obj is ConstrainedMaxLengthStringRequest other)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.MaxLength.GetHashCode();
        }

        public bool Equals(ConstrainedMaxLengthStringRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.MaxLength == other.MaxLength;
        }
    }
}
