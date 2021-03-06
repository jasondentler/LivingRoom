﻿using System.Collections.Generic;

namespace LivingRoom
{
    public interface IDevice
    {

        string Name { get; }
        IEnumerable<ICommand> Commands { get; }

    }
}
