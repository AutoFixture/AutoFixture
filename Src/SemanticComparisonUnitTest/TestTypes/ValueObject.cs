namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class ValueObject
    {
        public readonly int x;
        public readonly int y;

        public ValueObject(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ValueObject;
            if (other != null)
                return this.X == other.X
                    && this.Y == other.Y;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                this.X.GetHashCode() ^
                this.Y.GetHashCode();
        }
    }
}
