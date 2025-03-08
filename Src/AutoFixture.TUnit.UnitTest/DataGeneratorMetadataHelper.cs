using System;
using System.Linq;
using System.Reflection;
using TUnit.Core.Enums;

namespace AutoFixture.TUnit.UnitTest;

public class DataGeneratorMetadataHelper
{
    public static DataGeneratorMetadata CreateDataGeneratorMetadata(Type type, string methodName)
    {
        return CreateDataGeneratorMetadata(type.GetMethod(methodName));
    }
    
    public static DataGeneratorMetadata CreateDataGeneratorMetadata(MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        var type = methodInfo.ReflectedType ?? methodInfo.DeclaringType!;
        var methodName = methodInfo.Name;
        var attributes = methodInfo.GetCustomAttributes().ToArray();
        
        var sourceGeneratedParameterInformations = parameters?.Select(CreateParameter).ToArray() ?? [];

        return new DataGeneratorMetadata
        {
            Type = DataGeneratorType.TestParameters,
            TestBuilderContext = null!,
            TestSessionId = null!,
            MembersToGenerate = sourceGeneratedParameterInformations.Cast<SourceGeneratedMemberInformation>().ToArray(),
            TestInformation = new SourceGeneratedMethodInformation
            {
                Type = type,
                Name = methodName,
                Attributes = attributes ?? [],
                GenericTypeCount = 0,
                Class = new SourceGeneratedClassInformation
                {
                    Type = type,
                    Assembly = null!,
                    Attributes = type.GetCustomAttributes().ToArray(),
                    Name = type.Name,
                    Namespace = null,
                    Parameters = [],
                    Properties = []
                },
                Parameters = sourceGeneratedParameterInformations,
                ReturnType = typeof(void),
            }
        };
    }

    private static SourceGeneratedParameterInformation CreateParameter(ParameterInfo parameterInfo)
    {
        return new SourceGeneratedParameterInformation(parameterInfo.ParameterType)
        {
            Name = parameterInfo.Name!,
            Attributes = parameterInfo.GetCustomAttributes().ToArray()
        };
    }
}