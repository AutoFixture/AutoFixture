using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public abstract class TypeWithSealedMembersTemp
    {
        public abstract string ExplicitlySealedProperty { get; set; }
        public abstract string ExplicitlySealedMethod();
    }

    public class TypeWithSealedMembers : TypeWithSealedMembersTemp
    {
        public override sealed string ExplicitlySealedProperty { get; set; }
        public string ImplicitlySealedProperty { get; set; }

        public override sealed string ExplicitlySealedMethod()
        {
            return "Awesome string";
        }

        public string ImplicitlySealedMethod()
        {
            return "Awesome string";
        }
    }
}
