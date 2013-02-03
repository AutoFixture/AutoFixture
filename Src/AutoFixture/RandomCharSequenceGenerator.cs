using Ploeh.AutoFixture.Kernel;
using System;

namespace Ploeh.AutoFixture
{
    public class RandomCharSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomPrintableCharNumbers;

        public RandomCharSequenceGenerator()
        {
            this.randomPrintableCharNumbers =
                new RandomNumericSequenceGenerator(33, 126);
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(char).Equals(request))
                return new NoSpecimen(request);

            return Convert.ToChar(
                this.randomPrintableCharNumbers.Create(typeof(long), context));
        }
    }
}
