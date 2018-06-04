using System;

namespace AutoFixtureDocumentationTest.Simple
{
    public class Filter
    {
        private int max;
        private int min;

        public int Max
        {
            get => this.max;
            set
            {
                if (value < this.Min)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                this.max = value;
            }
        }

        public int Min
        {
            get => this.min;
            set
            {
                if (value > this.Max)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                this.min = value;
            }
        }
    }
}
