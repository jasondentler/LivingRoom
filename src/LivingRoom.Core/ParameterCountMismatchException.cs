using System;

namespace LivingRoom.Core
{
    public class ParameterCountMismatchException : ApplicationException
    {

        public ParameterCountMismatchException(
            string commandName,
            int expectedCount,
            int actualCount)
            : base(string.Format("{0} expects {1} parameters but was sent {0}.",
                    commandName, expectedCount, actualCount))
        {
        }

    }
}
