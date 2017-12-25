using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using TestTypeFoundation;

namespace AutoFixtureUnitTest
{
    /// <summary>
    /// A specimen that contains just about everything that AutoFixture can handle
    /// using all the defaults.
    /// </summary>
    internal class SpecimenWithEverything
    {
        public static SpecimenWithEverything PublicStaticFactoryMethod()
        {
            return new SpecimenWithEverything(
                stringReadOnlyFieldAssignedByCtorOnly: "factoryCreated",
                stringReadOnlyAssignedByCtorOnly: "factoryCreated",
                listOfConcreteTypesAssignedByCtorOnly: new List<ConcreteType>
                {
                    new ConcreteType(new object(), new object(), new object(), new object()),
                    new ConcreteType(1, 2, 3, 4),
                });
        }

        public virtual bool PublicVirtualInstanceQueryMethod()
        {
            return true;
        }

        public SpecimenWithEverything(
            string stringReadOnlyFieldAssignedByCtorOnly,
            string stringReadOnlyAssignedByCtorOnly,
            List<ConcreteType> listOfConcreteTypesAssignedByCtorOnly)
        {
            this.StringReadOnlyFieldAssignedByCtorOnly = stringReadOnlyFieldAssignedByCtorOnly;
            this.StringReadOnlyAssignedByCtorOnly = stringReadOnlyAssignedByCtorOnly;
            this.ListOfConcreteTypesAssignedByCtorOnly = listOfConcreteTypesAssignedByCtorOnly;
        }

        // public fields
        public readonly string StringReadOnlyFieldAssignedByCtorOnly;
#pragma warning disable 649
        public int IntWritableField;
        public Guid? NullableGuidWritableField;
#pragma warning restore 649

        // ReadOnly property
        public string StringReadOnlyAssignedByCtorOnly { get; private set; }

        // Enumerable, Array, List, ObservableCollection, Dictionary
        public IEnumerable<string> EnumerableString { get; set; }
        public string[] StringArray { get; set; }
        public List<ConcreteType> ListOfConcreteTypesAssignedByCtorOnly { get; private set; }
        public ObservableCollection<ConcreteType> ObservableCollectionOfConcreteTypes { get; set; }
        public Dictionary<string, ConcreteType> DictionaryOfConcreteTypes { get; set; }
        public byte[] ArrayOfBytes { get; set; }
        public char[] ArrayOfChars { get; set; }

        // Additional default primitive parts
        public Uri Uri { get; set; }
        public Type Type { get; set; }
        // public IntPtr? IntPtr { get; set; }
        public EventHandler<EventArgs> EvenHandlerDelegate { get; set; }
        public bool? NullableBool { get; set; }
        public Guid? NullableGuid { get; set; }
        public DateTime? NullableDateTime { get; set; }
        public decimal? NullableDecimal { get; set; }
        public double? NullableDouble { get; set; }
        public float? NullableFloat { get; set; }

        // Attribute-based properties
        [RegularExpression("[0-9]")]
        public string StringWithRegExAttributeOnlyAllowingNumbers { get; set; }

        [RegularExpression("[A-Z]")]
        public string StringWithRegExAttributeOnlyAllowingUpperCaseLetters { get; set; }

        [Range(44.44d, 55.55d)]
        public double? DoubleWithRangeAttributeOnlyAllowing44Point44To55Point55 { get; set; }

        [StringLength(5)]
        public string StringWithStringLengthAttributeOnlyAllowing5Characters { get; set; }
    }
}
