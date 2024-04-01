// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "AutoFixture is not supposed to be used for generating security sensitive data.")]
[assembly: SuppressMessage("Security", "CA2300:Do not use insecure deserializer BinaryFormatter", Justification = "The use of BinaryFormatter is maked as Obsolete, and will be removed in future versions.")]
[assembly: SuppressMessage("Build", "CA2301:The method 'object BinaryFormatter.Deserialize(Stream serializationStream)' is insecure when deserializing untrusted data without a SerializationBinder to restrict the type of objects in the deserialized object graph. (https://docs.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca2301)",
    Justification = "The BinaryFormatter code has been marked as Obsolete and will be removed in future versions.")]
