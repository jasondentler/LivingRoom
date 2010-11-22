using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingRoom.Core
{
    public class InvalidParameterValueException : Exception
    {

        private readonly string _message;
        private readonly IEnumerable<KeyValuePair<IParameter, string>> _invalidParams;

        public InvalidParameterValueException(
            IEnumerable<KeyValuePair<IParameter, string>> invalidParams)
        {
            _invalidParams = invalidParams;
        }

        public override string Message
        {
            get
            {
                if (_invalidParams == null || !_invalidParams.Any())
                {
                    return "No invalid parameters.";
                }
                return BuildMessages();
            }
        }

        private string BuildMessages()
        {
            var msgs = _invalidParams.Each(i => string.Format("Parameter {0} has invalid value {1}",
                                                              i.Key.Name, i.Value ?? "null"));
            return string.Join(Environment.NewLine, msgs);
        }

    }
}
