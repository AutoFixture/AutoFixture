using System;

namespace TestTypeFoundation
{
    public class GuardedConstructorHostHoldingStaticReadOnlyProperty<TItem, TStaticProperty> where TItem : class
    {
        static GuardedConstructorHostHoldingStaticReadOnlyProperty()
        {
            Property = default(TStaticProperty);
        }

        public GuardedConstructorHostHoldingStaticReadOnlyProperty(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Item = item;
        }

        public static TStaticProperty Property { get;private set;}

        public TItem Item { get; private set; }
    }
}
