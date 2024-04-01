using System;

namespace TestTypeFoundation
{
    public class GuardedConstructorHostHoldingStaticReadOnlyField<TItem, TStaticField>
        where TItem : class
    {
        public static readonly TStaticField Field;

        public GuardedConstructorHostHoldingStaticReadOnlyField(TItem item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Item = item;
        }

        public TItem Item { get; private set; }
    }
}
