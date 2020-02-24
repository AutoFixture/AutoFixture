using System;
using System.Globalization;

namespace AutoFixture.Idioms
{
    public class WhiteSpaceStringBehaviorExpectation : IBehaviorExpectation
    {
        public void Verify(IGuardClauseCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (command.RequestedType != typeof(string))
            {
                return;
            }

            try
            {
                command.Execute(" ");
            }
            catch (ArgumentException e)
            {
                if (string.Equals(e.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
                {
                    return;
                }

                throw command.CreateException(
                    "<white space>",
                    string.Format(CultureInfo.InvariantCulture,
                        "Guard Clause prevented it, however the thrown exception contains invalid parameter name. " +
                        "Ensure you pass correct parameter name to the ArgumentException constructor.{0}" +
                        "Expected parameter name: {1}{0}Actual parameter name: {2}",
                        Environment.NewLine,
                        command.RequestedParameterName,
                        e.ParamName),
                    e);
            }
            catch (Exception e)
            {
                throw command.CreateException("<white space>", e);
            }

            throw command.CreateException("<white space>");
        }
    }
}