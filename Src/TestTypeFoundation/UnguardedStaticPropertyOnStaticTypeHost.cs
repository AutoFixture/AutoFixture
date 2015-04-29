namespace Ploeh.TestTypeFoundation
{
    public static class UnguardedStaticPropertyOnStaticTypeHost
    {
        public static object Property
        {
            get;
            set;
        }
    }
}