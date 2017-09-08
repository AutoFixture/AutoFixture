using System;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedConstructorHostHoldingStaticReadOnlyField<TItem, TStaticField> where TItem : class
    {
        public static readonly TStaticField Field = default(TStaticField);

        public GuardedConstructorHostHoldingStaticReadOnlyField(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Item = item;
        }

        public TItem Item { get; private set; }
    }
}
