using System;

namespace Ploeh.SemanticComparison.UnitTest.TestTypes
{
    public class Entity
    {
        public readonly string name;
        public readonly Guid id;

        public Entity(string name)
        {
            this.name = name;
            this.id = Guid.NewGuid();
        }

        public string Name
        {
            get { return this.name; }
        }

        public Guid Id
        {
            get { return this.id; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;
            if (other != null)
                return this.Id.Equals(other.Id);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
