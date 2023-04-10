namespace ActivitiesScheduler.MainApp
{
    internal class Room
    {
        public Room(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}