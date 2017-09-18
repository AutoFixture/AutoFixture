namespace Ploeh.AutoFixture.AutoNSubstitute.CustomCallHandler
{
    /// <inheritdoc />
    public class CallResultCacheFactory : ICallResultCacheFactory
    {
        /// <inheritdoc />
        public ICallResultCache CreateCache()
        {
            return new CallResultCache();
        }
    }
}