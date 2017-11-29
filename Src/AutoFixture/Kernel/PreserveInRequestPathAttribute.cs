using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A marker to indicate that request of this type should not be skipped in the request path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class PreserveInRequestPathAttribute : Attribute
    {
    }
}