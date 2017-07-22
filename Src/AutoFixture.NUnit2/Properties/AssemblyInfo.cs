using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3a54aa4b-be9b-4591-bf8c-0a463e91088f")]

[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en")]

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace",
        Target = "Ploeh.AutoFixture.NUnit2.Addins",
        Justification = "It has been ported from other project and I don't want to introduce the breaking changes.")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", 
        Scope = "namespace",
        Target = "Ploeh.AutoFixture.NUnit2.Addins.Builders",
        Justification = "It has been ported from other project and I don't want to introduce the breaking changes.")]
        