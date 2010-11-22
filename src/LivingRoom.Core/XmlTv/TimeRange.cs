using System;

namespace LivingRoom.XmlTv
{
    public class TimeRange
    {

        protected TimeRange()
        {
        }

        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public virtual DateTime Start { get; private set; }
        public virtual DateTime End { get; private set; }
        public virtual TimeSpan Length { get { return End.Subtract(Start); } }

    }
}
