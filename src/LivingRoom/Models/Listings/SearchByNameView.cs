using System;

namespace LivingRoom.Models.Listings
{
    public class SearchByNameView
    {

        public string ChannelId;
        public Int64 ChannelNumber;
        public string ChannelLongName;
        public bool HasIcon;
        public string Icon
        {
            get { return HasIcon ? "Listings/ChannelIcon/" + ChannelId : null; }
        }

        public Guid Id;
        public DateTime TimeRangeStart;
        public DateTime TimeRangeEnd;
        public string Title;
        public string EpisodeTitle;
        public string Description;
        public string EpisodeNumber;
        public string[] Attributes;

    }
}