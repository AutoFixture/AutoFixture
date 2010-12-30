using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class TypeWithOverloadedMembers
    {
        public object SomeProperty { get; set; }

        public void DoSomething()
        {
        }

        public void DoSomething(object obj)
        {
        }

        public void DoSomething(object x, object y)
        {
        }

        public void DoSomething(object x, object y, object z)
        {
        }
    }
}
