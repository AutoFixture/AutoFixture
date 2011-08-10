using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// An implementation of DataAttribute that composes other DataAttribute instances.
    /// </summary>
    [CLSCompliant(false)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CompositeDataAttribute : DataAttribute
    {
        private readonly IEnumerable<DataAttribute> attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(IEnumerable<DataAttribute> attributes)
            : this(attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data theory.
        /// </param>
        public CompositeDataAttribute(params DataAttribute[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IEnumerable<DataAttribute> Attributes
        {
            get { return this.attributes; }
        }

        /// <summary>
        /// Returns the composition of data to be used to test the theory. Favors the data returned
        /// by DataAttributes in ascending order. Data already returned is ignored on next
        /// DataAttribute returned data.
        /// </summary>
        /// <param name="methodUnderTest">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the theory data.
        /// </returns>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            if (methodUnderTest == null)
            {
                throw new ArgumentNullException("methodUnderTest");
            }

            if (parameterTypes == null)
            {
                throw new ArgumentNullException("parameterTypes");
            }

            var theoryList = new List<object>();
            int parameters = methodUnderTest.GetParameters().Length;

            var grouppings = this.attributes
                .Select(attribute => attribute.GetData(methodUnderTest, parameterTypes))
                .SelectMany((theories, index)  => theories
                    .Select((theory, position) => new { theory, index, position }))
                .GroupBy(x => new { x.position })
                .Select(x => x.SelectMany(data => data.theory
                    .Select(theory => new { theory, data.index })));

            if (!grouppings.Any() && parameters > 0)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "Expected {0} parameters, got {1} parameters",
                        parameters, theoryList.Count
                        )
                    );
            }

            foreach (var group in grouppings)
            {
                foreach (var current in group)
                {
                    int remaining = parameters - theoryList.Count;
                    if (remaining == 0)
                    {
                        break;
                    }

                    int found = parameters - remaining;
                    if (found > 0)
                    {
                        var theories = group
                            .Where(g => g.index == current.index)
                            .Skip(found).Take(remaining).Select(data => data.theory);

                        theoryList.AddRange(theories);
                    }
                    else
                    {
                        theoryList.Add(current.theory);
                    }
                }

                if (theoryList.Count < parameters)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "Expected {0} parameters, got {1} parameters", 
                            parameters, theoryList.Count
                            )
                        );
                }

                yield return theoryList.ToArray();
            }
        }
    }
}
