using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal abstract class Accessor
    {
        private readonly MemberInfo member;
        private readonly object value;

        protected Accessor(MemberInfo member, object value)
        {
            this.member = member;
            this.value = value;
        }

        protected object Value
        {
            get { return this.value; }
        }

        internal abstract bool CanRead { get; }

        internal abstract bool CanWrite { get; }

        internal MemberInfo Member
        {
            get { return this.member; }
        }

        internal abstract void AssignOn(object obj);

        internal abstract object ReadFrom(object obj);

        internal MemberAnnotatedAction<T> ToAnnotatedAction<T>()
        {
            return new MemberAnnotatedAction<T>(x => this.AssignOn(x), this.Member);
        }
    }
}
