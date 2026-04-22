using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// Creates new <see cref="Guid"/> instances.
/// </summary>
public class GuidGenerator : ISpecimenBuilder
{
    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(Guid).Equals(request))
        {
            return NoSpecimen.Instance;
        }

        return Guid.NewGuid();
    }
}