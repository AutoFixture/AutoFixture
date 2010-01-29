using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class FieldAccessor : Accessor
    {
        private readonly FieldInfo fi;

        internal FieldAccessor(FieldInfo field, object value)
            : base(field, value)
        {
            this.fi = field;
        }

        internal override bool CanRead
        {
            get { return !this.fi.IsStatic; }
        }

        internal override bool CanWrite
        {
            get { return (!this.fi.IsInitOnly) && (!this.fi.IsStatic); }
        }

        internal override void AssignOn(object obj)
        {
            this.fi.SetValue(obj, this.Value);
        }

        internal override object ReadFrom(object obj)
        {
            return this.fi.GetValue(obj);
        }
    }
}
