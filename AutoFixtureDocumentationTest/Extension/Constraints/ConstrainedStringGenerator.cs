using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Extension.Constraints
{
    public class ConstrainedStringGenerator
    {
        private readonly int minimumLength;
        private readonly int maximumLength;

        public ConstrainedStringGenerator(int minimumLength, 
            int maximumLength)
        {
            if (maximumLength < 0)
            {
                throw new ArgumentOutOfRangeException("...");
            }
            if (minimumLength > maximumLength)
            {
                throw new ArgumentOutOfRangeException("...");
            }

            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
        }

        public string CreateaAnonymous(string seed)
        {
            var s = string.Empty;
            while (s.Length < this.minimumLength)
            {
                s += GuidStringGenerator.CreateAnonymous(seed);
            }
            if (s.Length > this.maximumLength)
            {
                s = s.Substring(0, this.maximumLength);
            }
            return s;
        }
    }
}
