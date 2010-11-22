namespace LivingRoom.XmlTv
{
    public class Credit : Entity 
    {

        public virtual string Role { get; set; }
        public virtual string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Credit);
        }

        protected virtual bool Equals(Credit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id) ||
                   (Equals(other.Role, Role) && Equals(other.Name, Name));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
