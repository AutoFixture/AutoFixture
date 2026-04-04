using System;
using AutoFixture.Kernel;

namespace AutoFixture;

/// <summary>
/// If member request (e.g. PropertyInfo, FieldInfo, ParameterInfo) is passed, extracts the member type
/// and uses the inner <see cref="Builder"/> to resolve request value.
/// </summary>
public class UnwrapMemberRequest : ISpecimenBuilder
{
    private IRequestMemberTypeResolver _requestMemberTypeResolver = new RequestMemberTypeResolver();

    /// <summary>
    /// Gets or sets resolver used to extract member type.
    /// </summary>
    public IRequestMemberTypeResolver MemberTypeResolver
    {
        get => _requestMemberTypeResolver;
        set => _requestMemberTypeResolver = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Builder used to resolve the unwrapped member request.
    /// </summary>
    public ISpecimenBuilder Builder { get; }

    /// <summary>
    /// Creates a new instance of <see cref="UnwrapMemberRequest"/>.
    /// </summary>
    public UnwrapMemberRequest(ISpecimenBuilder builder)
    {
        Builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        if (MemberTypeResolver.TryGetMemberType(request, out Type memberType))
        {
            return Builder.Create(memberType, context);
        }

        return NoSpecimen.Instance;
    }
}