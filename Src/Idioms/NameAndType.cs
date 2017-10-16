using System;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Holds a name and type. Instances are equal when the names are the
    /// same (ignoring case) and the types are exactly the same.
    /// </summary>
    internal class NameAndType
    {
        public string Name { get; private set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "It's fine to have the 'Type' property and we cannot re-use the GetType() method intead.")]
        public Type Type { get; private set; }

        public NameAndType(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var other = obj as NameAndType;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.Type, other.Type)
                && string.Equals(this.Name, other.Name,
                    StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.Name.ToUpperInvariant().GetHashCode() ^
                this.Type.GetHashCode();
        }
    }
}