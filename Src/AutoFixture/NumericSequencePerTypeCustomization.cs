using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that enables numeric specimens to be unique within a specific numeric <see cref="Type"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When this customization is added to an <see cref="IFixture"/> instance, requests for numeric specimens
    /// will be satisfied by returning numbers from a <see cref="Type"/> specific sequence.
    /// </para>
    /// <para>
    /// This customization reproduces the generation strategy for numeric specimens used in AutoFixture up to version 2.1.
    /// </para>
    /// <example>
    /// <code>
    /// var fixture = new Fixture();
    /// fixture.Customize(new NumericSequencePerTypeCustomization());
    ///
    /// Console.WriteLine("Byte specimen is {0}", fixture.Create&lt;byte&gt;());
    /// Console.WriteLine("Int32 specimen is {0}", fixture.Create&lt;int&gt;());
    /// Console.WriteLine("Single specimen is {0}", fixture.Create&lt;float&gt;());
    ///
    /// // The output of this program will be:
    /// // Byte specimen is 1
    /// // Int32 specimen is 1
    /// // Single specimen is 1
    /// </code>
    /// </example>
    /// </remarks>
    public class NumericSequencePerTypeCustomization : ICustomization
    {
        /// <summary>
        /// Customizes the specified fixture by adding the <see cref="Type"/> specific numeric sequence generators.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var numericBuilders = new CompositeSpecimenBuilder(new ISpecimenBuilder[]
            {
                new ByteSequenceGenerator(),
                new DecimalSequenceGenerator(),
                new DoubleSequenceGenerator(),
                new Int16SequenceGenerator(),
                new Int32SequenceGenerator(),
                new Int64SequenceGenerator(),
                new SByteSequenceGenerator(),
                new SingleSequenceGenerator(),
                new UInt16SequenceGenerator(),
                new UInt32SequenceGenerator(),
                new UInt64SequenceGenerator()
            });

            fixture.Customizations.Add(numericBuilders);
        }
    }
}