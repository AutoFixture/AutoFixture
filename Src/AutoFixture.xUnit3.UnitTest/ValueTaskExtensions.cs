using System.Threading.Tasks;

namespace AutoFixture.Xunit3.UnitTest;

internal static class ValueTaskExtensions
{
    /// <summary>
    /// Wraps the value in a <see cref="ValueTask{T}"/>.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <typeparam name="T">The generic type of the value task.</typeparam>
    /// <returns>Returns a completed <see cref="ValueTask{T}"/> instance.</returns>
    public static ValueTask<T> ToValueTask<T>(this T value) => new(value);

    /// <summary>
    /// Wraps the task in a <see cref="ValueTask{T}"/>.
    /// </summary>
    /// <param name="task">The task to be wrapped.</param>
    /// <typeparam name="T">The generic type of the value task.</typeparam>
    /// <returns>Returns a <see cref="ValueTask{T}"/> instance.</returns>
    public static ValueTask<T> ToValueTask<T>(this Task<T> task) => new(task);
}