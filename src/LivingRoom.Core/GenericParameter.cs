namespace LivingRoom
{
    public class GenericParameter : IParameter
    {
        private readonly string _name;
        private readonly string _description;

        public GenericParameter(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public virtual bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
