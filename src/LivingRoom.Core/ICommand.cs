using System.Collections.Generic;

namespace LivingRoom
{
    public interface ICommand
    {

        string Name { get; }
        string Description { get; }
        IEnumerable<IParameter> Parameters { get; }
        void Execute(params string[] parameterValues);

    }
}
