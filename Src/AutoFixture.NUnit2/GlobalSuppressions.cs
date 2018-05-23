// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NUnit", Scope = "namespace", Target = "AutoFixture.NUnit2", Justification ="NUnit is the proper name")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NUnit", Justification = "NUnit is the proper name")]

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
        Scope = "namespace",
        Target = "AutoFixture.NUnit2.Addins",
        Justification = "It has been ported from other project and I don't want to introduce the breaking changes.")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
        Scope = "namespace",
        Target = "AutoFixture.NUnit2.Addins.Builders",
        Justification = "It has been ported from other project and I don't want to introduce the breaking changes.")]