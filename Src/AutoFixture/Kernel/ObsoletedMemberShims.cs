using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A container for the strongly obsoleted member shims.
    /// </summary>
    /// <remarks>
    /// You can use the "hard" obsoleted members from the "soft" obsoleted members.
    /// Use this trick to allow non-obsoleted the code use the totally obsoleted members for compatibility.
    /// </remarks>
    [Obsolete("Remove these shims when target members are completely removed.")]
    internal static class ObsoletedMemberShims
    {
        public interface ISpecifiedSpecimenCommand<T> : Kernel.ISpecifiedSpecimenCommand<T>
        {
        }

        public static object RecursionGuard_HandleRecursiveRequest(RecursionGuard guard, object request)
        {
            return guard.HandleRecursiveRequest(request);
        }

        public static Action<object, ISpecimenContext> Postprocessor_GetAction(Postprocessor postprocessor)
        {
            return postprocessor.Action;
        }

        public static void Postprocessor_SetAction(
            Postprocessor postprocessor, Action<object, ISpecimenContext> action)
        {
            postprocessor.Action = action;
        }
    }
}
