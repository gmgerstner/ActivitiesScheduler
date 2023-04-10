namespace ActivitiesScheduler.MainApp
{
    internal class Activity
    {
        public Activity(Section section, Room room, int timeSlot, string facilitator)
        {
            Section = section;
            Room = room;
            TimeSlot = timeSlot;
            Facilitator = facilitator;
        }

        public Section Section { get; set; }
        public Room Room { get; set; }
        public int TimeSlot { get; set; }
        public string Facilitator { get; set; }
    }
}