using System;
using System.Reflection;
using TUnit.Core.Enums;

namespace AutoFixture.TUnit.UnitTest;

public class DataGeneratorMetadataHelper
{
    public static DataGeneratorMetadata CreateDataGeneratorMetadata(MethodInfo methodInfo)
    {
        return CreateDataGeneratorMetadata(methodInfo.DeclaringType!, methodInfo.Name);
    }

    public static DataGeneratorMetadata CreateDataGeneratorMetadata(Type type, string methodName)
    {
        return new DataGeneratorMetadata
        {
            Type = DataGeneratorType.TestParameters,
            TestBuilderContext = null!,
            TestSessionId = null!,
            MembersToGenerate = null!,
            TestInformation = new SourceGeneratedMethodInformation
            {
                Type = type,
                Name = methodName,
                Attributes = [],
                GenericTypeCount = 0,
                Class = new SourceGeneratedClassInformation
                {
                    Type = type,
                    Assembly = null!,
                    Attributes = [],
                    Name = type.Name,
                    Namespace = null,
                    Parameters = [],
                    Properties = []
                },
                Parameters = [],
                ReturnType = typeof(void),
            }
        };
    }
}