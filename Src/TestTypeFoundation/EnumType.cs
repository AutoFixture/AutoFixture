namespace TestTypeFoundation
{
    public enum EnumType
    {
        First = 1,
        Second = 2,
        Third = 3
    }

    public enum ByteEnumType : byte
    {
        First = 99,
        Second,
        Third
    }

    public enum SByteEnumType : sbyte
    {
        First = -98,
        Second,
        Third
    }

    public enum ShortEnumType : short
    {
        First = 100,
        Second,
        Third
    }

    public enum UShortEnumType : ushort
    {
        First = 1,
        Second,
        Third
    }

    public enum UIntEnumType : uint
    {
        First,
        Second,
        Third
    }

    public enum LongEnumType : long
    {
        First = (long)int.MaxValue + 1,
        Second,
        Third
    }

    public enum ULongEnumType : ulong
    {
        First = 1,
        Second,
        Third
    }
}