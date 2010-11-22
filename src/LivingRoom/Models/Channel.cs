using System;

namespace LivingRoom.Models
{
    public class Channel
    {
        public string Id { get; set; }
        public Int64 Number { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Icon { get; set; }
    }
}