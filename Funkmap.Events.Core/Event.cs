using System;
using System.Collections.Generic;

namespace Funkmap.Events.Core
{
    public class Event
    {
        public string Id { get; set; }
        public EventType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime Date { get; set; }
        public string CreatorLogin { get; set; }
        public List<string> Participants { get; set; }
        public string Address { get; set; }



    }

    public enum EventType
    {
        Concert = 1,
        MasterClass = 2,
        Jam = 3
    }
}
