using System.Collections.Generic;
using System.Linq;

namespace LivingRoom
{
    public abstract class GenericCommand : ICommand
    {
        private readonly string _name;
        private readonly string _description;
        private readonly IParameter[] _parameters;

        protected GenericCommand(string name, string description, params IParameter[] parameters)
        {
            _name = name;
            _description = description;
            _parameters = parameters;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public IEnumerable<IParameter> Parameters
        {
            get { return _parameters; }
        }

        public void Execute(params string[] parameterValues)
        {
            var parameters = BuildParametersMap(parameterValues);
            SendCommand(parameters);
        }

        protected abstract void SendCommand(IDictionary<IParameter, string> parameters);

        protected virtual IDictionary<IParameter, string> BuildParametersMap(string[] parameterValues)
        {
            if (parameterValues.Length != Parameters.Count())
                throw new ParameterCountMismatchException(
                    Name, Parameters.Count(), parameterValues.Length);
            var parameters = Parameters
                .Zip(parameterValues, (k, v) => new {k = k, v = v})
                .ToDictionary(x => x.k, x => x.v);

            var invalid = parameters.Where(kv => !kv.Key.IsValid(kv.Value));

            if (invalid.Any())
                throw new InvalidParameterValueException(invalid);

            return parameters;
        }

    }
}
