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

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "AutoFixture.AutoNSubstitute", 
        Justification = "This is the root namespace of the project. There is no other namespace those types could be merged with.")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", 
        Justification = "AutoFixture itself currently doesn't have a strong name.")]

[assembly:
    SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant",
        Justification = "NSubstitute contains non-CLS compliant types and they are used, so lib is not CLS compliant.")]

[assembly:
    SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type",
        Target = "AutoFixture.AutoNSubstitute.CustomCallHandler.CallResultData+ArgumentValue",
        Justification =
            "It's defined as a sub-class because it's very tiny and it's just a way to group data together." +
            "It's never used without its parent and doesn't bring any value without parent.")]
