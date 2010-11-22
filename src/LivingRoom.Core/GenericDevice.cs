using System.Collections.Generic;

namespace LivingRoom.Core
{
    public class GenericDevice : IDevice
    {
        private readonly string _name;
        private readonly ICommand[] _commands;

        public GenericDevice(string name, params ICommand[] commands)
        {
            _name = name;
            _commands = commands;
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<ICommand> Commands
        {
            get { return _commands; }
        }

    }
}
