using System;

namespace Ploeh.AutoFixture.Xunit
{
    /// <summary>
    /// Base class for customizing parameters in methods decorated with
    /// <see cref="AutoDataAttribute"/>.
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of AutoFixture. Please use AutoFixture.CustomizeAttribute instead.")]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class CustomizeAttribute : AutoFixture.CustomizeAttribute
    {
    }
}
