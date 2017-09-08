namespace Ploeh.AutoFixture.Idioms.FsCheck

open System

/// <summary>
/// Represents an error about a public Query (a method that returns a value)
/// with null return value.
/// </summary>
type ReturnValueMustNotBeNullException (message) =
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ReturnValueMustNotBeNullException"/> class.
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    inherit Exception (message)