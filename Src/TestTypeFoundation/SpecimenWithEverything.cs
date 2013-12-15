using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ploeh.TestTypeFoundation
{
    /// <summary>
    /// A specimen that contains just about everything that AutoFixture can handle
    /// using all the defaults.
    /// </summary>
    public class SpecimenWithEverything
    {
        public static SpecimenWithEverything PublicStaticFactoryMethod()
        {
            return new SpecimenWithEverything("factoryCreated", new List<ConcreteType>
            {
                new ConcreteType(new object(), new object(), new object(), new object()),
                new ConcreteType(1, 2, 3, 4),
            });
        }
        
        public SpecimenWithEverything(
            string stringReadOnlyAssignedByCtorOnly,
            List<ConcreteType> listOfComposConcreteTypesAssignedByCtorOnly)
        {
            this.ListOfComposConcreteTypesAssignedByCtorOnly = listOfComposConcreteTypesAssignedByCtorOnly;
            this.StringReadOnlyAssignedByCtorOnly = stringReadOnlyAssignedByCtorOnly;
        }

        // public fields
        public int IntWritableField;
        public int? NullableIntWritableField;

        // ReadOnly property
        public string StringReadOnlyAssignedByCtorOnly { get; private set; }
        
        // Enumerable, Array, List, ObservableCollection
        public IEnumerable<string> EnumerableString { get; set; }
        public string[] StringArray { get; set; }
        public List<ConcreteType> ListOfComposConcreteTypesAssignedByCtorOnly { get; private set; }
        public ObservableCollection<ConcreteType> ObserviceCollectionOfConcreteTypes { get; set; }

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

        public bool IsPopulated()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                object propValue = prop.GetValue(this, null);
                
                // Handle everything
                if (propValue == null)
                {
                    // Null value
                    return false;
                }

                // Handle non-nullable primitives
                if (propValue.GetType().IsPrimitive
                    && 0.Equals(propValue))
                {
                    return false;
                }

                // Handle enumerables
                var propValueAsEnumerable = propValue as IEnumerable;
                if (propValueAsEnumerable != null
                    && !propValueAsEnumerable.GetEnumerator().MoveNext())
                {
                    // Empty enumerable
                    return false;
                }
            }
            return true;
        }
    }
}
