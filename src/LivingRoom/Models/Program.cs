using System;

namespace LivingRoom.Models
{
    public class Program
    {

        public string Id { get; set; }
        public Channel Channel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EpisodeId { get; set; }
        public string EpisodeNumber { get; set; }

    }
}