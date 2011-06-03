using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseException : Exception
    {
        private readonly MemberInfo memberInfo;
        private readonly Type valueType;

        public GuardClauseException(Exception e)
            : base(Guid.NewGuid().ToString(), e)
        {
        }

        public GuardClauseException()
        {
        }

        public GuardClauseException(MemberInfo memberInfo, Type valueType)
        {
            this.memberInfo = memberInfo;
            this.valueType = valueType;
        }

        public GuardClauseException(MemberInfo memberInfo, Type valueType, string message)
        {
            this.memberInfo = memberInfo;
            this.valueType = valueType;
        }

        public GuardClauseException(MemberInfo memberInfo, Type valueType, string message, Exception innerException)
            : base(message, innerException)
        {
            this.memberInfo = memberInfo;
            this.valueType = valueType;
        }

        public MemberInfo MemberInfo
        {
            get { return this.memberInfo; }
        }

        public Type ValueType
        {
            get { return this.valueType; }
        }
    }
}
