using System;
using System.Collections.Generic;

namespace TestTypeFoundation
{
    public class RecordType<T> : IEquatable<RecordType<T>>
    {
        public RecordType(T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        public bool Equals(RecordType<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as RecordType<T>);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(this.Value);
        }
    }
}