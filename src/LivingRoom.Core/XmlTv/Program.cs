
using Iesi.Collections.Generic;

namespace LivingRoom.XmlTv
{
    public class Program : Entity 
    {

        protected Program()
        {
        }

        public Program(Channel channel, TimeRange timeRange, string title)
            : this()
        {
            Channel = channel;
            TimeRange = timeRange;
            Title = title;
            Attributes = new HashedSet<string>();
            Categories = new HashedSet<string>();
            Credits = new HashedSet<Credit>();
        }

        public virtual Channel Channel { get; set; }
        public virtual TimeRange TimeRange { get; set; }
        public virtual string Title { get; set; }
        public virtual string EpisodeTitle { get; set; }
        public virtual string Description { get; set; }
        public virtual string EpisodeId { get; set; }
        public virtual string EpisodeNumber { get; set; }
        public virtual ISet<string> Attributes { get; set; }
        public virtual ISet<string> Categories { get; set; }
        public virtual ISet<Credit> Credits { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as Program);
        }

        public virtual bool Equals(Program other)
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
