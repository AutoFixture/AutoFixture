
namespace Ploeh.AutoFixtureUnitTest
{
    public class RecursiveRequestItem<TValue> where TValue : class
    {
        readonly TValue value;
        readonly int recursionDepth;

        public RecursiveRequestItem(TValue value, int recursionDepth)
        {
            this.value = value;
            this.recursionDepth = recursionDepth;
        }

        public int RecursionDepth { get { return this.recursionDepth; } }

        public override bool Equals(object obj)
        {
            var compare = obj as TValue;

            if (compare != null)
            {
                return compare.Equals(this.value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}
