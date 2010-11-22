namespace LivingRoom
{
    public interface IParameter
    {

        string Name { get; }
        string Description { get; }
        bool IsValid(string value);

    }
}
