using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class PropertyAccessor : Accessor
    {
        private readonly PropertyInfo pi;

        internal PropertyAccessor(PropertyInfo property, object value)
            : base(property, value)
        {
            this.pi = property;
        }

        internal override bool CanRead
        {
            get
            {
                return this.pi.CanRead
                    && this.pi.GetIndexParameters().Length == 0;
            }
        }

        internal override bool CanWrite
        {
            get 
            {
                return this.pi.CanWrite 
                    && this.pi.GetSetMethod() != null
                    && this.pi.GetIndexParameters().Length == 0; 
            }
        }

        internal override void AssignOn(object obj)
        {
            this.pi.SetValue(obj, this.Value, null);
        }

        internal override object ReadFrom(object obj)
        {
            return this.pi.GetValue(obj, null);
        }
    }
}
