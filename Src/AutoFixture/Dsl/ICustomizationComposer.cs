namespace AutoFixture.Dsl
{
    /// <summary>
    /// Provides statements that can be used to control how specimens are created and
    /// post-processed.
    /// </summary>
    /// <typeparam name="T">The type of specimen to customize.</typeparam>
    public interface ICustomizationComposer<T> : IFactoryComposer<T>, IPostprocessComposer<T>
    {
    }
}