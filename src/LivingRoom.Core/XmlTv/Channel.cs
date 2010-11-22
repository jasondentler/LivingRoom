using System;

namespace LivingRoom.XmlTv
{
    public class Channel 
    {

        public virtual string Id { get; set; }
        public virtual Int64 Number { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Icon { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Channel);
        }

        public virtual bool Equals(Channel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
