using System.Linq;
using System.Reflection;
using TUnit.Core.Enums;

namespace AutoFixture.TUnit.Extensions;

internal static class DataGeneratorMetadataExtensions
{
    public static MethodBase GetMethod(this DataGeneratorMetadata dataGeneratorMetadata)
    {
        if (dataGeneratorMetadata.Type == DataGeneratorType.ClassParameters)
        {
            return dataGeneratorMetadata.TestClassType.GetConstructors().First();
        }

        return dataGeneratorMetadata.TestInformation.ReflectionInformation;
    }
}