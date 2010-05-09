using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Represents types that hold the potential to generate instances based on a given
    /// description.
    /// </summary>
    public interface IInstanceGenerator
    {
        /// <summary>
        /// Indicates whether the current instance can generate object instances based on the given
        /// <see cref="ICustomAttributeProvider"/>.
        /// </summary>
        /// <param name="attributeProvider">
        /// Provides a description upon which the current instance can base its decision.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current instance can generate object instances based on
        /// <paramref name="attributeProvider"/>; otherwise, <see langword="false"/>.
        /// </returns>
        bool CanGenerate(ICustomAttributeProvider attributeProvider);

        /// <summary>
        /// Generates object instances based on the given <see cref="ICustomAttributeProvider"/>.
        /// </summary>
        /// <param name="attributeProvider">
        /// Provides a description that guides the current instance in generating object instances.
        /// </param>
        /// <returns>A new object based on <paramref name="attributeProvider"/>.</returns>
        object Generate(ICustomAttributeProvider attributeProvider);
    }
}
