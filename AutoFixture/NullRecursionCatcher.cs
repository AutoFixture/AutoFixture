namespace Ploeh.AutoFixture
{
    using System;
    using Kernel;

    public class NullRecursionCatcher : RecursionCatcher
    {
        public NullRecursionCatcher(ISpecimenBuilder builder) : base(new InterceptingBuilder(builder))
        {
        }

        protected override object GetRecursionBreakSpecimen(object request)
        {
            return null;
        }
    }
}