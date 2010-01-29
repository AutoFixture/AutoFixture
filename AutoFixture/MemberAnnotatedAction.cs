using System;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class MemberAnnotatedAction<T>
    {
        private readonly Action<T> action;
        private readonly MemberInfo mi;

        internal MemberAnnotatedAction(Action<T> action)
            : this(action, null)
        {
        }

        internal MemberAnnotatedAction(Action<T> action, MemberInfo member)
        {
            this.action = action;
            this.mi = member;
        }

        internal Action<T> Action
        {
            get { return this.action; }
        }

        internal MemberInfo Member
        {
            get { return this.mi; }
        }
    }
}
