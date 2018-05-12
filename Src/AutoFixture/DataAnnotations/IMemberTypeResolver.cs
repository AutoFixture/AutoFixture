using System;

namespace AutoFixture.DataAnnotations
{
    public interface IMemberTypeResolver
    {
        Type TryGetMemberType(object request);
    }
}