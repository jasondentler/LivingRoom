using System.Collections.Generic;

namespace LivingRoom.Core
{
    public interface IDevice
    {

        string Name { get; }
        IEnumerable<ICommand> Commands { get; }

    }
}
