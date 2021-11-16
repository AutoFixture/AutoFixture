using System;
using System.Globalization;

namespace AutoFixture.Idioms
{
    public static class GuardClauseCommandExtensions
    {

        public static Exception CreateInvalidParamNameException(this IGuardClauseCommand command, string value,
            ArgumentException innerException)
        {
                return command.CreateException(
                    value,
                    string.Format(CultureInfo.InvariantCulture,
                        "a Guard Clause prevented it; however, the thrown exception contains an invalid parameter name. " +
                        "Ensure you pass the correct parameter name to the ArgumentException constructor.{0}" +
                        "Expected parameter name: {1}{0}Actual parameter name: {2}",
                        Environment.NewLine,
                        command.RequestedParameterName,
                        innerException.ParamName),
                    innerException);
        }
    }
}