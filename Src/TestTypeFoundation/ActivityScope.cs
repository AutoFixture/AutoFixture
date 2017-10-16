using System;

namespace TestTypeFoundation
{
    [Flags]
    public enum ActivityScope : long
    {
        /// <summary>
        ///   Undefined scope.
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///   The OnDuty activity has its own special scope.
        /// </summary>
        OnDuty = 0x01,

        /// <summary>
        ///   The OffDuty activity has its own special scope.
        /// </summary>
        OffDuty = 0x02,

        /// <summary>
        ///   A standalone has no parent or children.
        /// </summary>
        Standalone = 0x04,

        /// <summary>
        ///   A parent activity can have one or more child activities..
        /// </summary>
        Parent = 0x08,

        /// <summary>
        ///   A child activity has a parent activity.
        /// </summary>
        Child = 0x10,

        /// <summary>
        ///   The set of all scopes that are allowed when there is no open Parent activity.
        /// </summary>
        AllInitiatingScopes = Parent | Standalone,

        /// <summary>
        ///   The set all valid scopes.
        /// </summary>
        All = OnDuty | OffDuty | Standalone | Parent | Child,
    }
}