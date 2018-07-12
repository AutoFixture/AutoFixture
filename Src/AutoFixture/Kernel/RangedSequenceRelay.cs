using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relay the <see cref="RangedSequenceRelay"/> to the request for the sequence of the fixed length.
    /// </summary>
    public class RangedSequenceRelay : ISpecimenBuilder
    {
        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var rsr = request as RangedSequenceRequest;
            if (rsr == null)
                return new NoSpecimen();

            if (!TryGetSequenceLength(rsr, context, out int sequenceLength))
                return new NoSpecimen();

            return context.Resolve(new FiniteSequenceRequest(rsr.Request, sequenceLength));
        }

        private static bool TryGetSequenceLength(RangedSequenceRequest rsr, ISpecimenContext ctx, out int length)
        {
            var result = ctx.Resolve(new RangedNumberRequest(typeof(int), rsr.MinLength, rsr.MaxLength));
            if (result is int randNumber)
            {
                length = randNumber;
                return true;
            }

            length = default;
            return false;
        }
    }
}