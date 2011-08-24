using System;

namespace Ploeh.TestTypeFoundation
{
    public class UnguardedMethodHost
    {
        public void ConsumeUnguardedString(string s)
        {
        }

        public void ConsumeGuardedGuidAndUnguardedString(Guid g, string s)
        {
            if (g == Guid.Empty)
            {
                throw new ArgumentException("Guid cannont be empty.", "g");
            }
        }
    }
}
