using System;
using System.Globalization;
using System.Reflection;

namespace Ploeh.AutoFixture.AutoMoq
{
    internal class MockConstructorInfo : ConstructorInfo
    {
        private readonly ConstructorInfo ctor;
        private readonly ParameterInfo[] paramInfos;

        internal MockConstructorInfo(ConstructorInfo ctor, ParameterInfo[] paramInfos)
        {
            if (ctor == null)
            {
                throw new ArgumentNullException("ctor");
            }
            if (paramInfos == null)
            {
                throw new ArgumentNullException("paramInfos");
            }

            this.ctor = ctor;
            this.paramInfos = paramInfos;
        }

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            var paramsArray = new object[] { parameters };
            return this.ctor.Invoke(invokeAttr, binder, paramsArray, culture);
        }

        public override MethodAttributes Attributes
        {
            get { return this.ctor.Attributes; }
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return this.ctor.GetMethodImplementationFlags();
        }

        public override ParameterInfo[] GetParameters()
        {
            return this.paramInfos;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return this.ctor.Invoke(obj, invokeAttr, binder, parameters, culture);
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { return this.ctor.MethodHandle; }
        }

        public override Type DeclaringType
        {
            get { return this.ctor.DeclaringType; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return this.ctor.GetCustomAttributes(attributeType, inherit);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            return this.ctor.GetCustomAttributes(inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return this.ctor.IsDefined(attributeType, inherit);
        }

        public override string Name
        {
            get { return this.ctor.Name; }
        }

        public override Type ReflectedType
        {
            get { return this.ctor.ReflectedType; }
        }
    }
}
