namespace TestTypeFoundation
{
    public class InternalGetterPropertyHolder<T>
    {
        public InternalGetterPropertyHolder(T property)
        {
            this.Property = property;
        }

        public T Property { internal get; set; }
    }
}