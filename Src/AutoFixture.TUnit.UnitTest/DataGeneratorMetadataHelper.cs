using System;
using System.Linq;
using System.Reflection;
using TUnit.Core.Enums;

namespace AutoFixture.TUnit.UnitTest;

public class DataGeneratorMetadataHelper
{
    public static DataGeneratorMetadata CreateDataGeneratorMetadata(MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        return CreateDataGeneratorMetadata(methodInfo.ReflectedType ?? methodInfo.DeclaringType!, methodInfo.Name, parameters);
    }

    public static DataGeneratorMetadata CreateDataGeneratorMetadata(Type type, string methodName,
        ParameterInfo[] parameters = null)
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
                Parameters = parameters?.Select(CreateParameter).ToArray() ?? [],
                ReturnType = typeof(void),
            }
        };
    }

    private static SourceGeneratedParameterInformation CreateParameter(ParameterInfo parameterInfo)
    {
        return new SourceGeneratedParameterInformation(parameterInfo.ParameterType)
        {
            Name = parameterInfo.Name,
            Attributes =
            [
            ]
        };
    }
}