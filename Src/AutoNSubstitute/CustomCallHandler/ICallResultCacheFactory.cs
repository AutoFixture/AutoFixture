namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <summary>
    ///  Factory to create an instance of the cache used later to store resolved call results. 
    /// </summary>
    public interface ICallResultCacheFactory
    {
        /// <summary>
        /// Create a new instance of cache.
        /// </summary>
        ICallResultCache CreateCache();
    }
}