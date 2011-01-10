using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class ConcreteType : AbstractType
    {
        public ConcreteType()
        {
        }

        public ConcreteType(object obj)
        {
            this.Property1 = obj;
        }

        public ConcreteType(object obj1, object obj2)
        {
            this.Property1 = obj1;
            this.Property2 = obj2;
        }

        public ConcreteType(object obj1, object obj2, object obj3)
        {
            this.Property1 = obj1;
            this.Property2 = obj2;
            this.Property3 = obj3;
        }

        public ConcreteType(object obj1, object obj2, object obj3, object obj4)
        {
            this.Property1 = obj1;
            this.Property2 = obj2;
            this.Property3 = obj3;
            this.Property4 = obj4;
        }

        public override object Property4 { get; set; }

        public object Property5 { get; set; }
    }
}
